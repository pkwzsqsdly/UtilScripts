using System.Collections.Generic;
using System.Text;
using System.IO;

[MapTo("json_csharp")]
public class CSharpForJson : ITamplate
{
	public void ToFile(List<CustomExcelTable> tables)
	{
		for (int i = 0; i < tables.Count; i++)
		{
			string str = tableToFile(tables[i]);
			write(tables[i].Name,str);
		}
	}

	protected void write(string fileName,string context)
	{
		var fullName = Path.Combine(UserSetting.ClassOutputPath,fileName + ".cs");
		FileStream fs = new FileStream(fullName, FileMode.Create);
		byte[] data = System.Text.Encoding.Default.GetBytes(context);
        //开始写入
        fs.Write(data, 0, data.Length);
        //清空缓冲区，关闭流
        fs.Flush();
        fs.Close();
	}
	private string tableToFile(CustomExcelTable table)
	{
		StringBuilder sbvar = new StringBuilder();
		
		for (int i = 1; i <= table.Col; i++)
		{
			CellTitle title = table.GetTitle(i);

			if (title.IsIgnore())
				continue;

			ExcelUtils.MapTo(this,title.ValueType,sbvar,title);
		}
		
		StreamReader sr = new StreamReader("res/CSharpJson.tamplate",Encoding.UTF8);
		string tamp = sr.ReadToEnd();
		sr.Close();

		tamp = tamp.Replace("{className}",table.Name);
		tamp = tamp.Replace("{variable}",sbvar.ToString());
		
		return tamp;
	}
	
	[MapTo("int")]
	private void ToInt(StringBuilder sbvar,CellTitle title)
	{
		sbvar.Append($"\t//{title.Name}\n");
		sbvar.Append($"\tpublic int {title.KeyWords} {{get; private set;}}\n");
		// sbctx.Append($"\t\t{title.KeyWords} = bArr.ReadInt();\n");
	}
	[MapTo("float")]
	private void ToFloat(StringBuilder sbvar,CellTitle title)
	{
		sbvar.Append($"\t//{title.Name}\n");
		sbvar.Append($"\tpublic float {title.KeyWords} {{get; private set;}}\n");
		// sbctx.Append($"\t\t{title.KeyWords} = bArr.ReadSingle();\n");
	}
	[MapTo("string")]
	private void ToString(StringBuilder sbvar,CellTitle title)
	{
		sbvar.Append($"\t//{title.Name}\n");
		sbvar.Append($"\tpublic string {title.KeyWords} {{get; private set;}}\n");
		// sbctx.Append($"\t\t{title.KeyWords} = bArr.ReadString();\n");
	}
	[MapTo("iArray")]
	private void ToArrayInt(StringBuilder sbvar,CellTitle title)
	{
		sbvar.Append($"\t//{title.Name}\n");
		sbvar.Append($"\tpublic int[] {title.KeyWords} {{get; private set;}}\n");

		// sbctx.Append($"\t\tint n{title.KeyWords} = bArr.ReadInt();\n");
		// sbctx.Append($"\t\t{title.KeyWords} = new int[n{title.KeyWords}];\n");
		// sbctx.Append($"\t\tfor(int i = 0; i < n{title.KeyWords}; i++)\n");
		// sbctx.Append($"\t\t\t{title.KeyWords}[i] = bArr.ReadInt();\n");
	}
	[MapTo("fArray")]
	private void ToArrayFloat(StringBuilder sbvar,CellTitle title)
	{
		sbvar.Append($"\t//{title.Name}\n");
		sbvar.Append($"\tpublic float[] {title.KeyWords} {{get; private set;}}\n");

		// sbctx.Append($"\t\tint n{title.KeyWords} = bArr.ReadInt();\n");
		// sbctx.Append($"\t\t{title.KeyWords} = new float[n{title.KeyWords}];\n");
		// sbctx.Append($"\t\tfor(int i = 0; i < n{title.KeyWords}; i++)\n");
		// sbctx.Append($"\t\t\t{title.KeyWords}[i] = bArr.ReadSingle();\n");
	}
	[MapTo("sArray")]
	private void ToArrayString(StringBuilder sbvar,CellTitle title)
	{
		sbvar.Append($"\t//{title.Name}\n");
		sbvar.Append($"\tpublic string[] {title.KeyWords} {{get; private set;}}\n");

		// sbctx.Append($"\t\tint n{title.KeyWords} = bArr.ReadInt();\n");
		// sbctx.Append($"\t\t{title.KeyWords} = new string[n{title.KeyWords}];\n");
		// sbctx.Append($"\t\tfor(int i = 0; i < n{title.KeyWords}; i++)\n");
		// sbctx.Append($"\t\t\t{title.KeyWords}[i] = bArr.ReadString();\n");
	}

}