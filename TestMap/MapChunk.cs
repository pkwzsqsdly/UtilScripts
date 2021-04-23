

public class MapChunk 
{
	public enum RotateType
	{
		Rotate0,
		Rotate90,
		Rotate180,
		Rotate270,
		FlipHorizontal,
		FlipVertical
	}
	public enum Direction
	{
		None,
		Top,
		Bottom,
		Left,
		Right
	}
	//空
	public const char MAP_BLOCK_WALL = 'n';
	//路
	public const char MAP_BLOCK_ROAD = 'r';

	public int mapSize {get;private set;}
	public char[,] blocks => _mapBlocks;

	private char[,] _mapBlocks;
	
	public MapChunk(string data,int size)
	{
		var array = data.Trim().ToCharArray();
		mapSize = size;
		_mapBlocks = new char[mapSize,mapSize];
		for (int i = 0; i < array.Length; i++)
		{
			int x = i / mapSize;
			int y = i % mapSize;
			if(x >= mapSize || y >= mapSize) break;
			_mapBlocks[x,y] = array[i];
		}
	}

	//连接点是否吻合
	public bool IsLinkMatch(int link)
	{
		var top = _mapBlocks[link,0];
		if (top == MapChunk.MAP_BLOCK_ROAD)
			return true;

		var left = _mapBlocks[0,link];
		if (left == MapChunk.MAP_BLOCK_ROAD)
			return true;

		var bottom = _mapBlocks[link,mapSize];
		if (bottom == MapChunk.MAP_BLOCK_ROAD)
			return true;

		var right = _mapBlocks[mapSize,link];
		if (right == MapChunk.MAP_BLOCK_ROAD)
			return true;

		return false;
	}

}