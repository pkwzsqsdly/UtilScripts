
using System.Collections.Generic;

namespace LG.TableUtil.FileOperational
{
	public interface ITableToFile
	{
		void ToFile(List<CustomTable> tableList);
		void UseFileWriter(IFileOperational fileOpera);
	}
}