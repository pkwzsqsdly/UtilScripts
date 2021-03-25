using System.Collections.Generic;

public interface IExcelToFile
{
	ITamplate Tamplate{get;}
	void SetInfo(ITamplate tamp);
	void ToFile(List<CustomExcelTable> tableList);
}