using System.Collections.Generic;
using System.IO;
using LG.TableUtil.FileOperational;


namespace LG.TableUtil.Bin
{
	public interface IBinWriterParse : IKeywordTypeParse
	{
		void ParseTo(ByteArray bArr,CellData data);
	}
	public interface IBinReaderParse : IKeywordTypeParse
	{
		object ReadFor(ByteArray bArr);
	}
	public class BinFileWriter : TypeParsing,ITableToFile
	{
		private IFileOperational fileOperational;
		public BinFileWriter()
		{
			BindType("int"		,new BinKeywordInt()	);
			BindType("float"	,new BinKeywordFloat()	);
			BindType("string"	,new BinKeywordString()	);
			BindType("iArray"	,new BinKeywordIArray()	);
			BindType("fArray"	,new BinKeywordFArray()	);
			BindType("sArray"	,new BinKeywordSArray()	);
		}

		public void ToFile(List<CustomExcelTable> tableList)
		{
			for (int i = 0; i < tableList.Count; i++)
			{
				writeToFile(tableList[i]);
			}
		}

		public void UseFileWriter(IFileOperational fileOpera)
		{
			fileOperational = fileOpera ?? new FileOpera();
		}

		private void writeToFile(CustomExcelTable table)
		{
			var fullName = Path.Combine(UserSetting.ConfigOutputPath,$"{table.Name}.bytes");
			//创建二进制写入流的实例
			// BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			ByteArray bArry = new ByteArray();
			

			for (int i = 0; i < table.Row; i++)
			{
				RowData row = table.GetRow(i);
				
				if(!row.IsExport)
					continue;
				
				for (int j = 0; j < table.Col; j++)
				{
					CellData data = table.GetCell(i,j);
					
					if(data.Title.IsIgnore())
						continue;
					
					UserSetting.Log($"Write Cell Row {i} Col {j},Value is {data.InnerValue}");
					
					SeekParse<IBinWriterParse>(data.Title.ValueType)?.ParseTo(bArry,data);
				}
			}
			
			FileOpera.WriteToFile(fullName,fileOperational != null ? 
					fileOperational.ProcessEncrypt(bArry.GetBytes()) : bArry.GetBytes());
			

			// FileStream fileStream = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write);

			// if (cryptography != null)
			// {
			// 	var newData = cryptography.Encrypt(bArry.GetBytes());
			// 	fileStream.Write(newData, 0, newData.Length);
			// }
			// else
			// {
			// 	fileStream.Write(bArry.GetBytes(),0,bArry.Cursor);
			// }
			// fileStream.Flush();
			// //关闭文件流
			// fileStream.Close();
		}
		
		internal class BinKeywordInt : IBinWriterParse
		{
			public void ParseTo(ByteArray bArr,CellData data)
			{
				int res = 0;
				int.TryParse(data.InnerValue,out res);
				bArr.WriteInt(res);
			}
		}

		internal class BinKeywordFloat : IBinWriterParse
		{
			public void ParseTo(ByteArray bArr,CellData data)
			{
				float res = 0;
				float.TryParse(data.InnerValue,out res);
				bArr.WriteFloat(res);
			}
		}
		
		internal class BinKeywordString : IBinWriterParse
		{
			public void ParseTo(ByteArray bArr,CellData data)
			{
				bArr.WriteString(data.InnerValue);
			}
		}
		
		internal class BinKeywordIArray : IBinWriterParse
		{
			public void ParseTo(ByteArray bArr,CellData data)
			{
				string [] splitStrs = data.InnerValue.Split(',');
				bArr.WriteInt(splitStrs.Length);
				for (int i = 0; i < splitStrs.Length; i++)
				{
					int res = int.Parse(splitStrs[i],0);
					bArr.WriteInt(res);
				}
			}
		}

		internal class BinKeywordFArray : IBinWriterParse
		{
			public void ParseTo(ByteArray bArr,CellData data)
			{
				string [] splitStrs = data.InnerValue.Split(',');
				bArr.WriteInt(splitStrs.Length);
				for (int i = 0; i < splitStrs.Length; i++)
				{
					float res = float.Parse(splitStrs[i],0);
					bArr.WriteFloat(res);
				}
			}
		}
		internal class BinKeywordSArray : IBinWriterParse
		{
			public void ParseTo(ByteArray bArr,CellData data)
			{
				string [] splitStrs = data.InnerValue.Split(',');
				bArr.WriteInt(splitStrs.Length);
				for (int i = 0; i < splitStrs.Length; i++)
				{
					bArr.WriteString(splitStrs[i]);
				}
			}
		}
	}
}
