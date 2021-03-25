
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil
{
	public class ExcelLoader
	{
		protected List<CustomExcelTable> tableList;
		protected List<ITableToFile> exportorList;
		public ExcelLoader()
		{
			tableList = new List<CustomExcelTable>();
			exportorList = new List<ITableToFile>();
		}

		public ExcelLoader AddExportor(ITableToFile tableToFile)
		{
			exportorList.Add(tableToFile);
			return this;
		}

		public ExcelLoader Load(string filePath)
		{
			FileInfo info = new FileInfo(filePath);
			FileStream stream = new FileStream(filePath,FileMode.Open);
			var excel = ExcelReaderFactory.CreateOpenXmlReader(stream);
			var workBook = excel.AsDataSet();
			
			int titleNum = 4;
			for (int i = 0; i < workBook.Tables.Count; i++)
			{
				var sheet = workBook.Tables[i];
				var maxRowNum = sheet.Rows.Count;//Sheet中的行数
				var maxColumnNum = sheet.Columns.Count;//ExcelFile的表Sheet的列数
				CustomExcelTable customTable = new CustomExcelTable(sheet.TableName);
				bool isTableIgnore = false;

				for (int k = 0; k < maxColumnNum; k++)
				{
					var name 	= sheet.Rows[0][k].ToString();
					var keyword = sheet.Rows[1][k].ToString();
					var vtype 	= sheet.Rows[2][k].ToString();
					var export 	= sheet.Rows[3][k].ToString();
					var title 	= new CellTitle(name,keyword,vtype,export.Equals("yes"));

					if (title.IsNull() && k == 0)
					{
						isTableIgnore = true;
						break;
					}

					customTable.AddTitle(title);
				}

				if (isTableIgnore)
				{
					UserSetting.Log($"Table Sheet Named {sheet.TableName} Skiped!");
					continue;
				}

				UserSetting.Log($"Start Read Table Sheet Name is {sheet.TableName}");

				for (int j = titleNum; j < maxRowNum; j++)
				{
					RowData row = new RowData();
					bool isIgnore = false;
					for (int k = 0; k < maxColumnNum; k++)
					{
						var title = customTable.GetTitle(k);

						var val = sheet.Rows[j][k].ToString();
						if (k == 0)
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
			
			// using (ExcelPackage excelPkg = new ExcelPackage(info))
			// {
			// 	int titleNum = 4;
			// 	for (int i = 1; i <= excelPkg.Workbook.Worksheets.Count; i++)
			// 	{
			// 		var sheet = excelPkg.Workbook.Worksheets[i];
			// 		int maxColumnNum = sheet.Dimension.End.Column;//最大列
			// 		int maxRowNum = sheet.Dimension.End.Row;//最小行

			// 		CustomExcelTable customTable = new CustomExcelTable(sheet.Name);

			// 		bool isTableIgnore = false;

			// 		for (int k = 1; k <= maxColumnNum; k++)
			// 		{
			// 			var title = new CellTitle(sheet.Cells[1, k].Text,
			// 					sheet.Cells[2, k].Text,
			// 					sheet.Cells[3, k].Text,
			// 					sheet.Cells[4, k].Text.Equals("yes"));

			// 			if (title.IsNull() && k == 1)
			// 			{
			// 				isTableIgnore = true;
			// 				break;
			// 			}

			// 			customTable.AddTitle(title);
			// 		}

			// 		if (isTableIgnore)
			// 		{
			// 			UserSetting.Log($"Table Sheet Named {sheet.Name} Skiped!");
			// 			continue;
			// 		}

			// 		UserSetting.Log($"Start Read Table Sheet Name is {sheet.Name}");

			// 		for (int j = 1; j <= maxRowNum; j++)
			// 		{
			// 			RowData row = new RowData();
			// 			bool isIgnore = false;
			// 			for (int k = 1; k <= maxColumnNum; k++)
			// 			{
			// 				var title = customTable.GetTitle(k);

			// 				var val = sheet.Cells[j + titleNum, k].Text;
			// 				if (k == 1)
			// 				{
			// 					isIgnore = string.IsNullOrEmpty(val);
			// 				}

			// 				UserSetting.Log($"Read Cell({j},{k}) {title.Name}.");
			// 				row.AddCell(new CellData(title, val));
			// 			}

			// 			row.IsExport = !isIgnore;

			// 			customTable.AddRow(row);
			// 		}

			// 		tableList.Add(customTable);
			// 	}
			// }
			return this;
		}
		
		public CustomExcelTable GetTable(string tableName)
		{
			return tableList.Find(x => x.Name == tableName);
		}

		public void GenerateFile()
		{
			exportorList.ForEach(exportor => exportor.ToFile(tableList));
		}

	}
}