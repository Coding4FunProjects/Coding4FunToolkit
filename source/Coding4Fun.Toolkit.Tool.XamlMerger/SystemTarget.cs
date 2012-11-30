namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	public enum SystemTarget
	{
		Invalid = -1,
		All,
		WindowsPhone,
		WindowsPhone7,
		WindowsPhone8,
		WindowsStore,
	}

	internal static class SystemTargets
	{
		public static SystemTarget GetSystemTargetFromArgument(string value)
		{
			if (value == null)
				value = "";

			switch (value.ToLowerInvariant())
			{
				case "":
				case Constants.WindowsStoreArg:
					return SystemTarget.WindowsStore;
				case Constants.WindowsPhone7Arg:
					return SystemTarget.WindowsPhone7;
				case Constants.WindowsPhone8Arg:
					return SystemTarget.WindowsPhone8;
				default:
					return SystemTarget.Invalid;
			}
		}

		public static string GetSystemTargetPath(SystemTarget target)
		{
			switch (target)
			{
				case SystemTarget.WindowsPhone7:
					return Constants.WindowsPhone7;
				case SystemTarget.WindowsPhone8:
					return Constants.WindowsPhone8;
				case SystemTarget.WindowsStore:
					return Constants.WindowsStore;
				default:
					return null;
			}
		}
	}
}