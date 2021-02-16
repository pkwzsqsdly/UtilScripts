using System.Collections.Generic;

public interface IWrite2File
{
	void RegisterType();
	void Write(CustomExcelTable table);
}

public class FileWriter
{
	protected IWrite2File fileWriter;

	public FileWriter(IWrite2File writer)
	{
		fileWriter = writer;
		fileWriter.RegisterType();
	}

	public void Write(List<CustomExcelTable> tables)
	{
		for (int i = 0; i < tables.Count; i++)
		{
			fileWriter.Write(tables[i]);
		}
	}
}