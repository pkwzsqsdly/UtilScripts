
using System.Collections.Generic;
using System.IO;

namespace LG.TableUtil
{
	public class UserSetting {
		public static string ConfigOutputPath = "./Out";
		public static string ClassOutputPath = "./AutoClass";
		public static string InputDocPath = "./Docs";
		public static bool IsExportManifest = false;
		public static bool IsZipCompress = false;

		private static System.Action<string> logHandle;
		public static void Initialized(System.Action<string> log) {
			logHandle = log;

			CheckFolder(ConfigOutputPath);
			CheckFolder(ClassOutputPath);

			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		public static void Log(string str)
		{
			logHandle?.Invoke(str);
		}

		public static void CheckFolder(string path)
		{
			DirectoryInfo info = new DirectoryInfo(path);
			if (info.Exists) 
				Directory.Delete(path,true);
			Directory.CreateDirectory(path);
		}
	}
}
