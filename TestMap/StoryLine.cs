using System.Collections.Generic;

public class StoryLine {
	private List<Story> _storys;
	private int _depth;
	public StoryLine(int depth = 10)
	{
		_storys = new List<Story>();
		_depth = depth;
	}

	public Story GetStoryWithRandId(int id)
	{
		return new Story();
	}

	//初始化主线路径
	protected void InitStory()
	{
		for (int i = 0; i < _depth; i++)
		{
			var story = GetStoryWithRandId(i);
			story.index = i;
			_storys.Add(story);
		}
	}

	public void Generate()
	{
		_storys.Clear();
		InitStory();

		
	}
}