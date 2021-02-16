using System.Collections.Generic;

public class ClassRegistor : Singleton<ClassRegistor>
{
	private Dictionary<string, System.Type> m_Dic;
	public ClassRegistor()
	{
		m_Dic = new Dictionary<string, System.Type>();
	}

	public override void Init()
	{
		RegistClass("bin",		typeof(BinFileWriter)		);
		RegistClass("json",		typeof(JsonFileWrite)		);
	}

	public void RegistClass(string t, System.Type tc)
	{
		if (!m_Dic.ContainsKey(t))
		{
			m_Dic.Add(t, tc);
		}
		else
		{
			m_Dic[t] = tc;
		}
	}
	public object Create(string t)
	{
		if (m_Dic.ContainsKey(t))
		{
			return System.Activator.CreateInstance(m_Dic[t]);
		}
		return null;
	}
}