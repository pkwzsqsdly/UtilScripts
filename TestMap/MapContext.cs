

public class MapContext 
{
	private MapChunkEntity[,] _mapEntitys;
	private MapChunkTample _mapTample;

	private int _mapLength;
	
// nnnnnnn
// nnnnnnn
// nnnnnnn
// nnnnnnn
// nnnnnnn
// rrrrrrr
// nnnnnnn

	public MapContext(int mapLength)
	{
		_mapLength = mapLength;
		_mapEntitys = new MapChunkEntity[_mapLength,_mapLength];
		_mapTample = new MapChunkTample(9);
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrrrrrrnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample("nnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrrrrrnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnn");
		_mapTample.AddTample("nnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample("nnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnn");
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrnnnnnrrnrnnnnnrnnrnnnnnrnnrrrrrrrnnnnnnnnnn");
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnrrrnnnnnrrnrrnnnrrnnnrnrrnnnnnrrrnnnnnnnrrnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrrrrrrnnnnnnnnn");
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrnnnnrrnnnnnnrrnnnnrrrrnnnnnnnnnnnnnn");
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrrrrnnnnrnnnnnnnnrnnnnrrrrrnnnnnnnnrnnnn");
		_mapTample.AddTample("nnnnrnnnnnnnnrnnnnnnnnrrrrrnnnnrnnnnnnnnrnnnnnnnnrnnnnrrrrrrrnnnnnnnnrnnnnnnnnrnn");
		_mapTample.AddTample("nnnnnnnrnnnnnnnnrnnnnnnnnrnnrrrrrrrrnrnnnnnnnnrnnnnnnnrrnnnnnnnnnnnnnnnnnnnnnnnnn");
		_mapTample.AddTample("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnrrnnnnnnnnrnnnnnnn");
		_mapTample.AddTample("nnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnnnnnnnrnnrrrnnnrrrrnnnnnnnnrnn");
		_mapTample.AddTample("nnnnrnnnnnnnnrnnnnrrrrrrrrrnnnnrnnnnnnnnrnnnnnnnnrnnnnnnrrrrrnnnnrnnnrnnnnrnnnrnn");
		_mapTample.AddTample("nnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrnnnnnnnnrrrnnnnnnnrn");
	}

	public void Generate()
	{

	}

}