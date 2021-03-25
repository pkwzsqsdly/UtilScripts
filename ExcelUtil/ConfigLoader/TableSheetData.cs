using System.Collections.Generic;

public class TableSheetData
{
	public Dictionary<int, int> tableCellDic;
	public string sheetName { get; protected set; }

	public ITableCellData this[int id] { get => GetDataById(id);}

	public List<ITableCellData> tableCells;
	public TableSheetData(string name)
	{
		sheetName = name;
		tableCellDic = new Dictionary<int, int>();
		tableCells = new List<ITableCellData>();
	}

	public void AddData(ITableCellData data)
	{
		int val = tableCells.Count;
		if (!tableCellDic.ContainsKey(data.Id))
		{
			tableCellDic.Add(data.Id, val);
			tableCells.Add(data);
		}
		else throw new System.Exception($"Data id {data.Id} is exsit in {sheetName}!");
	}
	public ITableCellData GetDataById(int id)
	{
		if (tableCellDic.ContainsKey(id))
			return GetDataByIndex(tableCellDic[id]);
		return null;
	}

	public ITableCellData GetDataByIndex(int index)
	{
		if (index >= 0 && index < tableCells.Count)
			return tableCells[index];
		return null;
	}
	public T GetDataById<T>(int id) where T : class, ITableCellData
	{
		return GetDataById(id) as T;
	}

	public T GetDataByIndex<T>(int index) where T : class, ITableCellData
	{
		return GetDataByIndex(index) as T;
	}
}