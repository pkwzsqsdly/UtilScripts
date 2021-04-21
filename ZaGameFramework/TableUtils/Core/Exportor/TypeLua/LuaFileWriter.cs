using System.Collections.Generic;
using System.IO;
using System.Text;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil.Lua
{
	public interface ILuaWriterParse : IKeywordTypeParse
	{
		void ParseTo(StringBuilder sb,CellData data);
	}
	public class LuaFileWriter : TypeParsing,ITableToFile
	{
		private IFileOperational fileOperational;
		public LuaFileWriter()
		{
			BindType("int"		,new LuaKeywordInt()	);
			BindType("float"	,new LuaKeywordFloat()	);
			BindType("string"	,new LuaKeywordString());
			BindType("iArray"	,new LuaKeywordIArray());
			BindType("fArray"	,new LuaKeywordFArray());
			BindType("sArray"	,new LuaKeywordSArray());
		}
		
		public void ToFile(List<CustomTable> tableList)
		{
			for (int i = 0; i < tableList.Count; i++)
			{
				var str = tableToFile(tableList[i]);
				writeToFile(tableList[i].Name,str);
			}
		}
		private string tableToFile(CustomTable table)
		{
			StringBuilder sbvar = new StringBuilder();
			sbvar.Append($"local {table.Name} = {{");
			for (int i = 0; i < table.Row; i++)
			{
				RowData rowData = table.GetRow(i);

				if (!rowData.IsExport)
					continue;
				
				StringBuilder newsb = new StringBuilder();
				newsb.Append("[@@@@]={");
				int id = 0;
				for (int j = 0; j < table.Col; j++)
				{
					CellData data = rowData.GetCell(j);
					
					if(data.Title.IsIgnore())
						continue;
					if(j > 0)
					{
						UserSetting.Log($"{j}===={table.Col - 1}");
						newsb.Append(",");
					}

					if (data.Title.KeyWords == "Id")
					{
						int.TryParse(data.InnerValue,out id);
					}
										
					UserSetting.Log($"Write Cell Row {i} Col {j},Value is {data.InnerValue}");
					
					SeekParse<ILuaWriterParse>(data.Title.ValueType)?.ParseTo(newsb,data);

					
				}
				newsb.Append("}");
				
				string rowStr = newsb.ToString().Replace("@@@@",id.ToString());
				sbvar.Append(rowStr);

				if(i < table.Row - 1)
					sbvar.Append(",\n");
			}
			sbvar.Append($"}}\nreturn {table.Name}");
			return sbvar.ToString();
		}

		private void writeToFile(string name,string context)
		{
			var fullName = Path.Combine(UserSetting.ConfigOutputPath,$"{name}.lua");
			FileStream fs = new FileStream(fullName, FileMode.Create);
			byte[] data = System.Text.Encoding.Default.GetBytes(context);
			//开始写入
			fs.Write(data, 0, data.Length);
			//清空缓冲区，关闭流
			fs.Flush();
			fs.Close();
		}

		public void UseFileWriter(IFileOperational fileOpera)
		{
			fileOperational = fileOpera ?? new FileOpera();
		}

		internal class LuaKeywordInt : ILuaWriterParse
		{
			public void ParseTo(StringBuilder sb,CellData data)
			{
				int res = 0;
				if (int.TryParse(data.InnerValue,out res))
				{
					sb.Append($"{data.Title.KeyWords} = {res}");
				}
			}
		}

		internal class LuaKeywordFloat : ILuaWriterParse
		{
			public void ParseTo(StringBuilder sb,CellData data)
			{
				float res = 0;
				if (float.TryParse(data.InnerValue,out res))
				{
					sb.Append($"{data.Title.KeyWords} = {res}");
				}
			}
		}
		
		internal class LuaKeywordString : ILuaWriterParse
		{
			public void ParseTo(StringBuilder sb,CellData data)
			{
				sb.Append($"{data.Title.KeyWords} = \"{data.InnerValue}\"");
			}
		}
		
		internal class LuaKeywordIArray : ILuaWriterParse
		{
			public void ParseTo(StringBuilder sb,CellData data)
			{
				string [] splitStrs = data.InnerValue.Split(',');
				
				sb.Append($"{data.Title.KeyWords} = {{");
				for (int i = 0; i < splitStrs.Length; i++)
				{
					int res = int.Parse(splitStrs[i],0);
					sb.Append(res);
					
					if(i < splitStrs.Length - 1)
						sb.Append(",");
				}
				sb.Append("}}");
			}
		}

		internal class LuaKeywordFArray : ILuaWriterParse
		{
			public void ParseTo(StringBuilder sb,CellData data)
			{
				string [] splitStrs = data.InnerValue.Split(',');

				sb.Append($"{data.Title.KeyWords} = {{");
				for (int i = 0; i < splitStrs.Length; i++)
				{
					float res = int.Parse(splitStrs[i],0);
					sb.Append(res);
					
					if(i < splitStrs.Length - 1)
						sb.Append(",");
				}
				sb.Append("}}");

			}
		}
		internal class LuaKeywordSArray : ILuaWriterParse
		{
			public void ParseTo(StringBuilder sb,CellData data)
			{
				string [] splitStrs = data.InnerValue.Split(',');
				
				sb.Append($"{data.Title.KeyWords} = {{");
				for (int i = 0; i < splitStrs.Length; i++)
				{
					sb.Append($"\"{splitStrs[i]}\"");
					
					if(i < splitStrs.Length - 1)
						sb.Append(",");
				}
				sb.Append("}}");
			}
		}
	}
}