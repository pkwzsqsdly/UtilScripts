
using LG.TableUtil.FileOperational;

public class CodeBase64 : ICryptography
{
	public byte[] Decrypt(byte[] data)
	{
		string res = System.Text.Encoding.Unicode.GetString(data);
		return System.Convert.FromBase64String(res);
	}

	public byte[] Encrypt(byte[] data)
	{
		string str = System.Convert.ToBase64String(data);
		return System.Text.Encoding.Unicode.GetBytes(str);
	}
}