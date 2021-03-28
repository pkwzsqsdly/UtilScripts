using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace LG.TableUtil.FileOperational
{
	public class ZipConfigFiles : ITableToFile
	{
		public void ToFile(List<CustomTable> tableList)
		{
            new ZipCompress(UserSetting.ConfigOutputPath).Process();
		}

		public void UseFileWriter(IFileOperational fileOpera)
		{
		}
	}
	public class ZipCompress : IFileCompress
	{
		private string filePath;
        private string password;
		private string zipFileName;
		public ZipCompress(string srcFilePath, string zipFile = null,string passw = null)
		{
            password = passw;
			
            filePath = srcFilePath.Replace('/', Path.DirectorySeparatorChar);
            if (!filePath.EndsWith(Path.DirectorySeparatorChar))
            {
                filePath = string.Concat(filePath,Path.DirectorySeparatorChar);
            }
			
            
            if (zipFile == null)
            {
                string zipName = filePath;
                if (filePath.EndsWith(Path.DirectorySeparatorChar))
                    zipName = filePath.Substring(0,filePath.Length - 1);
                zipFileName = string.Concat(zipName,".zip");
            }
			else
			{
				zipFileName = zipFile.Replace('/', Path.DirectorySeparatorChar);
			}
		}
		public bool Process()
		{
            bool result = true;
			ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName));
			s.SetLevel(6); // 0 - store only to 9 - means best compression
			if (Directory.Exists(filePath))
			{
				result = CompressFolder(filePath, s, filePath);
			}
			else
			{
                if (!string.IsNullOrEmpty(password)) 
                    s.Password = password;
				var rootPath = filePath.Substring(0, filePath.LastIndexOf(Path.DirectorySeparatorChar));
				result = CompressFile(filePath, s, rootPath);
			}
			s.Finish();
			s.Close();
            return result;
		}
        private void CompressDo(string strFile,ZipOutputStream s,Crc32 crc,out FileStream fs,out ZipEntry entry, string staticFile)
        {
            fs = File.OpenRead(strFile);

            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            string tempfile = strFile.Replace(staticFile, "");
            entry = new ZipEntry(tempfile);
            

            entry.DateTime = DateTime.Now;
            entry.Size = buffer.Length;
            
            crc.Reset();
            crc.Update(buffer);
            entry.Crc = crc.Value;
            
            s.PutNextEntry(entry);
            s.Write(buffer, 0, buffer.Length);
        }
		private bool CompressFile(string strFile, ZipOutputStream s, string staticFile)
		{
            bool result = true;
            FileStream fs = null;
            ZipEntry entry = null;
            try{
                Crc32 crc = new Crc32();
                CompressDo(strFile,s,crc,out fs,out entry,staticFile);
            }
			catch
			{
				result = false;
			}
			finally
			{
				if (s != null)
				{
					s.Finish();
					s.Close();
				}
				if (entry != null)
				{
					entry = null;
				}
				if (fs != null)
				{
					fs.Close();
					fs.Dispose();
				}
			}
			GC.Collect();
			GC.Collect(1);

            return result;
		}
		/// <summary>
		/// 递归压缩文件夹的内部方法
		/// </summary>
		/// <param name="folderToZip">要压缩的文件夹路径</param>
		/// <param name="zipStream">压缩输出流</param>
		/// <param name="parentFolderName">此文件夹的上级文件夹</param>
		/// <returns></returns>
		private bool CompressFolder(string strFile, ZipOutputStream s, string staticFile)
		{
			bool result = true;
			string[] folders, files;
			ZipEntry entry = null;
			FileStream fs = null;
			Crc32 crc = new Crc32();
			try
			{
				files = Directory.GetFiles(strFile);
				foreach (string file in files)
				{
                    CompressDo(file,s,crc,out fs,out entry,staticFile);                  
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
					fs.Dispose();
				}
				if (entry != null)
				{
					entry = null;
				}
				GC.Collect();
				GC.Collect(1);
			}

			folders = Directory.GetDirectories(strFile);
			foreach (string folder in folders)
				if (!CompressFolder(folder, s, staticFile))
					return false;

			return result;
		}

		
		private void CompressFolder2(string strFile, ZipOutputStream s, string staticFile)
		{
			string[] filenames = Directory.GetFiles(strFile);
			foreach (string file in filenames)
			{

				if (Directory.Exists(file))
				{
					CompressFolder(file, s, staticFile);
				}
				else // 否则直接压缩文件
				{
					CompressFile(file, s, staticFile);
				}
			}
		}
	}
	public class ZipUnCompress : IFileCompress
	{
		private string filePath;
		private string zipFileName;
        private string password;
		public ZipUnCompress(string zipFile, string outFilePath,string passw = null)
		{
			zipFileName = zipFile.Replace('/', Path.DirectorySeparatorChar);
            password = passw;
			if (outFilePath == null)
			{
				filePath = zipFileName.Substring(0,zipFileName.LastIndexOf(Path.DirectorySeparatorChar));
			}
            else
            {
                filePath = outFilePath.Replace('/',Path.DirectorySeparatorChar);
            }
            if (!filePath.EndsWith(Path.DirectorySeparatorChar))
                filePath = string.Concat(filePath,Path.DirectorySeparatorChar);
		}
		/// <summary>
		/// 解压功能(解压压缩文件到指定目录)
		/// </summary>
		/// <param name="fileToUnZip">待解压的文件</param>
		/// <param name="zipedFolder">指定解压目标目录</param>
		/// <param name="password">密码</param>
		/// <returns>解压结果</returns>
		public bool Process()
		{
			bool result = true;
			FileStream fs = null;
			ZipInputStream zipStream = null;
			ZipEntry ent = null;
			string fileName;

			if (!File.Exists(zipFileName))
				return false;

			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			try
			{
				zipStream = new ZipInputStream(File.OpenRead(zipFileName));
				if (!string.IsNullOrEmpty(password)) 
                    zipStream.Password = password;
				while ((ent = zipStream.GetNextEntry()) != null)
				{
					if (!string.IsNullOrEmpty(ent.Name))
					{
						fileName = ent.Name.Replace('/', Path.DirectorySeparatorChar);//change by Mr.HopeGi
						fileName = Path.Combine(filePath, fileName);
                        
                        var dir = fileName.Substring(0,fileName.LastIndexOf(Path.DirectorySeparatorChar));
                        if (!Directory.Exists(dir))
                        {
							Directory.CreateDirectory(dir);
                        }
                        
						fs = File.Create(fileName);
						int size = 2048;
						byte[] data = new byte[size];
						while (true)
						{
							size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
								fs.Write(data, 0, size);
							else
								break;
						}
                        fs.Close();
                        fs.Dispose();
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
					fs.Dispose();
				}
				if (zipStream != null)
				{
					zipStream.Close();
					zipStream.Dispose();
				}
				if (ent != null)
				{
					ent = null;
				}
				GC.Collect();
				GC.Collect(1);
			}
			return result;
		}

	}
}