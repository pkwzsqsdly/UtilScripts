using LitJson;

public class JsonTableSheetData : ITableSheetCreator
{
	public TableSheetData Create<T>(string name) where T : ITableCellData,new()
	{
		TableSheetData table = new TableSheetData(name);

		JsonData json = JsonMapper.ToObject(LocalConstTable.LoadString(name));
		for (int i = 0; i < json.Count; i++)
		{
			JsonData data = json[i];
			T t = JsonMapper.ToObject<T>(data.ToJson());
			table.AddData(t);
		}
		return table;
	}
}