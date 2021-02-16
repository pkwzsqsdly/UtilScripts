using System.IO;
using LitJson;

public class LocalConfig : Singleton<LocalConfig>
{
	private JsonData jsonData;
	public override void Init()
	{
		loadConfig();
		if(!Directory.Exists("Out"))
		{
			Directory.CreateDirectory("Out");
		}
	}
	
	protected void loadConfig()
	{
		StreamReader sr = new StreamReader("Config.json", System.Text.Encoding.UTF8);
		string str = sr.ReadToEnd();
		sr.Close();
		jsonData = JsonMapper.ToObject(str);
	}

	public string GetTamplateContrastType(string tamp,string ktype)
	{
		var contData = JSONHelper.GetData(jsonData,"contrastType");
		var data = JSONHelper.GetData(contData,tamp);
		var varData = JSONHelper.GetData(data,"variable");
		return JSONHelper.GetString(varData,ktype);
	}
	public string GetTamplateTypeReadVar(string tamp,string ktype)
	{
		var contData = JSONHelper.GetData(jsonData,"contrastType");
		var data = JSONHelper.GetData(contData,tamp);
		var varData = JSONHelper.GetData(data,"readVar");
		return JSONHelper.GetString(varData,ktype);
	}
	public string GetDesc(string tamp)
	{
		var contData = JSONHelper.GetData(jsonData,"contrastType");
		var data = JSONHelper.GetData(contData,tamp);
		var varData = JSONHelper.GetString(data,"description");
		return varData;
	}
	public string GetFullName(string tamp,string name)
	{
		var contData = JSONHelper.GetData(jsonData,"contrastType");
		var data = JSONHelper.GetData(contData,tamp);
		var varData = JSONHelper.GetString(data,"fileSuffix");
		return "Out/" + name + varData;
	}
}