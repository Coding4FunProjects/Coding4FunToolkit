using System;
using System.IO;
using System.Reflection;

namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	public static class FilePaths
	{
		public static string BaseFilePath = Path.Combine(
			BaseFolderPath,
			Constants.ControlFolder,
			Constants.ThemesFolder,
			Constants.GenericTarget);

		// source/Coding4Fun.Toolkit.Controls/Themes/Generic/SYSTEM/Generic.Xaml"
		public static string GenerateGenericFilePath(string arg)
		{
			return GenerateGenericFilePath(SystemTargets.GetSystemTargetFromArgument(arg));
		}

		public static string GenerateGenericFilePath(SystemTarget target)
		{
			return Path.Combine(BaseFilePath, SystemTargets.GetSystemTargetPath(target), Constants.GenericThemeXaml);
		}

		public static string GetExecutingAssemblyFilePath()
		{
			var returnVal = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

			if (returnVal != null && returnVal.StartsWith(Constants.FileCmdDeclare))
				returnVal = returnVal.Remove(0, Constants.FileCmdDeclare.Length);

			return returnVal;
		}

		private static string _baseFolderPath;

		public static string BaseFolderPath
		{
			get
			{
				if (!string.IsNullOrEmpty(_baseFolderPath))
					return _baseFolderPath;

				var di = new DirectoryInfo(GetExecutingAssemblyFilePath());
				var found = false;

				while (di != null && !IsBase(di))
				{
					var childern = di.GetDirectories(Constants.BaseFolder);

					if (childern.Length != 0)
					{
						foreach (var child in childern)
						{
							if (!IsBase(child))
								continue;

							di = child;
							found = true;
							break;
						}
					}

					if (found)
						break;

					if (di.Parent != null)
					{
						di = di.Parent;
					}
				}

				if (!IsBase(di))
					throw new Exception("Can't find " + Constants.BaseFolder);

				_baseFolderPath = di.FullName;

				return _baseFolderPath;
			}

			set { _baseFolderPath = value; }
		}

		private static bool IsBase(FileSystemInfo di)
		{
			return (di.Name.ToLower() == Constants.BaseFolder.ToLower());
		}
	}
}