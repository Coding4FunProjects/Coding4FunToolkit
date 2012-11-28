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
		    var engine = new Merger(targetPlatform, isTestMode);

            successfulMerge &= engine.Process();

		    if (!successfulMerge)
		    {
		        Console.WriteLine("There are errors, please fix");
                Console.WriteLine("Press any key to exit");

		        Console.Read();
		    }

		    return successfulMerge ? 0 : -1;
		}
	}
}
