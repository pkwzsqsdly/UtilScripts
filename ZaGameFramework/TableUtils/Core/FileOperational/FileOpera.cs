using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LG.TableUtil.FileOperational
{
	public class FileOpera : IFileOperational
	{
		private List<ICryptography> processList;
		public FileOpera()
		{
			processList = new List<ICryptography>();
		}
		public FileOpera AddProcess(ICryptography process)
		{
			processList.Add(process);
			return this;
		}
		public static void WriteToFile(string fullName,byte[] bArr)
		{
			FileStream fs = new FileStream(fullName, FileMode.CreateNew);
			fs.Write(bArr, 0, bArr.Length);
			fs.Flush();
			fs.Close();
		}
		public static List<string> FindAllFiles(string path,string extension = null)
		{
			var list = new List<string>();
			if(Directory.Exists(path))
			{
				DirectoryInfo TheFolder = new DirectoryInfo(path);
				//遍历文件
				foreach (FileInfo NextFile in TheFolder.GetFiles())
				{
					if(!string.IsNullOrEmpty(extension) 
						&& !NextFile.Extension.Equals(extension))
						continue;
		　　　　　　　　// 获取文件完整路径
					string heatmappath = NextFile.FullName;
					list.Add(heatmappath);
				}
			}
			return list;
		}
		public static string ToMD5(string fileName)
		{
			try
			{
				FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				file.Close();
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				return sb.ToString();
			}
			catch (System.Exception exception)
			{
				throw new System.Exception("ToMD5() fail,error:" + exception.Message);
			}
		}
		public byte[] ProcessEncrypt(byte[] bArr)
		{
			byte[] newData = bArr;
			for (int i = 0; i < processList.Count; i++)
			{
				newData = processList[i].Encrypt(newData);
			}
			return newData;
		}
		public byte[] ProcessDecrypt(byte[] bArr)
		{
			byte[] newData = bArr;
			for (int i = processList.Count - 1; i >= 0; i--)
			{
				newData = processList[i].Decrypt(newData);
			}
			return newData;
		}
	}
}