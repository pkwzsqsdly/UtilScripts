using System.Reflection;
using System.Collections.Generic;

public class ExcelUtils {
	private static Dictionary<string,System.Type> classMap = new Dictionary<string, System.Type>();
	public static object MapTo<T>(T t,string key,params object[] args)
	{
		var atts = t.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
		foreach(var att in atts)
		{
			if(att.IsDefined(typeof(MapToAttribute)))
			{
				var ktm = att.GetCustomAttribute<MapToAttribute>();
				if (ktm != null && ktm.Key.Equals(key))
				{
					return att.Invoke(t,args);
				}
			}
		}
		return null;
	}

	public static void RegisterAllClass()
	{
		var asm = Assembly.GetEntryAssembly();
		var list = asm.GetExportedTypes();
		foreach(var item in list)
		{
			var atts = item.GetCustomAttributes<MapToAttribute>();
			foreach(var att in atts)
			{
				if (!classMap.ContainsKey(att.Key))
				{
					classMap.Add(att.Key,item);
				}
			}
		}
	}

	public static T ClassFactory<T>(string key) where T : class
	{
		if (!classMap.ContainsKey(key))
			return null;

		System.Type ctype = classMap[key];
		var cls = System.Activator.CreateInstance(ctype);
		return cls as T;
	}
}
