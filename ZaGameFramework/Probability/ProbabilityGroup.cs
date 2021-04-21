using System.Collections.Generic;

namespace LG.Probability
{
	public class ProbabilityGroup : Probability
	{
		public string groupName{get;protected set;}
		public override ProbabilityType probabilityType => ProbabilityType.Group;
		protected List<IProbabilityItem> probList;
		protected IProbabilityPattern pattern;

		public ProbabilityGroup(string name,int chance = 0,int mark = -1):base(chance,mark)
		{
			pattern = new ProbabilityPatternNList();
			groupName = name;
			probList = new List<IProbabilityItem>();
		}
		public ProbabilityGroup(int chance = 0,int mark = -1):this(null,chance,mark) {}

		public void Add(IProbabilityItem t)
		{
			probList.Add(t);
		}

		public void Remove(IProbabilityItem t)
		{
			probList.Remove(t);
		}
		public void RemoveAt(int index)
		{
			probList.RemoveAt(index);
		}

		public IProbabilityItem GetResult()
		{
			pattern.Restore();
			for (int i = 0; i < probList.Count; i++)
			{
				var item = probList[i];
				int c = item.chance;
				
				if(c == 0)  continue;
				
				if (pattern.IsHit(c))
				{
					if(item.probabilityType == ProbabilityType.Group)
					{
						return (item as ProbabilityGroup).GetResult();
					}
					return item;
				}
				
			}
			return null;
		}
	}
}