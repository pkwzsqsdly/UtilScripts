using System.Collections.Generic;
using System.IO;
using LitJson;
[MapTo("json")]
public class JsonFileWriter : IExcelToFile
{
	public ITamplate Tamplate {get;private set;}

	public void SetInfo(ITamplate tamp = null)
	{
		Tamplate = tamp;
	}
	public void ToFile(List<CustomExcelTable> tableList)
	{
		for (int i = 0; i < tableList.Count; i++)
		{
			var str = tableToFile(tableList[i]);
			writeToFile(tableList[i].Name,str);
		}
		Tamplate?.ToFile(tableList);
	}
	private string tableToFile(CustomExcelTable table)
	{
		JsonData json = new JsonData();
		for (int i = 1; i <= table.Row; i++)
		{
			JsonData row = new JsonData();
			RowData rowData = table.GetRow(i);

			if (!rowData.IsExport)
				continue;

			for (int j = 1; j <= table.Col; j++)
			{
				CellData data = rowData.GetCell(j);
				
				if(data.Title.IsIgnore())
					continue;
				
				UserSetting.Log($"Write Cell Row {i} Col {j},Title Name is {data.Title.Name}");
				
				ExcelUtils.MapTo(this,data.Title.ValueType,row,data);
			}

			json.Add(row);
		}
		return json.ToJson();
	}

	private void writeToFile(string name,string context)
	{
		var fullName = Path.Combine(UserSetting.ConfigOutputPath,$"{name}.json");
		FileStream fs = new FileStream(fullName, FileMode.Create);
		byte[] data = System.Text.Encoding.Default.GetBytes(context);
		//开始写入
		fs.Write(data, 0, data.Length);
		//清空缓冲区，关闭流
		fs.Flush();
		fs.Close();
	}
	
	[MapTo("int")]
	private void ToInt(JsonData json,CellData data)
	{
		int res = 0;
		if (int.TryParse(data.InnerValue,out res))
		{
			json[data.Title.KeyWords] = res;
		}
	}
	[MapTo("float")]
	private void ToFloat(JsonData json,CellData data)
	{
		float res = 0;
		if (float.TryParse(data.InnerValue,out res))
		{
			json[data.Title.KeyWords] = res;
		}
	}
	[MapTo("string")]
	private void ToString(JsonData json,CellData data)
	{
		json[data.Title.KeyWords] = data.InnerValue;
	}
	[MapTo("iArray")]
	private void ToArrayInt(JsonData json,CellData data)
	{
		string [] splitStrs = data.InnerValue.Split(',');
		JsonData jsonList = new JsonData();
		for (int i = 0; i < splitStrs.Length; i++)
		{
			int res = 0;
			if (int.TryParse(splitStrs[i],out res))
			{
				jsonList.Add(res);
			}
		}
		json[data.Title.KeyWords] = jsonList;
	}
	[MapTo("fArray")]
	private void ToArrayFloat(JsonData json,CellData data)
	{
		string [] splitStrs = data.InnerValue.Split(',');
		JsonData jsonList = new JsonData();
		for (int i = 0; i < splitStrs.Length; i++)
		{
			float res = 0;
			if (float.TryParse(splitStrs[i],out res))
			{
				jsonList.Add(res);
			}
		}
		json[data.Title.KeyWords] = jsonList;
	}
	[MapTo("sArray")]
	private void ToArrayString(JsonData json,CellData data)
	{
		string [] splitStrs = data.InnerValue.Split(',');
		JsonData jsonList = new JsonData();
		for (int i = 0; i < splitStrs.Length; i++)
		{
			jsonList.Add(splitStrs[i]);
		}
		json[data.Title.KeyWords] = jsonList;
	}
}