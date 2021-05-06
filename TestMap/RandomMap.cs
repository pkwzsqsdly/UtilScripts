
using System;
using System.Collections.Generic;

public class RandomMap
{
    public Tile2d[,] currentMap;
    public int size;
    private Random _random;
    private Tile2d _startCoord;
    //转向几率
    private int _turnProbability = 20;

    private List<Tile2d> _turnPoint;

    public RandomMap(int size)
    {
        _random = new Random();
        this.size = size;
        currentMap = new Tile2d[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                currentMap[i, j] = new Tile2d(i, j);
            }
        }

        _turnPoint = new List<Tile2d>(size * size);
    }


    public void RandomStart()
    {
        int startX = _random.Next(0, size);
        int startY = _random.Next(0, size);

        _startCoord = new Tile2d(startX, startY);
    }

    public bool IsTurnDirection()
    {
        return _random.Next(0, 100) < _turnProbability;
    }

    public void Generate()
    {
        var pos = _startCoord.coord2d;
        while (true)
        {
            var coord = RandomDir(pos);
            var next = GetTile(coord);
            if (_turnPoint.IndexOf(next) >= 0)
            {
                continue;
            }
            _turnPoint.Add(next);
        }
    }

    public Coord2d RandomDir(Coord2d coord)
    {
        List<Coord2d> randCoord = new List<Coord2d>(4);
        var top = coord.top;
        var bottom = coord.bottom;
        var left = coord.left;
        var right = coord.right;
        
        if (!IsOutSideMap(top))
        {
            randCoord.Add(top);
        }
        if (!IsOutSideMap(bottom))
        {
            randCoord.Add(bottom);
        }
        if (!IsOutSideMap(left))
        {
            randCoord.Add(left);
        }
        if (!IsOutSideMap(right))
        {
            randCoord.Add(right);
        }

        int ran = _random.Next(0, randCoord.Count);
        return randCoord[ran];
    }

    public bool IsOutSideMap(Coord2d coord)
    {
        if (coord.x < 0 || coord.y < 0 || coord.x >= size || coord.y >= size)
        {
            return true;
        }
        return false;
    }

    public Tile2d GetTile(Coord2d coord2d)
    {
        return currentMap[coord2d.x,coord2d.y];
    }
}