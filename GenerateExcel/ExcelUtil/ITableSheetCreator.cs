public interface ITableSheetCreator
{
	TableSheetData Create<T>(string name) where T : ITableCellData,new();
}