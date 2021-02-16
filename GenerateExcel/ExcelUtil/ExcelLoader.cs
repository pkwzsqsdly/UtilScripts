
using System;
using System.IO;
using OfficeOpenXml;
using System.Text;
using System.Collections.Generic;

public class ExcelLoader
{
    protected List<CustomExcelTable> tableList;
    protected ITamplate tamplate;
    protected FileWriter fileWriter;
    public ExcelLoader(IWrite2File writer)
    {
        tableList = new List<CustomExcelTable>();
        fileWriter = new FileWriter(writer);
        tamplate = new SharpTamplate();
    }

    public ExcelLoader Load(string filePath)
    {
        FileInfo info = new FileInfo(filePath);
        using(ExcelPackage excelPkg = new ExcelPackage(info))
        {
            for (int i = 0; i < excelPkg.Workbook.Worksheets.Count; i++)
            {
                var sheet = excelPkg.Workbook.Worksheets[i+1];
                int maxColumnNum    = sheet.Dimension.End.Column;//最大列
                int maxRowNum       = sheet.Dimension.End.Row;//最小行
                CustomExcelTable customTable = new CustomExcelTable(sheet.Name);
                tableList.Add(customTable);
                
                for (int j = 6; j <= maxRowNum; j++)
                {
                    for (int k = 1; k <= maxColumnNum; k++)
                    {
                        var val = sheet.Cells[j,k].Text;
                        customTable.AddCell(j - 6,new CellData(
                            sheet.Cells[1,k].Text,
                            sheet.Cells[2,k].Text,
                            sheet.Cells[3,k].Text,
                            sheet.Cells[4,k].Text.Equals("yes"),
                            sheet.Cells[5,k].Text.Equals("yes"),
                            val
                        ));
                    }
                }
            }
        }
        return this;
    }

    public void WriteToFile()
    {
        fileWriter.Write(tableList);
        if(tamplate != null)
        {
            tamplate.ToFile(tableList);
        }
    }
}