namespace LG.TableUtil
{

	public class CellData
	{
		public string InnerValue { get; private set; }
		public CellTitle Title {get;private set; }

		public CellData(CellTitle title,string value)
		{
			Title = title;
			InnerValue = value;
		}

	}
}