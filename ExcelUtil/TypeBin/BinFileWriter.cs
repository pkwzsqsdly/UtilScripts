using System.Collections.Generic;
using System.IO;
[MapTo("bin")]
public class BinFileWriter :IExcelToFile
{
	public ITamplate Tamplate {get;private set;}

	public void SetInfo(ITamplate tamp)
	{
		Tamplate = tamp;
	}

	public void ToFile(List<CustomExcelTable> tableList)
	{
		for (int i = 0; i < tableList.Count; i++)
		{
			writeToFile(tableList[i]);
		}
		Tamplate?.ToFile(tableList);
	}
	
	private void writeToFile(CustomExcelTable table)
	{
		var fullName = Path.Combine(UserSetting.ConfigOutputPath,$"{table.Name}.bytes");
		FileStream fileStream = new FileStream(fullName, FileMode.Create, FileAccess.Write);
        //创建二进制写入流的实例
        BinaryWriter binaryWriter = new BinaryWriter(fileStream);

		for (int i = 1; i <= table.Row; i++)
		{
			RowData row = table.GetRow(i);
			
			if(!row.IsExport)
				continue;
			
			for (int j = 1; j <= table.Col; j++)
			{
				CellData data = table.GetCell(i,j);
				
				if(data.Title.IsIgnore())
					continue;
				
				UserSetting.Log($"Write Cell Row {i} Col {j},Title Name is {data.Title.Name}");
				
				ExcelUtils.MapTo(this,data.Title.ValueType,binaryWriter,data);
			}
		}
		
		binaryWriter.Flush();
        //关闭二进制流
        binaryWriter.Close();
        //关闭文件流
        fileStream.Close();
	}
	
	[MapTo("int")]
	private void ToInt(BinaryWriter bw,CellData data)
	{
		int res = 0;
		int.TryParse(data.InnerValue,out res);
		bw.Write(res);

	}
	[MapTo("float")]
	private void ToFloat(BinaryWriter bw,CellData data)
	{
		float res = 0;
		float.TryParse(data.InnerValue,out res);
		bw.Write(res);
	}
	[MapTo("string")]
	private void ToString(BinaryWriter bw,CellData data)
	{
		bw.Write(data.InnerValue);
	}
	[MapTo("iArray")]
	private void ToArrayInt(BinaryWriter bw,CellData data)
	{
		string [] splitStrs = data.InnerValue.Split(',');
		bw.Write(splitStrs.Length);
		for (int i = 0; i < splitStrs.Length; i++)
		{
			int res = int.Parse(splitStrs[i],0);
			bw.Write(res);
		}
	}
	[MapTo("fArray")]
	private void ToArrayFloat(BinaryWriter bw,CellData data)
	{
		string [] splitStrs = data.InnerValue.Split(',');
		bw.Write(splitStrs.Length);
		for (int i = 0; i < splitStrs.Length; i++)
		{
			float res = float.Parse(splitStrs[i],0);
			bw.Write(res);
		}
	}
	[MapTo("sArray")]
	private void ToArrayString(BinaryWriter bw,CellData data)
	{
		string [] splitStrs = data.InnerValue.Split(',');
		bw.Write(splitStrs.Length);
		for (int i = 0; i < splitStrs.Length; i++)
		{
			bw.Write(splitStrs[i]);
		}
	}

}