using System.IO;

public class CellData
{
	public string Description { get; private set; }
	public string KeyWords { get; private set; }
	public string ValueType { get; private set; }
	public bool IsExport { get; private set; }
	public bool IsTranslate { get; private set; }
	public string InnerValue { get; private set; }

	public CellData(string desc,string kwords,string vType,bool isExport,bool isTrans,string value)
	{
		Description = desc;
		KeyWords = kwords;
		ValueType = vType;
		IsExport = isExport;
		IsTranslate = isTrans;
		InnerValue = value;
	}

}