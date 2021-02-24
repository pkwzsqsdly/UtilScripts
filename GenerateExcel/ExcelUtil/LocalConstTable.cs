using System.Collections.Generic;
using System.IO;
using System.Text;

public class TableSheetData
{
	public Dictionary<int,int> tableCellDic;
	public string sheetName{get;protected set;}
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
		if(!tableCellDic.ContainsKey(data.Id)){
			tableCellDic.Add(data.Id,val);
			tableCells.Add(data);
		}else throw new System.Exception($"Data id {data.Id} is exsit in {sheetName}!");
	}
	public ITableCellData GetDataById(int id) 
	{
		if(tableCellDic.ContainsKey(id))
			return GetDataByIndex(tableCellDic[id]);
		return null;
	}

	public ITableCellData GetDataByIndex(int index)
	{
		if (index >= 0 && index < tableCells.Count)
			return tableCells[index];
		return null;
	}
	public T GetDataById<T>(int id) where T : class,ITableCellData
	{
		return GetDataById(id) as T;
	}

	public T GetDataByIndex<T>(int index) where T : class,ITableCellData
	{
		return GetDataByIndex(index) as T;
	}
}
public class LocalConstTable
{
	public Dictionary<string,TableSheetData> localConstTable {get;protected set;}
	protected ITableSheetCreator tableSheetCreator;
	public LocalConstTable(ITableSheetCreator creator)
	{
		tableSheetCreator = creator;
		localConstTable = new Dictionary<string, TableSheetData>();
	}
	public static byte[] LoadBytes(string name)
	{
		using(FileStream fs = new FileStream(name,FileMode.Open))
		{
			byte[] bytsize = new byte[fs.Length];
			int readLength = bytsize.Length > 2048 ? 2048 : bytsize.Length;
			int cursor = 0;
			do{	
				cursor += fs.Read(bytsize, cursor, readLength);
				// int res = bytsize.Length - cursor;
				readLength = System.Math.Min(bytsize.Length - cursor,readLength);
				// if(res < readLength)
				// 	readLength = res;
			} 
			while (cursor < bytsize.Length);
			return bytsize;
		}
	}

	public static string LoadString(string name)
	{
		byte[] bts = LoadBytes(name);
		return Encoding.Default.GetString(bts);
	}

	public void AddTableData(TableSheetData table)
	{
		if (!localConstTable.ContainsKey(table.sheetName))
		{
			localConstTable.Add(table.sheetName,table);
		}
	}

	public void LoadTable<T>(string name) where T : ITableCellData,new()
	{
		TableSheetData table = tableSheetCreator?.Create<T>(name);
		if (table != null)
		{
			AddTableData(table);
		}
	}
}