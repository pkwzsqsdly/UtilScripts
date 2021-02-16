using System.Collections.Generic;
using System.IO;

public class FileOperaUtil 
{
	public static void GetFiles(string path,List<string> fileList)
	{
		DirectoryInfo info = new DirectoryInfo(path);
		if(info.Exists)
		{
			var files = info.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				fileList.Add(files[i].FullName);
			}

			var dirs = info.GetDirectories();
			for (int i = 0; i < dirs.Length; i++)
			{
				GetFiles(dirs[i].FullName,fileList);
			}
		}
	}
}