using System.Collections.Generic;

public class MapChunkTample
{
	public int Count => _tamples.Count;
	private List<MapChunk> _tamples;
	private int _mapSize;

	public MapChunkTample(int size)
	{
		_tamples = new List<MapChunk>();
		_mapSize = size;
	}

	public void AddTample(string data)
	{
		var mapChunk = new MapChunk(data,_mapSize);
		_tamples.Add(mapChunk);
	}
	public MapChunk GetTample(int index)
	{
		return _tamples[index];
	}
	public MapChunk GetMapWithLink(Coord2d coord)
	{
		int link = 0;
		if (!IsOutsideIndex(coord.x))
			link = coord.x;
		else if (!IsOutsideIndex(coord.y))
			link = coord.y;
		
		if(link == 0) return null;

		for (int i = 0; i < _tamples.Count; i++)
		{
			var chunk = _tamples[i];
			if (chunk.IsLinkMatch(coord.x))
			{
				return chunk;
			}
		}
		
		return null;
	}

	private bool IsOutsideIndex(int val)
	{
		return val > 0 && val < _mapSize - 1;
	}
}