
using System.Collections.Generic;

namespace LG.TableUtil.FileOperational
{
	public interface ITableToFile
	{
		void ToFile(List<CustomExcelTable> tableList);
		void UseFileWriter(IFileOperational fileOpera);
	}
}