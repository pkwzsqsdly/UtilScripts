
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TextFileConnect 
{
	protected StringBuilder stringBuilder;
	protected Encoding readEncoding;
	public TextFileConnect(string encodingStr)
	{
		stringBuilder = new StringBuilder();
		readEncoding = Encoding.GetEncoding(encodingStr);
	}

	public void Connect(List<string> filePaths,string preStr = null,string endStr = null)
	{
		stringBuilder.Clear();
		
		for (int i = 0; i < filePaths.Count; i++)
		{
			FileInfo info = new FileInfo(filePaths[i]);
			stringBuilder.Append("\n");
			if(!string.IsNullOrEmpty(preStr))
			{
				stringBuilder.Append(string.Format(preStr,i+1));
			}

			stringBuilder.Append(info.Name.Replace(info.Extension,""));

			stringBuilder.Append(readString(info.FullName));

			if(!string.IsNullOrEmpty(endStr))
			{
				stringBuilder.Append(string.Format(endStr,i+1));
			}
			stringBuilder.Append("\n");
			System.Console.WriteLine(info.Name);
		}

	}

	public string readString(string filePath)
	{
		FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		byte[] flieByte = new byte[fs.Length];
		fs.Read(flieByte, 0, flieByte.Length);
		fs.Close();

		byte[] temp = Encoding.Convert(readEncoding,Encoding.UTF8,flieByte);
		string str = Encoding.UTF8.GetString(temp);
		// System.Console.WriteLine(str);
		return str;
	}

	public void WriteAllInOne(string outPath)
	{
		FileStream fs = new FileStream(outPath, FileMode.Create);

		// File.WriteAllText(outPath,stringBuilder.ToString());
		byte[] data = System.Text.Encoding.Default.GetBytes(stringBuilder.ToString());
        //开始写入
        fs.Write(data, 0, data.Length);
        //清空缓冲区，关闭流
        fs.Flush();
        fs.Close();
	}
}