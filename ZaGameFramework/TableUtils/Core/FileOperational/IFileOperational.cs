namespace LG.TableUtil.FileOperational
{
	public interface IFileOperational
	{
		byte[] ProcessEncrypt(byte[] bArr);
		byte[] ProcessDecrypt(byte[] bArr);
	}
}