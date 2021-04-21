using System.Collections.Generic;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil
{
	public class ClassFactory 
	{
		private Dictionary<string,System.Func<ITableToFile>> classDic;

		public ClassFactory()
		{
			classDic = new Dictionary<string, System.Func<ITableToFile>>();
			RegistClass("bin",() => new Bin.BinFileWriter());
			RegistClass("bin_csharp",() => new Bin.CSharpForBin());
			RegistClass("json",() => new Json.JsonFileWriter());
			RegistClass("json_csharp",() => new Json.CSharpForJson());
			RegistClass("lua",() => new Lua.LuaFileWriter());
		}

		public void RegistClass(string key,System.Func<ITableToFile> func)
		{
			if(!classDic.ContainsKey(key))
			{
				classDic.Add(key,func);
			}
			else throw new System.Exception($"key:{key} is exsit!");
		}

		public ITableToFile GetClass(string key)
		{
			return classDic.ContainsKey(key) ? classDic[key].Invoke() : null;
		}

		public T GetClass<T>(string key) where T : class,ITableToFile
		{
			return GetClass(key) as T;
		}
	}
}