
public class Choose {
	public bool canChoose => _chooseNum == -1 || _chooseNum > 0;
	private int _chooseNum;
	private int _chooseEffect;

	public Story nextStory;

	public Choose()
	{
		_chooseNum = -1;
		_chooseEffect = 0;
	}

	public void Chosen()
	{
		if (canChoose)
		{
			if (_chooseNum > 0)
			{
				_chooseNum --;
			}
			//TODO: 事件跟上
		}
	}
}