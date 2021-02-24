
using System.IO;
using OfficeOpenXml;
using System.Collections.Generic;

public class ExcelLoader
{
	protected List<CustomExcelTable> tableList;
	protected ITamplate tamplate;
	protected IExcelToFile excelToFile;
	public ExcelLoader(IExcelToFile toFile, ITamplate tamp = null)
	{
		tableList = new List<CustomExcelTable>();
		excelToFile = toFile;
		tamplate = tamp;
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
						// System.Console.WriteLine(title.Name + "," + title.KeyWords+","+ title.ValueType);
						isTableIgnore = true;
						break;
					}

					customTable.AddTitle(title);
				}

				if (isTableIgnore)
					continue;

				for (int j = 1; j <= maxRowNum; j++)
				{
					RowData row = new RowData();
					bool isIgnore = false;
					for (int k = 1; k <= maxColumnNum; k++)
					{
						var title = customTable.GetTitle(k);

						// if (title.IsIgnore())
						// 	continue;

						var val = sheet.Cells[j + titleNum, k].Text;
						if (k == 1)
						{
							isIgnore = string.IsNullOrEmpty(val);
						}

						// customTable.AddCell(j, new CellData(title, val),isIgnore);
						row.AddCell(new CellData(title, val));
					}

					row.IsExport = !isIgnore;
					System.Console.WriteLine(j + "," + row.IsExport);

					customTable.AddRow(row);
				}

				tableList.Add(customTable);
			}
		}
		return this;
	}

	public void GenerateFile()
	{
		excelToFile?.ToFile(tableList);
		tamplate?.ToFile(tableList);
	}
}