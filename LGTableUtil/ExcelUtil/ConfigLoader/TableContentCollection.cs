using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LG.TableUtil.Config
{
	public class TableContentCollection : IEnumerable
	{
		public Dictionary<string, TableSheetData> tableContent { get; protected set; }
		private Dictionary<System.Type,string> tableTypeDic;
		private ITableSheetCreator tableSheetCreator;
		public TableContentCollection(ITableSheetCreator creator)
		{
			tableSheetCreator = creator;
			tableContent = new Dictionary<string, TableSheetData>();
			tableTypeDic = new Dictionary<System.Type, string>();
		}
		public static byte[] LoadBytes(string path,string name)
		{
			string fullName = Path.Combine(path,name);
			using (FileStream fs = new FileStream(fullName, FileMode.Open))
			{
				byte[] bytsize = new byte[fs.Length];
				int defaultSize = 1024 * 256;
				int readLength = bytsize.Length > defaultSize ? defaultSize : bytsize.Length;
				int cursor = 0;
				do
				{
					cursor += fs.Read(bytsize, cursor, readLength);
					readLength = System.Math.Min(bytsize.Length - cursor, readLength);
				}
				while (cursor < bytsize.Length);
				return bytsize;
			}
		}

		public static string LoadString(string path,string name)
		{
			byte[] bts = LoadBytes(path,name);
			return Encoding.Default.GetString(bts);
		}

		public void AddTableData(TableSheetData table)
		{
			if (!tableContent.ContainsKey(table.sheetName))
			{
				tableContent.Add(table.sheetName, table);
			}
		}
		public ITableCellData GetTableData(string name,int id)
		{
			if (tableContent.ContainsKey(name))
			{
				return tableContent[name].GetDataById(id);
			}
			return null;
		}
		public T GetTableData<T>(string name,int id) where T : class,ITableCellData
		{
			var data = GetTableData(name,id);
			return data == null ? default(T) : data as T;
		}

		public T GetTableData<T>(int id) where T : class,ITableCellData
		{
			string name = FindTableType<T>();
			return GetTableData<T>(name,id);
		}

		public void LoadTable<T>(string name) where T : ITableCellData, new()
		{
			TableSheetData table = tableSheetCreator?.Create<T>(name);
			if (table != null)
			{
				CachTableType<T>(name);
				AddTableData(table);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return tableContent.GetEnumerator();
		}

		public void CachTableType<T>(string name)
		{
			System.Type t = typeof(T);
			if (!tableTypeDic.ContainsKey(t))
				tableTypeDic.Add(t,name);

			tableTypeDic[t] = name;
		}

		public string FindTableType<T>()
		{
			System.Type t = typeof(T);
			if (tableTypeDic.ContainsKey(t))
				return tableTypeDic[t];
			return null;
		}
	}
}