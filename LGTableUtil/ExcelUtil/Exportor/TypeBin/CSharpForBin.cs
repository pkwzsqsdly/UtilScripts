using System.Collections.Generic;
using System.Text;
using System.IO;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil.Bin
{
	public interface IBinCSharpParse : IKeywordTypeParse
	{
		void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title);
	}
	public class CSharpForBin : TypeParsing,ITableToFile
	{
		private IFileOperational fileOperational;
		public CSharpForBin()
		{
			BindType("int"		,new BinKeywordInt()	);
			BindType("float"	,new BinKeywordFloat()	);
			BindType("string"	,new BinKeywordString()	);
			BindType("iArray"	,new BinKeywordIArray()	);
			BindType("fArray"	,new BinKeywordFArray()	);
			BindType("sArray"	,new BinKeywordSArray()	);
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
			fileOperational = fileOpera ?? new FileOpera();
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
			StringBuilder sbctx = new StringBuilder();
			
			for (int i = 0; i < table.Col; i++)
			{
				CellTitle title = table.GetTitle(i);

				if (title.IsIgnore())
					continue;

				SeekParse<IBinCSharpParse>(title.ValueType)?.ParseTo(sbvar,sbctx,title);
			}
			
			StreamReader sr = new StreamReader("res/CSharpBin.tamplate",Encoding.UTF8);
			string tamp = sr.ReadToEnd();
			sr.Close();

			tamp = tamp.Replace("{className}",table.Name);
			tamp = tamp.Replace("{variable}",sbvar.ToString());
			tamp = tamp.Replace("{assignment}",sbctx.ToString());
			
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
		
		internal class BinKeywordInt : IBinCSharpParse{
			public void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic int {title.KeyWords} {{get; private set;}}\n");
				sbctx.Append($"\t\t{title.KeyWords} = bArr.ReadInt();\n");
			}
		}
		internal class BinKeywordFloat : IBinCSharpParse{
			public void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic float {title.KeyWords} {{get; private set;}}\n");
				sbctx.Append($"\t\t{title.KeyWords} = bArr.ReadSingle();\n");
			}
		}
		internal class BinKeywordString : IBinCSharpParse{
			public void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic string {title.KeyWords} {{get; private set;}}\n");
				sbctx.Append($"\t\t{title.KeyWords} = bArr.ReadString();\n");
			}
		}
		internal class BinKeywordIArray : IBinCSharpParse{
			public void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic int[] {title.KeyWords} {{get; private set;}}\n");

				sbctx.Append($"\t\tint n{title.KeyWords} = bArr.ReadInt();\n");
				sbctx.Append($"\t\t{title.KeyWords} = new int[n{title.KeyWords}];\n");
				sbctx.Append($"\t\tfor(int i = 0; i < n{title.KeyWords}; i++)\n");
				sbctx.Append($"\t\t\t{title.KeyWords}[i] = bArr.ReadInt();\n");
			}
		}
		internal class BinKeywordFArray : IBinCSharpParse{
			public void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic float[] {title.KeyWords} {{get; private set;}}\n");

				sbctx.Append($"\t\tint n{title.KeyWords} = bArr.ReadInt();\n");
				sbctx.Append($"\t\t{title.KeyWords} = new float[n{title.KeyWords}];\n");
				sbctx.Append($"\t\tfor(int i = 0; i < n{title.KeyWords}; i++)\n");
				sbctx.Append($"\t\t\t{title.KeyWords}[i] = bArr.ReadSingle();\n");
			}
		}
		internal class BinKeywordSArray : IBinCSharpParse
		{
			public void ParseTo(StringBuilder sbvar,StringBuilder sbctx,CellTitle title)
			{
				sbvar.Append($"\t//{title.Name}\n");
				sbvar.Append($"\tpublic string[] {title.KeyWords} {{get; private set;}}\n");

				sbctx.Append($"\t\tint n{title.KeyWords} = bArr.ReadInt();\n");
				sbctx.Append($"\t\t{title.KeyWords} = new string[n{title.KeyWords}];\n");
				sbctx.Append($"\t\tfor(int i = 0; i < n{title.KeyWords}; i++)\n");
				sbctx.Append($"\t\t\t{title.KeyWords}[i] = bArr.ReadString();\n");
			}
		}





	}
}