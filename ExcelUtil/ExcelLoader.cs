
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;

public class ExcelLoader
{
	protected List<CustomExcelTable> tableList;
	protected IExcelToFile excelToFile;
	public ExcelLoader(IExcelToFile toFile)
	{
		tableList = new List<CustomExcelTable>();
		excelToFile = toFile;
	}

	public ExcelLoader Load(string filePath)
	{
		FileInfo info = new FileInfo(filePath);
		using (ExcelPackage excelPkg = new ExcelPackage(info))
		{
			int titleNum = 4;
			for (int i = 1; i <= excelPkg.Workbook.Worksheets.Count; i++)
			{
				var sheet = excelPkg.Workbook.Worksheets[i];
				int maxColumnNum = sheet.Dimension.End.Column;//最大列
				int maxRowNum = sheet.Dimension.End.Row;//最小行

				CustomExcelTable customTable = new CustomExcelTable(sheet.Name);

				bool isTableIgnore = false;

				for (int k = 1; k <= maxColumnNum; k++)
				{
					var title = new CellTitle(sheet.Cells[1, k].Text,
							sheet.Cells[2, k].Text,
							sheet.Cells[3, k].Text,
							sheet.Cells[4, k].Text.Equals("yes"));

					if (title.IsNull() && k == 1)
					{
						isTableIgnore = true;
						break;
					}

					customTable.AddTitle(title);
				}

				if (isTableIgnore)
				{
					UserSetting.Log($"Table Sheet Named {sheet.Name} Skiped!");
					continue;
				}

				UserSetting.Log($"Start Read Table Sheet Name is {sheet.Name}");

				for (int j = 1; j <= maxRowNum; j++)
				{
					RowData row = new RowData();
					bool isIgnore = false;
					for (int k = 1; k <= maxColumnNum; k++)
					{
						var title = customTable.GetTitle(k);

						var val = sheet.Cells[j + titleNum, k].Text;
						if (k == 1)
						{
							isIgnore = string.IsNullOrEmpty(val);
						}

						UserSetting.Log($"Read Cell({j},{k}) {title.Name}.");
						row.AddCell(new CellData(title, val));
					}

					row.IsExport = !isIgnore;

					customTable.AddRow(row);
				}

				tableList.Add(customTable);
			}
		}
		return this;
	}
	
	public CustomExcelTable GetTable(string tableName)
	{
		return tableList.Find(x => x.Name == tableName);
	}

	public void GenerateFile()
	{
		excelToFile?.ToFile(tableList);
	}

}