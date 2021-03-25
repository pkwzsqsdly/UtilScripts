using System.Collections.Generic;
using System.Text;
using System.IO;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil.Json
{
	public interface IJsonCSharpParse : IKeywordTypeParse
	{
		void ParseTo(StringBuilder sbvar,CellTitle title);
	}
	public class CSharpForJson : TypeParsing,ITableToFile
	{
		private IFileOperational fileOperational;
		public CSharpForJson()
		{
			BindType("int"		,new JsonKeywordInt()	);
			BindType("float"	,new JsonKeywordFloat()	);
			BindType("string"	,new JsonKeywordString());
			BindType("iArray"	,new JsonKeywordIArray());
			BindType("fArray"	,new JsonKeywordFArray());
			BindType("sArray"	,new JsonKeywordSArray());
		}
		public void ToFile(List<CustomExcelTable> tables)
		{
			for (int i = 0; i < tables.Count; i++)
			{
				string str = tableToFile(tables[i]);
				write(tables[i].Name,str);
			}
			WriteTypeClassDefine(tables);
		}

		public void UseFileWriter(IFileOperational fileOpera)
		{
			fileOperational = fileOpera;
		}

		protected void write(string fileName,string context)
		{
			var fullName = Path.Combine(UserSetting.ClassOutputPath,fileName + ".cs");
			byte[] data = System.Text.Encoding.Default.GetBytes(context);

			// fileOperational?.WriteToFile(fullName,data);
			FileOpera.WriteToFile(fullName,data);
			
			// FileStream fs = new FileStream(fullName, FileMode.Create);
			// //开始写入
			// fs.Write(data, 0, data.Length);
			// //清空缓冲区，关闭流
			// fs.Flush();
			// fs.Close();
		}
		private string tableToFile(CustomExcelTable table)
		{
			StringBuilder sbvar = new StringBuilder();
			
			for (int i = 0; i < table.Col; i++)
			{
				CellTitle title = table.GetTitle(i);

				if (title.IsIgnore())
					continue;

				SeekParse<IJsonCSharpParse>(title.ValueType)?.ParseTo(sbvar,title);
			}
			
			StreamReader sr = new StreamReader("res/CSharpJson.tamplate",Encoding.UTF8);
			string tamp = sr.ReadToEnd();
			sr.Close();

			tamp = tamp.Replace("{className}",table.Name);
			tamp = tamp.Replace("{variable}",sbvar.ToString());
			
			return tamp;
		}
		
		private void WriteTypeClassDefine(List<CustomExcelTable> list)
		{
			StreamReader sr = new StreamReader("res/CSharpTypeDefine.tamplate",Encoding.UTF8);
			string str = sr.ReadToEnd();
			sr.Close();

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				string name = list[i].Name;
				sb.Append($"\t\tloaderTableList.Add(lct => lct.LoadTable<{name}>(\"{name}\"));\n");
			}
			str = str.Replace("{variable}",sb.ToString());
			write("TableLoaderDefine",str);
		}
		internal class JsonKeywordInt : IJsonCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic int {title.KeyWords} {{get; private set;}}\n");
			}
		}

		internal class JsonKeywordFloat : IJsonCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic float {title.KeyWords} {{get; private set;}}\n");
			}
		}
		
		internal class JsonKeywordString : IJsonCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic string {title.KeyWords} {{get; private set;}}\n");
			}
		}
		
		internal class JsonKeywordIArray : IJsonCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic int[] {title.KeyWords} {{get; private set;}}\n");
			}
		}

		internal class JsonKeywordFArray : IJsonCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic float[] {title.KeyWords} {{get; private set;}}\n");
			}
		}
		internal class JsonKeywordSArray : IJsonCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic string[] {title.KeyWords} {{get; private set;}}\n");
			}
		}
	}
}