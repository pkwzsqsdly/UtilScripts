using System;
using System.Collections.Generic;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace GenerateExcel
{
	public class Program
	{
		public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);
		[Option(ShortName = "p",Description = "input file path")]
		public string FilePath { get; }
		[Option(ShortName = "o",Description = "out file type")]
		public string OutType { get; }
		[Option(ShortName = "t",Description = "out class type")]
		public string SharpType { get; }

		private List<string> filePathList;
		private void OnExecute()
		{
			ClassRegistor.Inst.Init();
			LocalConfig.Inst.Init();
			
			if(FilePath == null)
			{
				return;
			}

			var outType = OutType ?? "bin";
			IWrite2File wf = (IWrite2File)ClassRegistor.Inst.Create(outType);

			filePathList = fileAllFile(FilePath);

			filePathList.ForEach(p => {
				// new ExcelLoader(new BinFileWriter(),new CSharpTamplate())
				// 	.Load(@"C:\Users\Lee\Desktop\常量表.xlsx").WriteToFile();
				new ExcelLoader(wf).Load(p).WriteToFile();
			});
		}

		private List<string> fileAllFile(string path)
		{
			var list = new List<string>();
			if(File.Exists(FilePath))
			{
				list.Add(path);
			}
			else if(Directory.Exists(FilePath))
			{
				DirectoryInfo TheFolder = new DirectoryInfo(path);
				//遍历文件
				foreach (FileInfo NextFile in TheFolder.GetFiles())
				{
					if (NextFile.Extension != ".xlsx")
						continue;
		　　　　　　　　// 获取文件完整路径
					string heatmappath = NextFile.FullName;
					list.Add(heatmappath);
				}
			}
			return list;
		}

	}
}
