using System.Collections.Generic;
namespace LG.TableUtil.Config
{
	public interface ITableLoader 
	{
		List<System.Action<TableContentCollection>> loaderTableList {get;}
	}
}