namespace LG.TableUtil.FileOperational
{
	public interface ICryptography
	{
		byte[] Encrypt(byte[] data);
		byte[] Decrypt(byte[] data);
	}
}