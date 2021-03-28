class Dungeon {
  html.CanvasElement _canvas;
  html.CanvasRenderingContext2D _context;

  final int cellSize;
  final int speed;
  int roomTries;
  int maxRoomSize;

  bool _running = false;

  Array2D<int> _floors;

  final _rooms = <Rect>[];
  int _roomTriesLeft;

  int _currentRegion;

  /// Starting position of the current maze.
  int _mazeStartX;
  int _mazeStartY;

  Direction _lastMazeDir;

  /// The open cells for the current maze being generated.
  final _mazeCells = <Vec>[];

  List<Vec> _connectors;

  final _mergeCells = new Queue<Vec>();

  final _openCells = <Vec>[];
  int _deadEndSeek;

  /// Size of the dungeon in cells, not pixels.
  int _dungeonWidth;
  int _dungeonHeight;

  double _scale;

  bool showRegions = true;

  Dungeon(String id,
        {this.cellSize: 6, this.speed: 2, this.roomTries: 200,
        this.maxRoomSize: 5}) {
    _canvas = html.querySelector("#$id") as html.CanvasElement;
    _context = _canvas.context2D;

    var width = 570;
    var height = 390;

    _dungeonWidth = width ~/ cellSize;
    _dungeonHeight = height ~/ cellSize;

    // Handle high-resolution (i.e. retina) displays.
    _scale = html.window.devicePixelRatio;
    _canvas.width = (width * _scale).toInt();
    _canvas.height = (height * _scale).toInt();

    _floors = new Array2D<int>(_dungeonWidth, _dungeonHeight);

    _canvas.onClick.listen((_) {
      reset();
      start();
    });

    reset();
  }

  void start() {
    if (!_running) html.window.requestAnimationFrame(tick);
    _running = true;
  }

  void tick(time) {
    for (var i = 0; i < speed; i++) {
      if (!update()) {
        _running = false;
        return;
      }
    }

    html.window.requestAnimationFrame(tick);
  }

  void reset() {
    _roomTriesLeft = roomTries;
    _rooms.clear();

    _currentRegion = 0;

    _mazeStartX = 1;
    _mazeStartY = 1;
    _lastMazeDir = null;

    _connectors = [];

    /// The open cells for the current maze being generated.
    _mazeCells.clear();
    _mergeCells.clear();

    _openCells.clear();

    for (var pos in _floors.bounds) {
      _carve(pos, CELL_SOLID);
    }
  }

  bool update() {
    return _addRoom() ||
        _growMaze() ||
        _startMaze() ||
        _fillMerge() ||
        _mergeRegion() ||
        _removeDeadEnd();
  }

  bool _addRoom({bool fast: false}) {
    if (_roomTriesLeft <= 0) return false;

    while (--_roomTriesLeft >= 0) {
      var width = rng.range(2, maxRoomSize) * 2 + 1;
      var height = rng.range(2, maxRoomSize) * 2 + 1;

      var x = rng.range(0, (_dungeonWidth - width) ~/ 2) * 2 + 1;
      var y = rng.range(0, (_dungeonHeight - height) ~/ 2) * 2 + 1;

      var room = new Rect(x, y, width, height);

      var overlaps = false;
      for (var other in _rooms) {
        if (room.distanceTo(other) <= 0) {
          overlaps = true;
          break;
        }
      }

      if (overlaps) continue;

      _rooms.add(room);
      _currentRegion++;
      room.forEach(_carve);

      if (!fast) return true;
    }

    return false;
  }

  bool _startMaze() {
    if (_mazeStartY >= _dungeonHeight - 1) return false;

    // Find the next solid place to start a maze.
    while (_floors.get(_mazeStartX, _mazeStartY) != CELL_SOLID) {
      _mazeStartX += 2;
      if (_mazeStartX >= _dungeonWidth - 1) {
        _mazeStartX = 1;
        _mazeStartY += 2;

        // Stop if we've scanned the whole dungeon.
        if (_mazeStartY >= _dungeonHeight - 1) {
          _findConnectors();
          return false;
        }
      }
    }

    _startMazeCell();
    return true;
  }

  void _startMazeCell() {
    var pos = new Vec(_mazeStartX, _mazeStartY);
    _mazeCells.add(pos);
    _currentRegion++;
    _carve(pos);
  }

  /// Implementation of the "growing tree" algorithm from here:
  /// http://www.astrolog.org/labyrnth/algrithm.htm.
  bool _growMaze({bool fast: false}) {
    if (_mazeCells.isEmpty) return false;

    while (_mazeCells.isNotEmpty) {
      var cell = _mazeCells.last;

      // See which adjacent cells are open.
      var openDirs = <Direction>[];

      for (var dir in Direction.CARDINAL) {
        // Must end in bounds.
        if (!_floors.bounds.contains(cell + dir * 3)) continue;

        // Destination must not be open.
        if (_floors[cell + dir * 2] != CELL_SOLID) continue;

        openDirs.add(dir);
      }

      if (openDirs.isEmpty) {
        // No adjacent uncarved cells.
        _mazeCells.removeLast();
        continue;
      }

      var dir;
      if (openDirs.contains(_lastMazeDir) &&
          rng.range(100) > WIGGLE_PERCENT) {
        dir = _lastMazeDir;
      } else {
        dir = rng.item(openDirs);
      }

      _lastMazeDir = dir;

      _carve(cell + dir);
      _carve(cell + dir * 2);

      _mazeCells.add(cell + dir * 2);

      // Made progress, so refresh.
      if (!fast) return true;
    }

    return false;
  }

  void _findConnectors() {
    for (var pos in _floors.bounds.inflate(-1)) {
      // Can't already be part of a region.
      if (_floors[pos] > CELL_SOLID) continue;

      var regions = _getRegionsTouching(pos);
      if (regions.length < 2) continue;

      _connectors.add(pos);
    }

    _connectors.shuffle();

    // Start the merge by turning one room into the merge color.
    _startMerge();
  }

  void _startMerge() {
    _mergeCells.add(_rooms.first.center);
    _carve(_rooms.first.center, CELL_MERGED);
  }

  bool _mergeRegion() {
    if (_connectors.isEmpty) return false;

    // Find a connector that's touching the merged area.
    var connector;
    var merged;
    for (var i = 0; i < _connectors.length; i++) {
      merged = _getRegionsTouching(_connectors[i]);
      if (merged.contains(CELL_MERGED)) {
        connector = _connectors[i];
        _connectors.removeAt(i);
        break;
      }
    }

    // Remove any connectors that aren't needed anymore.
    _connectors.removeWhere((pos) {
      // Don't allow connectors right next to each other.
      if (connector - pos < 2) return true;

      // If the connector no long spans different regions, we don't need it.
      var regions = _getRegionsTouching(pos)
          .where((region) => !merged.contains(region));
      if (regions.isNotEmpty) return false;

      // This connecter isn't needed, but connect it occasionally so that the
      // dungeon isn't singly-connected.
      if (rng.oneIn(50)) _carve(pos, CELL_DOOR);

      return true;
    });

    // Start the merge floodfill.
    _mergeCells.add(connector);
    _carve(connector, CELL_DOOR);
    return true;
  }

  bool _fillMerge() {
    if (_mergeCells.isEmpty) return false;

    while (_mergeCells.isNotEmpty) {
      var pos = _mergeCells.removeFirst();

      for (var dir in Direction.CARDINAL) {
        var here = pos + dir;
        if (_floors[here] <= CELL_SOLID) continue;

        _carve(here, CELL_MERGED);
        _mergeCells.add(here);
      }

      break;
    }

    // Done merging, so get ready to remove the dead ends.
    if (_mergeCells.isEmpty && _connectors.isEmpty) {
      _findOpenCells();
    }

    return true;
  }

  void _findOpenCells() {
    for (var pos in _floors.bounds.inflate(-1)) {
      if (_floors[pos] != CELL_SOLID) _openCells.add(pos);
    }

    _openCells.shuffle();
    _deadEndSeek = 0;
  }

  bool _removeDeadEnd() {
    if (_openCells.isEmpty) return false;

    var start = _deadEndSeek;

    while (true) {
      var pos = _openCells[_deadEndSeek];

      // If it only has one exit, it's a dead end.
      var exits = 0;
      for (var dir in Direction.CARDINAL) {
        if (_floors[pos + dir] != CELL_SOLID) exits++;
      }

      if (exits == 1) {
        _carve(pos, CELL_SOLID);
        _openCells.removeAt(_deadEndSeek);
        if (_deadEndSeek == _openCells.length) _deadEndSeek = 0;
        return true;
      }

      // Move on to the next candidate.
      _deadEndSeek = (_deadEndSeek + 1) % _openCells.length;

      // If we did a full cycle and didn't find a dead end, there must not be
      // any more.
      if (_deadEndSeek == start) {
        _openCells.clear();
        break;
      }
    }

    return false;
  }

  Set<int> _getRegionsTouching(Vec pos) {
    var regions = new Set<int>();
    for (var dir in Direction.CARDINAL) {
      var region = _floors[pos + dir];
      if (region != CELL_SOLID) regions.add(region);
    }

    return regions;
  }

  void _carve(Vec pos, [int value]) {
    if (value == null) {
      if (showRegions) {
        value = _currentRegion;
      } else {
        value = CELL_MERGED;
      }
    }

    _floors[pos] = value;

    if (value > CELL_SOLID) {
      // Open region.
      _drawTile(pos.x, pos.y, TILE_FLOOR);

      _context.fillStyle = 'hsla(${value * 17 % 360}, 100%, 20%, 0.5)';
      _context.fillRect(
          pos.x * cellSize * _scale, pos.y * cellSize * _scale,
          cellSize * _scale, cellSize * _scale);
    } else if (value == CELL_SOLID) {
      // Draw the wall.
      if (pos.y < _dungeonHeight - 1 &&
          _floors.get(pos.x, pos.y + 1) != CELL_SOLID) {
        _drawTile(pos.x, pos.y, TILE_WALL);
      } else {
        _drawTile(pos.x, pos.y, TILE_SOLID);
      }

      // If the tile above this was a wall, it's solid now.
      if (pos.y > 0 && _floors.get(pos.x, pos.y - 1) == CELL_SOLID) {
        _drawTile(pos.x, pos.y - 1, TILE_SOLID);
      }
    } else if (value == CELL_DOOR) {
      // Draw the door.
      if (_floors.get(pos.x - 1, pos.y) == CELL_SOLID) {
        _drawTile(pos.x, pos.y, TILE_H_DOOR);
      } else {
        _drawTile(pos.x, pos.y, TILE_V_DOOR);
      }

      // Make solid above this a wall.
      if (pos.y > 0 && _floors.get(pos.x, pos.y - 1) == CELL_SOLID) {
        _drawTile(pos.x, pos.y - 1, TILE_WALL);
      }
    } else {
      _drawTile(pos.x, pos.y, TILE_FLOOR);

      // Make solid above this a wall.
      if (pos.y > 0 && _floors.get(pos.x, pos.y - 1) == CELL_SOLID) {
        _drawTile(pos.x, pos.y - 1, TILE_WALL);
      }
    }
  }

  void _drawTile(int x, int y, int tile) {
    _context.drawImageScaledFromSource(_tileset,
        tile * TILE_SIZE, 0, TILE_SIZE, TILE_SIZE,
        x * cellSize * _scale, y * cellSize * _scale,
        cellSize * _scale, cellSize * _scale);
  }

   void _drawConnector(Vec pos) {
     _context.fillStyle = '#ca8';
     _context.fillRect(
        (pos.x * cellSize + 2) * _scale,
        (pos.y * cellSize + 2 ) * _scale,
        (cellSize - 4) * _scale, (cellSize - 4) * _scale);
   }
}