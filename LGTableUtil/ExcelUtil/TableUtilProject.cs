using System.Collections.Generic;
using System.IO;
using LG.TableUtil.FileOperational;
using LitJson;

namespace LG.TableUtil
{
	public class ProjectInfo
	{
		public Dictionary<string,ManifestMeta> fileInfos;
		public string latest;
		public ProjectInfo()
		{
			fileInfos = new Dictionary<string, ManifestMeta>();
		}
		public void Init()
		{
			if(latest == null)
			{
				latest = "ver:0.0.1";
			}
			else
			{
				VersionNumber ver = null;
				foreach (var item in fileInfos)
				{
					if(ver == null || ver < item.Value.GetVersion())
					{
						ver = item.Value.GetVersion();
					}
				}
				latest = ver.ToString();
			}
		}
		public void SetManifestMeta(ManifestMeta meta)
		{
			string ver = meta.version;
			if(fileInfos.ContainsKey(ver))
			{
				fileInfos[ver] = meta;
			}
			else
			{
				fileInfos.Add(ver,meta);
			}
			RewriteLatest(meta);
		}

		private void RewriteLatest(ManifestMeta meta)
		{
			var ver = GetLatest();
			if(ver == null || meta.GetVersion() > ver.GetVersion())
			{
				latest = meta.version;
			}
		}

		public ManifestMeta GetManifestMeta(string verKey)
		{
			if(fileInfos.ContainsKey(verKey))
			{
				return fileInfos[verKey];
			}
			return null;
		}

		public ManifestMeta GetLatest()
		{
			return GetManifestMeta(latest);
		}
	}
	public class TableUtilProject 
	{
		public ProjectInfo projectInfo{get;private set;}
		private string projName = "tableutil.json";
		private string projPath;
		public TableUtilProject(string path)
		{
			projPath = path;
			LoadProject();
		}

		public static TableUtilProject Create(string path)
		{
			return new TableUtilProject(path);
		}

		public ProjectInfo LoadProject()
		{
			string fullName = System.IO.Path.Combine(projPath,projName);
			FileInfo info = new FileInfo(fullName);
			if(info.Exists)
			{
				projectInfo = LoadProject(fullName);
			}
			else
			{
				projectInfo = new ProjectInfo();
				projectInfo.Init();
				Save();
			}
			return projectInfo;
		}

		public void SetManifestMeta(ManifestMeta meta)
		{
			projectInfo.SetManifestMeta(meta);
		}

		public ProjectInfo LoadProject(string fullName)
		{
			System.IO.StreamReader sr = new System.IO.StreamReader(fullName);
			string content = sr.ReadToEnd();
			sr.Dispose();
			sr.Close();

			return JsonMapper.ToObject<ProjectInfo>(content);
		}
		public ManifestMeta GenerateNewMeta()
		{
			ManifestMeta manifestMeta = new ManifestMeta();
			var list = FileOpera.FindAllFiles(UserSetting.ConfigOutputPath);
			for (int i = 0; i < list.Count; i++)
			{
				FileInfo info = new FileInfo(list[i]);
				string justName = info.Name.Replace(info.Extension,"");
				ManifestMetaItem manifestInfo = new ManifestMetaItem()
				{
					name = justName,
					md5 = FileOpera.ToMD5(info.FullName),
					number = 0
				};
				manifestMeta.AddItem(manifestInfo);
			}
			return manifestMeta;
		}
		public void SaveWithNewMeta(ManifestMeta manifestMeta)
		{
			var latestMeta = projectInfo.GetLatest();

			if(latestMeta == null)
			{
				projectInfo.SetManifestMeta(manifestMeta);
				Save();
			}
			else
			{
				bool isChanged = manifestMeta.fileInfos.Count != latestMeta.fileInfos.Count;
				if(!isChanged)
				{
					for (int i = 0; i < latestMeta.fileInfos.Count; i++)
					{
						ManifestMetaItem lastItem = latestMeta.fileInfos[i];

						for (int j = 0; j < manifestMeta.fileInfos.Count; j++)
						{
							ManifestMetaItem currtItem = manifestMeta.fileInfos[j];

							if(!lastItem.name.Equals(currtItem.name))
								continue;
							
							if(!lastItem.md5.Equals(currtItem.md5))
							{
								isChanged = true;
								currtItem.number ++;
							}
						}
					}
				}
				if (isChanged)
				{
					manifestMeta.GetVersion().SetVersion(projectInfo.latest);
					manifestMeta.GetVersion().Upgrade();
					SetManifestMeta(manifestMeta);
					Save();
				}
			}
		}

		public void Save()
		{
			string fullName = System.IO.Path.Combine(projPath,projName);
			string str = JsonMapper.ToJson(projectInfo);
			StreamWriter sw = new StreamWriter(fullName,false,System.Text.Encoding.UTF8);
			sw.Write(str);
			sw.Close();
		}
	}
}