

using System;
using System.Collections.Generic;
using System.Text;

public class StoryRandomData
{
	public int storyId;
	public int storyType;
	
	private List<StoryRandomData> _linkDatas;
	private int _chooseNum;
	private List<int> _canLinkTypes;

	public StoryRandomData(int storyId,int storyType)
	{
		this.storyId = storyId;
		this.storyType = storyType;
		_chooseNum = new Random().Next(1, 4);
		_linkDatas = new List<StoryRandomData>();
		_canLinkTypes = new List<int>();
	}

	public StoryRandomData InitLinkTypes(params int[] ids)
	{
		_canLinkTypes.AddRange(ids);
		return this;
	}
	public StoryRandomData InitLinkTypes(List<int> ids)
	{
		_canLinkTypes.AddRange(ids);
		return this;
	}
	public StoryRandomData AddLinkType(int id)
	{
		_canLinkTypes.Add(id);
		return this;
	}

	public bool IsCanLink(int type)
	{
		Console.WriteLine(_chooseNum + "=IsCanLink=" + _canLinkTypes.Count);
		return _chooseNum > _linkDatas.Count && _canLinkTypes.FindIndex(x => x == type) >= 0;
	}

	public void LinkStoryData(StoryRandomData data)
	{
		if (!_linkDatas.Contains(data))
		{
			_linkDatas.Add(data);
		}
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("[");
		for (int i = 0; i < _linkDatas.Count; i++)
		{
			sb.Append(_linkDatas[i].storyId);
			sb.Append(",");
		}
		sb.Append("]");
		
		return $"(id={storyId},st={storyType},link={sb.ToString()})";
	}
}