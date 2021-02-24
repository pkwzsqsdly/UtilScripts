

public class BinTableSheetData : ITableSheetCreator
{
	public TableSheetData Create<T>(string name) where T : ITableCellData,new()
	{
		TableSheetData table = new TableSheetData(name);

		ByteArray bArr = new ByteArray(LocalConstTable.LoadBytes(name));
		do {
			IBinRead t = new T() as IBinRead;
			t.Read(bArr);
			table.AddData(t);
		} while (!bArr.IsReadDone);

		return table;
	}
}