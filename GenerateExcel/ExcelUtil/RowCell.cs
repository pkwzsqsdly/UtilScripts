

using System.Collections.Generic;
using System.Text;

public class RowCell {
	public List<CellData> Cells {get { return cells; } }
	protected List<CellData> cells;
	public RowCell()
	{
		cells = new List<CellData>();
	}

	public void AddCell(CellData kv)
	{
		cells.Add(kv);
	}

	public override string ToString()
	{
		StringBuilder buffer = new StringBuilder();
		
		cells.ForEach(x => {
			buffer.Append(x.ToString());
			buffer.Append("|");
		});

		return buffer.ToString();
	}
}