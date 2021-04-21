using System.Collections.Generic;
using System.Text;

public class MapChunkEntity
{
	public MapChunk.RotateType RotateType{get;private set;}
	private MapChunk _chunk;
	private Dictionary<MapChunk.Direction,List<Coord2d>> _linkDic;
	private char[,] _currBlocks;
	public MapChunkEntity(MapChunk chunk)
	{
		_chunk = chunk;
		_linkDic = new Dictionary<MapChunk.Direction, List<Coord2d>>();
		Rotate(MapChunk.RotateType.Rotate0);
	}

	private char[,] ToNewArray(System.Func<int,int,char> getBlockAction)
	{
		char[,] newMap = new char[_chunk.mapSize,_chunk.mapSize];
		var array = _chunk.blocks;
		for (int i = 0; i < array.GetLength(0); i++)
		{
			for (int j = 0; j < array.GetLength(1); j++)
			{
				newMap[i,j] = getBlockAction(i,j);
			}
		}
		return newMap;
	}

	public void Rotate(MapChunk.RotateType rot)
	{
		var array = _chunk.blocks;
		int size = _chunk.mapSize - 1;
		switch(rot)
		{
			case MapChunk.RotateType.Rotate90:
				_currBlocks = ToNewArray((i,j) => array[j,size - i]);
				break;
			case MapChunk.RotateType.Rotate180:
				_currBlocks = ToNewArray((i,j) => array[size - i,size - j]);

				break;
			case MapChunk.RotateType.Rotate270:
				_currBlocks = ToNewArray((i,j) => array[size - j,i]);

				break;
			case MapChunk.RotateType.FlipHorizontal:
				_currBlocks = ToNewArray((i,j) => array[i,size - j]);

				break;
			case MapChunk.RotateType.FlipVertical:
				_currBlocks = ToNewArray((i,j) => array[size - i,j]);
				break;
			default:
				_currBlocks = array;
				break;
		}
		SeekLink();
	}
	private void SeekLink()
	{
		_linkDic.Clear();

		var left = FindLink(_chunk.mapSize,i => {
			if(MapChunk.MAP_BLOCK_ROAD == _currBlocks[0,i])
				return new Coord2d(0,i);
			return Coord2d.zero;
		});
		_linkDic.Add(MapChunk.Direction.Left,left);

		var right = FindLink(_chunk.mapSize,i => {
			if(MapChunk.MAP_BLOCK_ROAD == _currBlocks[_chunk.mapSize - 1,i])
				return new Coord2d(_chunk.mapSize - 1,i);
			return Coord2d.zero;
		});
		_linkDic.Add(MapChunk.Direction.Right,right);


		var top = FindLink(_chunk.mapSize,i => {
			if(MapChunk.MAP_BLOCK_ROAD == _currBlocks[i,0])
				return new Coord2d(i,0);
			return Coord2d.zero;
		});
		_linkDic.Add(MapChunk.Direction.Top,top);


		var bottom = FindLink(_chunk.mapSize,i => {
			if(MapChunk.MAP_BLOCK_ROAD == _currBlocks[i,_chunk.mapSize - 1])
				return new Coord2d(i,_chunk.mapSize - 1);
			return Coord2d.zero;
		});
		_linkDic.Add(MapChunk.Direction.Bottom,bottom);

		StringBuilder sb = new StringBuilder();
		foreach (var item in _linkDic)
		{
			sb.Append(item.Key);
			sb.Append("=");
			for (int i = 0; i < item.Value.Count; i++)
			{
				sb.Append(item.Value[i].ToString());
			}
			sb.Append("\n");
		}
		System.Console.WriteLine(sb.ToString());
	}
	public List<Coord2d> GetLinks(MapChunk.Direction dir)
	{
		if(_linkDic.ContainsKey(dir))
		{
			return _linkDic[dir];
		}
		return null;
	}

	private List<Coord2d> FindLink(int len,System.Func<int,Coord2d> checkFunc)
	{
		var list = new List<Coord2d>();
		for (int i = 0; i < len; i++)
		{
			var coord = checkFunc(i);
			if(coord.mix == 0) continue;
			
			list.Add(coord);
		}
		return list;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < _currBlocks.GetLength(0); i++)
		{
			for (int j = 0; j < _currBlocks.GetLength(1); j++)
			{
				sb.Append(_currBlocks[i,j]);
			}
			sb.Append("\n");
		}
		return sb.ToString();
	}

}