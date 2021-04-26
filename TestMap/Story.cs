using System.Collections.Generic;

public class Story {
	private List<Choose> _chooses;

	//路程距离
	private int _distance;
	//当前这段路的随机组id  决定遇敌几率 怪物 物品 隐藏
	private int _randomId;
	public Story()
	{
		_chooses = new List<Choose>();
	}

	public void ShowChooses(System.Action<Choose> action)
	{
		for (int i = 0; i < _chooses.Count; i++)
		{
			var choose = _chooses[i];
			if(choose.canChoose)
			{
				action(choose);
			}
		}
	}

	public void MakeChoose(Choose choose)
	{
		choose.Chosen();
	}
}