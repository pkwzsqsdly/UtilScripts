using System.IO;
using System.Collections.Generic;
using LG.TableUtil.Json;
using LitJson;
using LG.TableUtil.FileOperational;

namespace LG.TableUtil
{
	public class ManifestExportor : FileOperational.ITableToFile
	{
		private TableUtilProject project;
		private bool isExport;
		public ManifestExportor(TableUtilProject proj,bool export = false)
		{
			project = proj;
			isExport = export;
		}
		public void ToFile(List<CustomTable> tableList)
		{
			var newMeta = project.GenerateNewMeta();
			project.SaveWithNewMeta(newMeta);
			if(isExport)
			{
				string json = LitJson.JsonMapper.ToJson(newMeta);
				string fullName = System.IO.Path.Combine(UserSetting.ConfigOutputPath,"Manifest.json");
				FileOpera.WriteToFile(fullName,System.Text.Encoding.UTF8.GetBytes(json));
			}
		}

		public void UseFileWriter(IFileOperational fileOpera)
		{
		}
	}
	public interface ICustomJsonDataParse
	{
		void ParseToObject(JsonData data);
	}
	public class ManifestMetaItem : ICustomJsonDataParse
	{
		public string name;
		public string md5;
		public int number;
		public void ParseToObject(JsonData data)
		{
			name = JSONHelper.GetString(data,"name");
			md5 = JSONHelper.GetString(data,"md5");
			number = JSONHelper.GetInt(data,"number");
		}
	}

	public class ManifestMeta : ICustomJsonDataParse
	{
		public List<ManifestMetaItem> fileInfos;
		public string version {get => versionNumber.ToString();}
		private VersionNumber versionNumber;
		public ManifestMeta()
		{
			fileInfos = new List<ManifestMetaItem>();
			versionNumber = new VersionNumber();
		}

		public void AddItem(ManifestMetaItem item)
		{
			fileInfos.Add(item);
		}

		public ManifestMetaItem GetItem(string fileName)
		{
			return fileInfos.Find(x => x.name == fileName);
		}

		public void ParseToObject(JsonData data)
		{
			var files = JSONHelper.GetData(data,"fileInfos");
			if(files != null && files.Count > 0)
			{
				for (int i = 0; i < files.Count; i++)
				{
					var item = new ManifestMetaItem();
					item.ParseToObject(files[i]);
					fileInfos.Add(item);
				}
			}
			string ver = JSONHelper.GetString(data,"version");
			versionNumber.SetVersion(ver);
		}
		public VersionNumber GetVersion()
		{
			return versionNumber;
		}
	}

	public class ManifestInfo
	{
		public Dictionary<string,ManifestMeta> ManifestMetas {get;private set;}
		private const string fileName = "Manifest.json";
		private string manifestDefaultPath;
		private ManifestMeta current;
		public ManifestInfo(string path)
		{
			manifestDefaultPath = path;
			ManifestMetas = new Dictionary<string,ManifestMeta>();
		}
		
		public void AddInfo(ManifestMeta info)
		{
			string ver = info.version;
			if(!ManifestMetas.ContainsKey(ver))
			{
				ManifestMetas.Add(ver,info);
			}
			else
			{
				ManifestMetas[ver] = info;
			}
		}
		public ManifestMeta GetInfo(string ver)
		{
			if(ManifestMetas.ContainsKey(ver))
			{
				return ManifestMetas[ver];
			}
			return null;
		}

		public void LoadDefault()
		{
			string fullName = Path.Combine(manifestDefaultPath,fileName);
			LoadManifest(fullName);
		}

		public void LoadManifest(string name)
		{
			StreamReader sr = new StreamReader(name,System.Text.Encoding.UTF8);
			string str = sr.ReadToEnd();
			sr.Dispose();
			sr.Close();

			JsonData json = JsonMapper.ToObject(str);
			var list = JSONHelper.GetData(json,"Subversion");
			if(list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					var info = new ManifestMeta();
					info.ParseToObject(list[i]);
					AddInfo(info);
				}
			}

			string ver = JSONHelper.GetString(json,"latest");
			current = GetInfo(ver);
		}
	}
}