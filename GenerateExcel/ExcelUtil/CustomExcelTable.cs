using System.Collections.Generic;
using System.IO;
using System.Text;

public class CustomExcelTable 
{
	public string Name {get{return sheetName;}}
	public List<RowCell> RowCells{get{return rowCells;}}
	protected List<RowCell> rowCells;
	protected string sheetName;
	public CustomExcelTable(string name)
	{
		rowCells = new List<RowCell>();
		sheetName = name;
	}

	public void AddCell(int row,CellData cell)
	{
		if(row >= rowCells.Count)
		{
			rowCells.Add(new RowCell());
		}
		var currRow = rowCells[row];
		currRow.AddCell(cell);
	}

	public override string ToString()
	{
		StringBuilder buffer = new StringBuilder();
		
		rowCells.ForEach(x => {
			buffer.Append(x.ToString());
			buffer.Append("\n");
		});

		return buffer.ToString();
	}

}