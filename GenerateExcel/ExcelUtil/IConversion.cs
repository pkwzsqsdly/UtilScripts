using System.IO;

public interface IConversion {
	void Write(BinaryWriter bw,string str){}
}

public class IntConversion : IConversion
{
	public void Write(BinaryWriter bw,string val)
	{
		int res = 0;
		if(int.TryParse(val,out res))
		{
			bw.Write(res);
		}
	}
}
public class FloatConversion : IConversion
{
	public void Write(BinaryWriter bw,string val)
	{
		float res = 0;
		if(float.TryParse(val,out res))
		{
			bw.Write(res);
		}
	}
}
public class StringConversion : IConversion
{
	public void Write(BinaryWriter bw,string val)
	{
		if(!string.IsNullOrEmpty(val))
		{
			bw.Write(val);
		}
	}
}

public class IntListConversion : IConversion
{
	public void Write(BinaryWriter bw,string val)
	{
		if(!string.IsNullOrEmpty(val))
		{
			string[] list = val.Split(",");
			bw.Write(list.Length);
			for (int i = 0; i < list.Length; i++)
			{
				int n = 0;
				if(int.TryParse(list[i],out n))
				{
					bw.Write(n);
				}
			}
		}
	}
}

public class FloatListConversion : IConversion
{
	public void Write(BinaryWriter bw,string val)
	{
		if(!string.IsNullOrEmpty(val))
		{
			string[] list = val.Split(",");
			bw.Write(list.Length);
			for (int i = 0; i < list.Length; i++)
			{
				float n = 0;
				if(float.TryParse(list[i],out n))
				{
					bw.Write(n);
				}
			}
		}
	}
}

public class StringListConversion : IConversion
{
	public void Write(BinaryWriter bw,string val)
	{
		if(!string.IsNullOrEmpty(val))
		{
			string[] list = val.Split(",");
			bw.Write(list.Length);
			for (int i = 0; i < list.Length; i++)
			{
				bw.Write(list[i]);
			}
		}
	}
}