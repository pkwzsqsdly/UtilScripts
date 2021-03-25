using LG.TableUtil.Config;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil.Bin
{
	public class BinTableSheetData : ITableSheetCreator
	{
		public string tableFilePath {get;private set;}
		public IFileOperational fileOperational{get;private set;}

		public BinTableSheetData(string path,IFileOperational fileOpera = null)
		{
			tableFilePath = path;
			fileOperational = fileOpera ?? new FileOpera();
		}
		public TableSheetData Create<T>(string name) where T : ITableCellData,new()
		{
			TableSheetData table = new TableSheetData(name);
			var data = TableContentCollection.LoadBytes(tableFilePath,name+".bytes");
			ByteArray bArr = new ByteArray(fileOperational != null ? 
					fileOperational.ProcessDecrypt(data) : data);
			do {
				IBinRead t = new T() as IBinRead;
				t.Read(bArr);
				table.AddData(t);
			} while (!bArr.IsReadDone);
			return table;
		}
	}
}