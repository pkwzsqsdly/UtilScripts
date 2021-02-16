using System.Collections.Generic;
using System.Text;
using System.IO;

public class SharpTamplate : ITamplate
{
	public void ToFile(List<CustomExcelTable> tables)
	{
		for (int i = 0; i < tables.Count; i++)
		{
			var table = tables[i];
			var row = table.RowCells[0];

			StreamReader sr = new StreamReader("res/CSharp.tamplate",Encoding.UTF8);
			string str = sr.ReadToEnd();
			sr.Close();
			
			str = str.Replace("{className}",table.Name);
			string strVar = getVariable(row.Cells);
			str = str.Replace("{variable}",strVar);
			string strRead = getReadVar(row.Cells);
			str = str.Replace("{readFile}",strRead);

			write(table.Name,str);
		}
	}

	protected void write(string fileName,string context)
	{
		var fullName = LocalConfig.Inst.GetFullName("csharp",fileName);
		FileStream fs = new FileStream(fullName, FileMode.Create);
		byte[] data = System.Text.Encoding.Default.GetBytes(context);
        //开始写入
        fs.Write(data, 0, data.Length);
        //清空缓冲区，关闭流
        fs.Flush();
        fs.Close();
	}

	protected string getVariable(List<CellData> cells)
	{
		StringBuilder str = new StringBuilder();
		for (int i = 0; i < cells.Count; i++)
		{
			var cell = cells[i];
			var desc = LocalConfig.Inst.GetDesc("csharp");
			str.Append(string.Format(desc,cell.Description));
			str.Append("\t");
			var sval = LocalConfig.Inst.GetTamplateContrastType("csharp",cell.ValueType);
			str.Append(string.Format(sval,cell.KeyWords));
			str.Append("\n");
		}
		return str.ToString();
	}

	protected string getReadVar(List<CellData> cells)
	{
		StringBuilder str = new StringBuilder();
		for (int i = 0; i < cells.Count; i++)
		{
			var cell = cells[i];
			str.Append("\t\t");
			var sval = LocalConfig.Inst.GetTamplateTypeReadVar("csharp",cell.ValueType);
			str.Append(string.Format(sval,cell.KeyWords));
			str.Append("\n");
		}
		return str.ToString();
	}
}