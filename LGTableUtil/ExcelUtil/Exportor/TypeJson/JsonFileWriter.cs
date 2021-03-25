using System.Collections.Generic;
using System.IO;
using LitJson;
using LG.TableUtil.FileOperational;


namespace LG.TableUtil.Json
{
	public interface IJsonWriterParse : IKeywordTypeParse
	{
		void ParseTo(JsonData json, CellData data);
	}
	public class JsonFileWriter : TypeParsing, ITableToFile
	{
		private IFileOperational fileOperational;
		public JsonFileWriter()
		{
			BindType("int", new JsonKeywordInt());
			BindType("float", new JsonKeywordFloat());
			BindType("string", new JsonKeywordString());
			BindType("iArray", new JsonKeywordIArray());
			BindType("fArray", new JsonKeywordFArray());
			BindType("sArray", new JsonKeywordSArray());
		}
		public void UseFileWriter(IFileOperational fileOpera)
		{
			fileOperational = fileOpera;
		}

		public void ToFile(List<CustomExcelTable> tableList)
		{
			for (int i = 0; i < tableList.Count; i++)
			{
				var str = tableToFile(tableList[i]);
				writeToFile(tableList[i].Name, str);
			}
		}
		private string tableToFile(CustomExcelTable table)
		{
			JsonData json = new JsonData();
			for (int i = 0; i < table.Row; i++)
			{
				JsonData row = new JsonData();
				RowData rowData = table.GetRow(i);

				if (!rowData.IsExport)
					continue;

				for (int j = 0; j < table.Col; j++)
				{
					CellData data = rowData.GetCell(j);

					if (data.Title.IsIgnore())
						continue;

					UserSetting.Log($"Write Cell Row {i} Col {j},Value is {data.InnerValue}");

					SeekParse<IJsonWriterParse>(data.Title.ValueType)?.ParseTo(row, data);
				}

				json.Add(row);
			}
			return json.ToJson();
		}

		private void writeToFile(string name, string context)
		{
			var fullName = Path.Combine(UserSetting.ConfigOutputPath, $"{name}.json");
			byte[] data = System.Text.Encoding.Default.GetBytes(context);

			// fileOperational?.WriteToFile(fullName,data);
			// FileOpera.WriteToFile(fullName,fileOperational.ProcessBytes(data));
			FileOpera.WriteToFile(fullName,fileOperational != null ? fileOperational.ProcessEncrypt(data):data);
			// FileStream fs = new FileStream(fullName, FileMode.Create);
			// if (cryptography != null)
			// {
			// 	var cdate = cryptography.Encrypt(data);
			// 	string str = System.Convert.ToBase64String(cdate);
			// 	byte[] newData = System.Text.Encoding.Unicode.GetBytes(str);
			// 	fs.Write(newData, 0, newData.Length);
			// }
			// else
			// {
			// 	fs.Write(data, 0, data.Length);
			// }
			// //清空缓冲区，关闭流
			// fs.Flush();
			// fs.Close();
		}

		internal class JsonKeywordInt : IJsonWriterParse
		{
			public void ParseTo(JsonData json, CellData data)
			{
				int res = 0;
				if (int.TryParse(data.InnerValue, out res))
				{
					json[data.Title.KeyWords] = res;
				}
			}
		}

		internal class JsonKeywordFloat : IJsonWriterParse
		{
			public void ParseTo(JsonData json, CellData data)
			{
				float res = 0;
				if (float.TryParse(data.InnerValue, out res))
				{
					json[data.Title.KeyWords] = res;
				}
			}
		}

		internal class JsonKeywordString : IJsonWriterParse
		{
			public void ParseTo(JsonData json, CellData data)
			{
				json[data.Title.KeyWords] = data.InnerValue;
			}
		}

		internal class JsonKeywordIArray : IJsonWriterParse
		{
			public void ParseTo(JsonData json, CellData data)
			{
				string[] splitStrs = data.InnerValue.Split(',');
				JsonData jsonList = new JsonData();
				for (int i = 0; i < splitStrs.Length; i++)
				{
					int res = 0;
					if (int.TryParse(splitStrs[i], out res))
					{
						jsonList.Add(res);
					}
				}
				json[data.Title.KeyWords] = jsonList;
			}
		}

		internal class JsonKeywordFArray : IJsonWriterParse
		{
			public void ParseTo(JsonData json, CellData data)
			{
				string[] splitStrs = data.InnerValue.Split(',');
				JsonData jsonList = new JsonData();
				for (int i = 0; i < splitStrs.Length; i++)
				{
					float res = 0;
					if (float.TryParse(splitStrs[i], out res))
					{
						jsonList.Add(res);
					}
				}
				json[data.Title.KeyWords] = jsonList;
			}
		}
		internal class JsonKeywordSArray : IJsonWriterParse
		{
			public void ParseTo(JsonData json, CellData data)
			{
				string[] splitStrs = data.InnerValue.Split(',');
				JsonData jsonList = new JsonData();
				for (int i = 0; i < splitStrs.Length; i++)
				{
					jsonList.Add(splitStrs[i]);
				}
				json[data.Title.KeyWords] = jsonList;
			}
		}
	}
}