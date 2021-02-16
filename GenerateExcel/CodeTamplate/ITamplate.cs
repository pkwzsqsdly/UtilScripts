using System.Collections.Generic;

public interface ITamplate 
{
	void ToFile(List<CustomExcelTable> tables);
}