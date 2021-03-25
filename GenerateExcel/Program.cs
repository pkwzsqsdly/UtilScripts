using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using LG.TableUtil;
using LG.TableUtil.Bin;
using System.Text;
using LG.TableUtil.Config;
using LG.TableUtil.FileOperational;

public class TestParse : IBinWriterParse,IBinReaderParse,IBinCSharpParse
	{
		public void ParseTo(ByteArray bArry, CellData data)
		{
			var list = data.InnerValue.Split(":");
			UserSetting.Log("list="+list.Length);
			if (list.Length == 2)
			{
				bArry.WriteString(list[0]);
				bArry.WriteString(list[1]);
			}
		}

		public void ParseTo(StringBuilder sbvar, StringBuilder sbctx, CellTitle title)
		{
			sbvar.Append($"\t//{title.Name}\n");
			sbvar.Append($"\tpublic {title.ValueType} {title.KeyWords} {{get; private set;}}\n");
			sbctx.Append($"\t\t{title.KeyWords} = ({title.ValueType})(new TestParse().ReadFor(bArr));\n");
		}

		public object ReadFor(ByteArray bArr)
		{
			var data = new TestObj();
			data.key = bArr.ReadString();
			data.value = bArr.ReadString();
			return data;
		}
	}
	public class TestObj
	{
		public string key;
		public string value;

		
	}
namespace GenerateExcel
{
	
	public class Program
	{
		public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);
		[Option(ShortName = "p",Description = "input file path.(defalut: ./Docs)")]
		public string InFilePath { get; }
		[Option(ShortName = "o",Description = "output file path.(defalut: ./Out)")]
		public string OutFilePath { get; }
		[Option(ShortName = "f",Description = "out file type.(defalut: bin)")]
		public string FileType { get; }
		[Option(ShortName = "t",Description = "out class type.(defalut: null)")]
		public string ClassType { get; }
		[Option(ShortName = "w",Description = "out class path.(defalut: ./AutoClass)")]
		public string ClassPath { get; }
		[Option(ShortName = "c",Description = "out file crypto key.(defalut: null)")]
		public string CryptoKey { get; }
		[Option(ShortName = "m",Description = "export manifest file.(defalut: false)")]
		public bool IsManifest { get; }
		[Option(ShortName = "z",Description = "zip data file.(defalut: false)")]
		public bool IsZip { get; }


		private void OnExecute()
		{
			UserSetting.InputDocPath 		= InFilePath 	?? UserSetting.InputDocPath;
			UserSetting.ConfigOutputPath 	= OutFilePath 	?? UserSetting.ConfigOutputPath;
			UserSetting.ClassOutputPath 	= ClassPath 	?? UserSetting.ClassOutputPath;
			UserSetting.IsExportManifest 	= IsManifest;
			UserSetting.IsZipCompress 		= IsZip;

			UserSetting.Initialized(s => {
				System.Console.WriteLine(s);
			});

			
			var b = new ProbabilityGroup<Probability>();
			for (int i = 0; i < 10; i++)
			{
				b.Add(new Probability(1000,i+1));
				// if(d) break;
			}
			Dictionary<int,int> test = new Dictionary<int, int>();
			for (int i = 0; i < 100; i++)
			{
				int a = b.GetResult();
				if(test.ContainsKey(a))
				{
					test[a] += 1;
				}else{
					test.Add(a,1);
				}
			}

			foreach (var item in test)
			{
				UserSetting.Log(item.Key + "," + item.Value.ToString());
			}

			return ;
			List<string> filePathList = FileOpera.FindAllFiles(UserSetting.InputDocPath,".xlsx");

			if(filePathList.Count == 0)
				return;

			var excel = new ExcelLoader();
			
			var project = new TableUtilProject("./");
			project.LoadProject();

			string exportFileType = FileType ?? "bin";
			var fileWriter = ExcelUtils.ExportFactory(exportFileType);
			var fileOpera = fileWriter as ITableToFile;
			// fileOpera.UseFileWriter(new FileOpera()
			// 	.AddProcess(new SimpleCrypto("mabi"))
			// 	.AddProcess(new CodeBase64())
			// );
			if (!string.IsNullOrEmpty(CryptoKey))
			{
				fileOpera.UseFileWriter(new FileOpera()
					.AddProcess(new SimpleCrypto(CryptoKey))
					// .AddProcess(new CodeBase64())
				);
				// fileParse.UseEncrpt(new SimpleCrypto("fuck"));
			}

			excel.AddExportor(fileWriter);

			if(!string.IsNullOrEmpty(ClassType))
			{
				string tampName = $"{exportFileType}_{ClassType}";
				// string tampName = "bin_csharp";
				ITableToFile tampWriter = ExcelUtils.ExportFactory(tampName);
				excel.AddExportor(tampWriter);
			}


			
			excel.AddExportor(new ManifestExportor(project,UserSetting.IsExportManifest));
			if (UserSetting.IsZipCompress)
			{
				excel.AddExportor(new ZipConfigFiles());
			}

			filePathList.ForEach(p => {
				excel.Load(p).GenerateFile();
			});

			
		}

	}
}
