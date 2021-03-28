using System.Collections.Generic;
using System.IO;


namespace LG.TableUtil
{

	public class ExcelExportor
	{
		public ExcelExportor()
		{

		}


		public void Export(List<CustomTable> tables)
		{
			
			// FileInfo info = new FileInfo("d:/text.xls");
			// using(ExcelPackage excelPkg = new ExcelPackage())
			// {
			// 	tables.ForEach(table => {
			// 		ExcelWorksheet sheet = excelPkg.Workbook.Worksheets.Add(table.Name);
			// 		for (int j = 1; j < table.Col; j++)
			// 		{
			// 			sheet.SetValue(1,j,table.GetTitle(j).Name);
			// 			sheet.SetValue(2,j,table.GetTitle(j).KeyWords);
			// 			sheet.SetValue(3,j,table.GetTitle(j).ValueType);
			// 			sheet.SetValue(4,j,table.GetTitle(j).IsExport);
			// 		}
			// 		for (int i = 1; i <= table.Row; i++)
			// 		{
			// 			for (int j = 1; j <= table.Col; j++)
			// 			{
			// 				sheet.SetValue(i + 4,j,table.GetCell(i,j).InnerValue);
			// 			}
			// 		}
			// 	});

			// 	excelPkg.SaveAs(new FileInfo("aaaa.xlsx"));
			// }
		}

	}
}