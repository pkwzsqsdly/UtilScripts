using LitJson;
using LG.TableUtil.Config;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil.Json
{
	public class JsonTableSheetData : ITableSheetCreator
	{
		public string tableFilePath {get;private set;}
		public IFileOperational fileOperational{get;private set;}

		public JsonTableSheetData(string path,IFileOperational fileOpera = null)
		{
			tableFilePath = path;
			fileOperational = fileOpera ?? new FileOpera();
		}
		public TableSheetData Create<T>(string name) where T : ITableCellData,new()
		{
			TableSheetData table = new TableSheetData(name);
			var data = TableContentCollection.LoadBytes(tableFilePath,name + ".json");
			string str = System.Text.Encoding.Default.GetString(fileOperational != null ?
					fileOperational.ProcessDecrypt(data) : data);
			// if (cryptography != null)
			// {
			// 	string res = System.Text.Encoding.Unicode.GetString(data);
			// 	byte[] newData = cryptography.Decrypt(System.Convert.FromBase64String(res));
			// 	str = System.Text.Encoding.Default.GetString(newData);
			// }
			// else{
			// 	str = System.Text.Encoding.Default.GetString(data);
			// }
			JsonData json = JsonMapper.ToObject(str);
			for (int i = 0; i < json.Count; i++)
			{
				JsonData jd = json[i];
				T t = JsonMapper.ToObject<T>(jd.ToJson());
				table.AddData(t);
			}
			return table;
		}
	}
}