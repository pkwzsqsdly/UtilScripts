using System.Collections;
using System.Collections.Generic;


public class ProbabilityGroup<T> : IProbabilityItem where T : IProbabilityItem
{
	protected List<T> probList;

	public ProbabilityGroup()
	{
		probList = new List<T>();
	}

	public void Add(T t)
	{
		probList.Add(t);
	}

	public void Remove(T t)
	{
		probList.Remove(t);
	}
	public void RemoveAt(int index)
	{
		probList.RemoveAt(index);
	}

	public int GetResult()
	{
		for (int i = 0; i < probList.Count; i++)
		{
			int val = probList[i].GetResult();
			if(val > 0) return val;
		}
		return 0;
	}

}