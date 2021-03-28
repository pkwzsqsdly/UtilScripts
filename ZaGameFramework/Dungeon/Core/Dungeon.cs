using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using Bramble.Core;

namespace Amaranth.Engine
{
	[Serializable]
	public class Dungeon
	{
		public readonly GameEvent<Dungeon, TileEventArgs> TileChanged = new GameEvent<Dungeon, TileEventArgs>();

		public Array2D<int> Regions { get { return regions; } }
		public Array2D<Tile> Tiles { get { return tiles; } }

		public Rect Bounds { get { return Tiles.Bounds; } }

		public List<Rect> Rooms { get { return mRooms; } }
		public Dungeon(int width, int height)
		{
			// mGame = game;

			regions = new Array2D<int>(width, height);
			tiles = new Array2D<Tile>(width, height);
			mRooms = new List<Rect>();

			// mItems = new ItemCollection(this);
			// mItems.ItemAdded.Add(Items_ItemAdded);
			// mItems.ItemRemoved.Add(Items_ItemRemoved);

			// mEntities = new EntityCollection(this);
			// mEntities.EntityAdded.Add(Entities_EntityAdded);
			// mEntities.EntityRemoved.Add(Entities_EntityRemoved);
		}

		// public Dungeon(Game game) : this(game, 100, 80) { }

		public void Generate(bool isDescending, int depth)
		{

			// fill the dungeon with default Regions
			tiles.Fill((pos) => new Tile(TileType.Wall));

			AddRooms();

			// place the hero on the stairs
			//### bob: need to handle placing on portals
			for (var y = 1; y < Bounds.Height; y += 2) {
				for (var x = 1; x < Bounds.Width; x += 2) {
					var pos = new Vec(x, y);
					if (GetTile(pos).Type != TileType.Wall)
						continue;
					GrowMaze(pos);
				}
			}
			
			ConnectRegions();
			RemoveDeadEnds();
			Rooms.ForEach(OnDecorateRoom);
			print();
		}
		private void print()
		{
			StreamWriter sw = new StreamWriter("test.txt");
			for (int i = 0; i < this.Tiles.Width; i++)
			{
				StringBuilder sbrow = new StringBuilder();
				for (int j = 0; j < this.Tiles.Height; j++)
				{
					string str = "";
					switch (this.Tiles[i,j].Type)
					{
						case TileType.Floor:
							str = "📉 ";
							break;
						case TileType.Wall:
							str = "📜 ";
							break;
						case TileType.DoorOpen:
							str = "🚪 ";
							break;
						case TileType.DoorClosed:
							str = "🔒 ";
							break;
					}
					sbrow.Append(str);
				}
				sw.WriteLine(sbrow.ToString());
				// UserSetting.Log(sbrow.ToString());
			}
			sw.Flush();
			sw.Close();
		}

		private void OnDecorateRoom(Rect room)
		{
		}

		private void RemoveDeadEnds()
		{
            var done = false;

            while (!done) {
				done = true;

				foreach (var pos in Bounds.Inflate(-1)) {
					if (GetTile(pos).Type == TileType.Wall) continue;

					// If it only has one exit, it's a dead end.
					var exits = 0;
					foreach (var dir in Direction.Nsew) {
						if (GetTile(pos + dir).Type != TileType.Wall) 
							exits++;
					}

					if (exits != 1) continue;

					done = false;
					SetTile(pos, TileType.Wall);
				}
            }
		}

		private void ConnectRegions()
		{
            var connectorRegions = new Dictionary<Vec, List<int>>();
            foreach (var pos in Bounds.Inflate(-1)) {
                  // Can't already be part of a region.
				if (GetTile(pos).Type != TileType.Wall) continue;

				var reg = new List<int>();
				foreach (var dir in Direction.Nsew) {
					var region = Regions[pos + dir];
					if (reg.IndexOf(region) < 0) 
						reg.Add(region);
				}

				if (reg.Count < 2) 
					continue;
				if(!connectorRegions.ContainsKey(pos))
				{
					connectorRegions.Add(pos,reg);
				}else{
					connectorRegions[pos] = reg;
				}
            }

            var connectors = connectorRegions.Keys.ToList();

            // Keep track of which regions have been merged. This maps an original
            // region index to the one it has been merged to.
            int[] merged = new int[currentRegion + 1];
			List<int> openRegions = new List<int>();
			for (var i = 0; i <= currentRegion; i++) {
                merged[i] = i;
				if(openRegions.IndexOf(i) < 0)
                	openRegions.Add(i);
            }

            // Keep connecting regions until we're down to one.
            while (openRegions.Count > 1) {
				
				var connector = Rng.Item(connectors);

				// Carve the connection.
				AddJunction(connector);

				// Merge the connected regions. We'll pick one region (arbitrarily) and
				// map all of the other regions to its index.
				var regions = Map(connectorRegions[connector],region => merged[region]);// .ToDictionary((region) => merged[region]);
				var dest = regions.First();
				var sources = regions.Skip(1).ToList();

				// Merge all of the affected regions. We have to look at *all* of the
				// regions because other regions may have previously been merged with
				// some of the ones we're merging now.
				for (var i = 0; i <= currentRegion; i++) {
					if (sources.IndexOf(merged[i]) >= 0) {
					    merged[i] = dest;
					}
				}

				// The sources are no longer in use.
				openRegions.RemoveAll(x => sources.IndexOf(x) >= 0);

				// Remove any connectors that aren't needed anymore.
				connectors.RemoveAll((pos) => {
							// Don't allow connectors right next to each other.
					if ((connector - pos).LengthSquared < 4) 
						return true;

					// If the connector no long spans different regions, we don't need it.
					var reg = Map(connectorRegions[pos],region => merged[region]);//.ToDictionary((region) => merged[region]);//.toSet();

					if (reg.Count > 1) 
						return false;

					// This connecter isn't needed, but connect it occasionally so that the
					// dungeon isn't singly-connected.
					if (extraConnectorChance > 0 && Rng.OneIn(extraConnectorChance)) 
						AddJunction(pos);

					return true;
				});
			}
		}
		private List<T> Map<T>(List<T> list,Func<T,T> func)
		{
			List<T> newList = new List<T>();
			for(var i = 0; i < list.Count; i++){
				T t = func(list[i]);
				if(newList.IndexOf(t) < 0)
					newList.Add(t);
			}
			return newList;
		}
		private void AddJunction(Vec pos)
		{
            if (Rng.OneIn(4)) {
                SetTile(pos, Rng.OneIn(3) ? TileType.DoorOpen : TileType.Floor);
            } else {
                SetTile(pos, TileType.DoorClosed);
            }
		}

		private void GrowMaze(Vec start)
		{
			var cells = new List<Vec>();
			Direction lastDir = Direction.None;

			StartRegion();
			Carve(start);

			cells.Add(start);
			while (cells.Count > 0)
			{
				var cell = cells[cells.Count - 1];

				// See which adjacent cells are open.
				var unmadeCells = new List<Direction>();

				foreach (var dir in Direction.Nsew)
				{
					if (CanCarve(cell, dir))
						unmadeCells.Add(dir);
				}

				if (unmadeCells.Count > 0)
				{
					// Based on how "windy" passages are, try to prefer carving in the
					// same direction.
					Direction dir;
					if (unmadeCells.IndexOf(lastDir) > 0
						&& Rng.Int(100) > windingPercent)
					{
						dir = lastDir;
					}
					else
					{
						dir = Rng.Item(unmadeCells);
					}

					Carve(cell + dir);
					Carve(cell + dir.Offset * 2);

					cells.Add(cell + dir.Offset * 2);
					lastDir = dir;
				}
				else
				{
					// No adjacent uncarved cells.
					cells.RemoveAt(cells.Count - 1);

					// This path has ended.
					lastDir = Direction.None;
				}
			}
		}

		private bool CanCarve(Vec pos, Direction direction)
		{
			if (!Bounds.Contains(pos + direction.Offset * 3)) return false;
            // Destination must not be open.
            return GetTile(pos + direction.Offset * 2).Type == TileType.Wall;
		}

		private void Carve(Vec pos,TileType val = TileType.Floor)
		{
			// if (val == -1) val = 0;
            SetTile(pos, val);
            Regions[pos] = currentRegion;
		}

		private void SetTile(Vec pos, TileType val)
		{
			// if(val > 0)
			// 	UnityEngine.Debug.Log(pos + "-" + val);
			tiles[pos].Type = val;
		}

		private void StartRegion()
		{
			currentRegion++;
		}

		private Tile GetTile(Vec pos)
		{
			return tiles[pos];
		}
		public void SetTileVisible(Vec pos,bool isVisiable)
		{

		}
		private void AddRooms()
		{
			for (var i = 0; i < numRoomTries; i++)
			{
				// Pick a random room size. The funny math here does two things:
				// - It makes sure rooms are odd-sized to line up with maze.
				// - It avoids creating rooms that are too rectangular: too tall and
				//       narrow or too wide and flat.
				// TODO: This isn't very flexible or tunable. Do something better here.
				var size = Rng.Int(2, roomExtraSize) * 2 + 1;
				var rectangularity = Rng.Int(0, 1 + size / 2) * 2;
				var width = size;
				var height = size;
				// if (Rng.OneIn(2))
				// {
				// 	width += rectangularity;
				// }
				// else
				// {
				// 	height += rectangularity;
				// }

				var x = Rng.Int((Bounds.Width - width) / 2) * 2 + 1;
				var y = Rng.Int((Bounds.Height - height) / 2) * 2 + 1;

				var room = new Rect(x, y, width, height);

				var overlaps = false;
				foreach (var other in Rooms)
				{
					var sz = Rect.Intersect(room, other).Size;
					if (sz.X > 0 && sz.Y > 0)
					{
						overlaps = true;
						break;
					}
				}
				if (overlaps) continue;

				Rooms.Add(room);

				StartRegion();
				// var test = new Rect(x, y, width, height);
				foreach (var pos in room)
				{
					Carve(pos);
				}
			}
		}


		private readonly Array2D<int> regions;
		private readonly Array2D<Tile> tiles;
		private readonly List<Rect> mRooms;
		private int roomExtraSize = 5;
		private int windingPercent = 50;
		private int numRoomTries = 200;
        
        /// The index of the current region being carved.
        private int currentRegion = 0;
        /// The inverse chance of adding a connector between two regions that have
        /// already been joined. Increasing this leads to more loosely connected
        /// dungeons.
        private int extraConnectorChance = 0;
	}
}
