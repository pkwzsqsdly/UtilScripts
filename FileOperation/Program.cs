using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;

namespace GenerateExcel
{
	public class Program
	{
		public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);
		[Option(ShortName = "p",Description = "input dir path")]
		public string FilePath { get; }
		[Option(ShortName = "o",Description = "out file path")]
		public string OutPath { get; }
		[Option(ShortName = "t",Description = "opera file type")]
		public string OperaType { get; }
		[Option(ShortName = "fc",Description = "read file coding(default gbk)")]
		public string FileCoding { get; }
        [Option(ShortName = "pre",Description = "pre string param")]
		public string Pre { get; }
        [Option(ShortName = "end",Description = "end string param")]
		public string End { get; }
		private void OnExecute()
		{
			if(FilePath == null)
			{
				return;
			}
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var outPath = OutPath ?? "allInOne.txt";

			var operaType = OperaType ?? "connect";

			var fileCoding = FileCoding ?? "gbk";

            switch(operaType)
            {
                case "connect":
                    TextFileConnect fileConn = new TextFileConnect(fileCoding);
                    var files = new List<string>();
                    FileOperaUtil.GetFiles(FilePath,files);
                    fileConn.Connect(files,Pre,End);
                    fileConn.WriteAllInOne(outPath);
                    break;
            }
		}
	}
}
