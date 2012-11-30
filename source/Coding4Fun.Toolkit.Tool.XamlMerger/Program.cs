using System;
using System.Linq;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	internal class Program
	{
		private static int Main(string[] args)
		{
			var targetPlatformArg = "";
			var successfulMerge = true;
			var isTestMode = false;

			if (args.Length > 0)
			{
				targetPlatformArg = Constants.TargetPlatformArgChoices.FirstOrDefault(
					target => args.Any(s => target == s.TrimStart(Constants.ArgDelimiters)));

				isTestMode = args.Any(s => s.ToLower().TrimStart(Constants.ArgDelimiters) == Constants.TestMode);
			}

			var targetPlatform = SystemTargets.GetSystemTargetFromArgument(targetPlatformArg);
			var engine = new Merger(targetPlatform, isTestMode);

			successfulMerge &= engine.ProcessXamlFiles();

			if (!successfulMerge)
			{
				Console.WriteLine("There are errors, please fix");
				Console.WriteLine("Press any key to exit");

				Console.Read();
			}
			else
			{
				var returnVal = engine.GenerateGenericXamlFile();
			}

			return successfulMerge ? 0 : -1;
		}
	}
}