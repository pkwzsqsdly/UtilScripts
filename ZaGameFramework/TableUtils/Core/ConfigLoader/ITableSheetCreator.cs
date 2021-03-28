using LG.TableUtil.FileOperational;

namespace LG.TableUtil.Config
{
	public interface ITableSheetCreator
	{
		string tableFilePath{get;}
		IFileOperational fileOperational{get;}
		TableSheetData Create<T>(string name) where T : ITableCellData,new();
	}
}