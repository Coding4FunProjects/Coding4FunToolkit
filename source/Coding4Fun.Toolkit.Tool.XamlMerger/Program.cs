using System;
using System.Linq;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	class Program
	{
		private static readonly char[] ArgDelimiters = new[] {'-', '/', '\\'};

		static int Main(string[] args)
		{
			var targetPlatformArg = "";
			var successfulMerge = true;
			var isTestMode = false;

			if (args.Length > 0)
			{
				targetPlatformArg = args[0].TrimStart(ArgDelimiters);

				isTestMode = args.Any(s => s.ToLower().TrimStart(ArgDelimiters) == Constants.TestMode);
			}

			var targetPlatform = SystemTargets.GetSystemTargetFromArgument(targetPlatformArg);

		    targetPlatform = SystemTarget.WindowsStore;
			var engine = new Merger(targetPlatform);
            successfulMerge &= engine.ProcessFile("WinPhone.xaml");

		    if (!successfulMerge)
		    {
		        Console.WriteLine("There are errors, please fix");
                Console.WriteLine("Press any key to exit");

		        Console.Read();
		    }

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
