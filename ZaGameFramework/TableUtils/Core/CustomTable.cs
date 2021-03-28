using System.Collections.Generic;
using System.Text;

namespace LG.TableUtil
{
	public class CustomTable
	{
		public string Name { get { return sheetName; } }
		public int Row{ get { return cellDatas.Count; } }
		public int Col{ get; private set; }
		protected List<RowData> cellDatas;
		protected List<CellTitle> cellTitles;
		protected string sheetName;
		public CustomTable(string name)
		{
			cellDatas = new List<RowData>();
			cellTitles = new List<CellTitle>();
			sheetName = name;
		}

		public void AddCell(int row, CellData cell,bool ignore = false)
		{
			int selfRow = row - 1;
			
			var currRow = cellDatas[selfRow];
			currRow.AddCell(cell);
		}

		public void AddRow(RowData data)
		{
			cellDatas.Add(data);
			if (Col < data.Count)
				Col = data.Count;
		}

		public RowData GetRow(int row)
		{
			if(row > cellDatas.Count)
				return null;

			return cellDatas[row];
		}

		public CellData GetCell(int row,int col)
		{
			var rowData = GetRow(row);
			return rowData?.GetCell(col);
		}
		public void AddTitle(CellTitle title)
		{
			cellTitles.Add(title);
		}

		public CellTitle GetTitle(int col)
		{
			if (col <= cellTitles.Count)
			{
				return cellTitles[col];
			}
			return null;
		}

		public override string ToString()
		{
			StringBuilder buffer = new StringBuilder();

			cellDatas.ForEach(x =>
			{
				buffer.Append(x.ToString());
				buffer.Append("\n");
			});

			return buffer.ToString();
		}

	}
}