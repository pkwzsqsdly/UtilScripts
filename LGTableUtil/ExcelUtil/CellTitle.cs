namespace LG.TableUtil
{
	public class CellTitle {
		public string Name { get; private set; }
		public string KeyWords { get; private set; }
		public string ValueType { get; private set; }
		public bool IsExport { get; private set; }

		public CellTitle(string name,string keyword,string valueType,bool isExport)
		{
			Name = name;
			KeyWords = keyword;
			ValueType = valueType;
			IsExport = isExport;
		}

		public bool IsIgnore()
		{
			return !IsExport || IsNull();
		}
		public bool IsNull()
		{
			return (string.IsNullOrEmpty(KeyWords) || string.IsNullOrEmpty(ValueType)) && string.IsNullOrEmpty(Name);
		}
	}
}