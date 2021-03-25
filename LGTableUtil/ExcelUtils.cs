using System.Reflection;
using System.Collections.Generic;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil
{
	public class ExcelUtils 
	{
		private static ClassFactory classFactory = new ClassFactory();

		public static void ExtensionExportType(string key,System.Func<ITableToFile> func)
		{
			classFactory.RegistClass(key,func);
		}

		public static ITableToFile ExportFactory(string key)
		{
			return classFactory.GetClass(key);
		}
	}
}