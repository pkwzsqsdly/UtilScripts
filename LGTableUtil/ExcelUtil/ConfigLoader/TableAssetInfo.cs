
namespace LG.TableUtil.Config
{
	public class TableAssetInfo {
		public string configPath;
		public ITableLoader loader;
		public ITableSheetCreator creator;

		public TableAssetInfo()
		{
		}
	}
}