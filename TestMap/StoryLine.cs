using System.Collections.Generic;

public class StoryLine {
	private List<Story> _storys;
	private List<List<StoryRandomData>> _randList;
	private int _depth;
	public StoryLine(int depth = 10)
	{
		_storys = new List<Story>();
		_randList = new List<List<StoryRandomData>>();
		_depth = depth;
	}

	public void AddRandomData()
	{
		for (int i = 0; i < 10; i++)
		{
			_randList.Add(new List<StoryRandomData>());
		}
		List<StoryRandomData> list = _randList[0];
		int index = 1;
		list.Add(new StoryRandomData(10000,index));

		list = _randList[1];
		list.Add(new StoryRandomData(3334,++index));
		list.Add(new StoryRandomData(3333,++index));
		list.Add(new StoryRandomData(3333,++index));

		list = _randList[2];
		list.Add(new StoryRandomData(3334,++index));
		list.Add(new StoryRandomData(3333,++index));
		list.Add(new StoryRandomData(3333,++index));

		list = _randList[3];
		list.Add(new StoryRandomData(3334,++index));
		list.Add(new StoryRandomData(3333,++index));
		list.Add(new StoryRandomData(3333,++index));


		list = _randList[4];
		list.Add(new StoryRandomData(2500,++index));
		list.Add(new StoryRandomData(2500,++index));
		list.Add(new StoryRandomData(2500,++index));
		list.Add(new StoryRandomData(2500,++index));

		list = _randList[5];
		list.Add(new StoryRandomData(2500,++index));
		list.Add(new StoryRandomData(2500,++index));
		list.Add(new StoryRandomData(2500,++index));
		list.Add(new StoryRandomData(2500,++index));
	}

	public void Generate()
	{

	}
}