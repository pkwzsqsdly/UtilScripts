using System.Collections.Generic;
using System.IO;
using System.Text;


public class LocalConstTable
{
	public Dictionary<string, TableSheetData> localConstTable { get; protected set; }
	protected ITableSheetCreator tableSheetCreator;
	public LocalConstTable(ITableSheetCreator creator)
	{
		tableSheetCreator = creator;
		localConstTable = new Dictionary<string, TableSheetData>();
	}
	public static byte[] LoadBytes(string name)
	{
		using (FileStream fs = new FileStream(name, FileMode.Open))
		{
			byte[] bytsize = new byte[fs.Length];
			int defaultSize = 1024 * 256;
			int readLength = bytsize.Length > defaultSize ? defaultSize : bytsize.Length;
			int cursor = 0;
			do
			{
				cursor += fs.Read(bytsize, cursor, readLength);
				readLength = System.Math.Min(bytsize.Length - cursor, readLength);
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
			localConstTable.Add(table.sheetName, table);
		}
	}

	public void LoadTable<T>(string name) where T : ITableCellData, new()
	{
		TableSheetData table = tableSheetCreator?.Create<T>(name);
		if (table != null)
		{
			AddTableData(table);
		}
	}
}