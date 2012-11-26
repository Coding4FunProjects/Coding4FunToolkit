using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	class Program
	{
		private static readonly char[] ArgDelimiters = new[] {'-', '/', '\\'};

		

		static int Main(string[] args)
		{
			var targetPlatformArg = "";
			var successfulMerge = false;
			var isTestMode = false;

			if (args.Length > 0)
			{
				targetPlatformArg = args[0].TrimStart(ArgDelimiters);

				isTestMode = args.Any(s => s.ToLower().TrimStart(ArgDelimiters) == Constants.TestMode);
			}

			var targetPlatform = SystemTargets.GetSystemTargetFromArgument(targetPlatformArg);

			var engine = new Merger(targetPlatform);
			engine.ProcessFile("WinPhone.xaml");

			Console.ReadLine();
			return successfulMerge ? 0 : -1;
		}

		

		private static bool ProcessFiles(SystemTarget target)
		{
			string path = FilePaths.GenerateGenericFilePath(target);

			Console.WriteLine(path);

			return true;
		}
	}
}
