using System.Collections.Generic;

namespace LG.TableUtil
{
	public class RowData 
	{
		public bool IsExport{get;set;}
		public int Count => rowCells.Count;
		private List<CellData> rowCells;

		public RowData()
		{
			rowCells = new List<CellData>();
			IsExport = true;
		}

		public CellData GetCell(int col)
		{
			if (col < rowCells.Count)
			{
				return rowCells[col];
			}
			return null;
		}

		public void AddCell(CellData data)
		{
			rowCells.Add(data);
		}
	}
}