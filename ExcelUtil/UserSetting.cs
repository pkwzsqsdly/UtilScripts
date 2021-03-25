
using System.Collections.Generic;
using System.IO;

public class UserSetting {
	public static string ConfigOutputPath = "./Out";
	public static string ClassOutputPath = "./AutoClass";
	public static string InputDocPath = "./Docs";

	public static System.Action<string> logHandle{get;set;}

	public static void Log(string str)
	{
		logHandle?.Invoke(str);
	}

	public static List<string> FileAllFile(string path,string extension = null)
		{
			var list = new List<string>();
			if(Directory.Exists(path))
			{
				DirectoryInfo TheFolder = new DirectoryInfo(path);
				//遍历文件
				foreach (FileInfo NextFile in TheFolder.GetFiles())
				{
					if(string.IsNullOrEmpty(extension) 
						|| !NextFile.Extension.Equals(extension))
						continue;
		　　　　　　　　// 获取文件完整路径
					string heatmappath = NextFile.FullName;
					list.Add(heatmappath);
				}
			}
			return list;
		}
}