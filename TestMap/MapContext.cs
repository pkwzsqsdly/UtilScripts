

public class MapContext 
{
	private MapChunkTample _mapTample;

	
	public MapContext()
	{
		_mapTample = new MapChunkTample(9);
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrrrrrrnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample(@"nnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrrrrrnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnn");
		_mapTample.AddTample(@"nnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample(@"nnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnn");
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrnnnnnrrnrnnnnnrnnrnnnnnrnnrrrrrrrnnnnnnnnnn");
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnrrrnnnnnrrnrrnnnrrnnnrnrrnnnnnrrrnnnnnnnrrnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrrrrrrnnnnnnnnn");
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrnnnnrrnnnnnnrrnnnnrrrrnnnnnnnnnnnnnn");
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrrnnnnrnnnnnnnnrnnnnrrrrrnnnnnnnnrnnnn");
		_mapTample.AddTample(@"nnnnrnnnnnnnnrnnnnnnnnrrrrrnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrrrnnnnnnnnrnnnnnnnnrnn");
		_mapTample.AddTample(@"nnnnnnnrnnnnnnnnrnnnnnnnnrnnrrrrrrrrnrnnnnnnnnrnnnnnnnrrnnnnnnnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample(@"nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrnnnnnnnnrnnnnnnn");
		_mapTample.AddTample(@"nnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnrrrnnnrrrrnnnnnnnnrnn");
		_mapTample.AddTample(@"nnnnrnnnnnnnnrnnnnrrrrrrrrrnnnnrnnnnnnnnrnnnnnnnnrnnnnnnrrrrrnnnnrnnnrnnnnrnnnrnn");
		_mapTample.AddTample(@"nnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrrnnnnnnnrn");
	}

	public void Generate()
	{
		var map = RandomInList();
		System.Console.WriteLine(map.ToString());
	}

	public MapChunkEntity RandomInList()
	{
		var ran = new System.Random();
		int len = _mapTample.Count;
		int index = ran.Next(0,len);
		int dir = ran.Next(0,4);
		System.Console.WriteLine($"map = {index},{((MapChunk.RotateType)(dir+1))}");
		var map = new MapChunkEntity(_mapTample.GetTample(index),Coord2d.zero);
		map.Rotate((MapChunk.RotateType)(dir+1));
		return map;
	}

}