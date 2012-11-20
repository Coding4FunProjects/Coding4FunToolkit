using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	class Program
	{
		static int Main(string[] args)
		{
			string targetPlatform = "";
			bool successfulMerge = false;

			if (args.Length > 0)
				targetPlatform = args[0].TrimStart(new[] { '-', '/', '\\' });

			string path = FilePaths.GenerateGenericFilePath(targetPlatform);

			Console.WriteLine(path);
			Console.ReadLine();

			return successfulMerge ? 0 : -1;
		}
	}
}
