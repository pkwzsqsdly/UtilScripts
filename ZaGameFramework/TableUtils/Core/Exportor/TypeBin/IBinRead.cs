using LG.TableUtil.Config;
using LG.TableUtil.FileOperational;


namespace LG.TableUtil.Bin
{
	public interface IBinRead : ITableCellData
	{
		void Read(ByteArray bArr);
	}
}