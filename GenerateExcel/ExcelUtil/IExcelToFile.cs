using System.Collections.Generic;

public interface IExcelToFile
{
	string OutputPath{get;}
	ITamplate Tamplate{get;}
	void SetInfo(string path,ITamplate tamp);
	void ToFile(List<CustomExcelTable> tableList);
}