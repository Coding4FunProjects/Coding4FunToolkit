namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
	public class Constants
	{
        public static readonly char[] ArgDelimiters = new[] { '-', '/', '\\' };
        public const string Colon = ":";

		public const string WindowsPhone7Arg = "wp7";
		public const string WindowsPhone8Arg = "wp8";
		public const string WindowsStoreArg = "winstore";
        public static readonly string[] TargetPlatformArgChoices = new[] { WindowsPhone7Arg, WindowsPhone8Arg, WindowsStoreArg };

		public const string WindowsPhone = "Windows Phone";
		public const string WindowsPhone7 = "Windows Phone 7";
		public const string WindowsPhone8 = "Windows Phone 8";
		public const string WindowsStore = "Windows Store";

        public const string WindowsPhoneEndFileName = "(" + WindowsPhone + ")" + XamlExt;
        public const string WindowsPhone7EndFileName = "(" + WindowsPhone7 + ")" + XamlExt;
        public const string WindowsPhone8EndFileName = "(" + WindowsPhone8 + ")" + XamlExt;
        public const string WindowsStoreEndFileName = "(" + WindowsStore + ")" + XamlExt;

		public const string GenericTarget = "generic";
        public const string CommonStyleTarget = "commonstyles"; 
        
        public const string GenericThemeXaml = GenericTarget + XamlExt;

        public const string CommonStyleThemeXaml = GenericTarget + XamlExt;
        public const string CommonStyleWinPhoneThemeXaml = CommonStyleTarget + " " + WindowsPhoneEndFileName;
        public const string CommonStyleWinPhone7ThemeXaml = CommonStyleTarget + " " + WindowsPhone7EndFileName;
        public const string CommonStyleWinPhone8ThemeXaml = CommonStyleTarget + " " + WindowsPhone8EndFileName;
        public const string CommonStyleWinStoreThemeXaml = CommonStyleTarget + " " + WindowsStoreEndFileName;
        
        public const string XamlExt = ".xaml";

		public const string BaseFolder = "source";
		public const string ControlFolder = "Coding4Fun.Toolkit.Controls";
		public const string ThemesFolder = "Themes";

		public const string FileCmdDeclare = "file:\\";

		public const string TestMode = "testmode";

		public const string UsingNamespace = "using";
		public const string Xmlns = "xmlns";
		public const string ClrNamespace = "clr-namespace";

		public const string KeyAttribute = "x:Key";
		public const string TargetTypeAttribute = "TargetType";

        public const string StyleNode = "Style";
	    public const string ResourceDictionaryNode = "ResourceDictionary";
		public const string ThemeDictionariesNode = "ResourceDictionary.ThemeDictionaries";

	    public const string WinPhoneOnlyResource = "{StaticResource Phone";
	    public const string WinStoreOnlyResource = "{StaticResource System";

        public const string DefaultTheme = "Default";
        public const string LightTheme = "Light";
        public const string HighContrastTheme = "HighContrast";
	}
}
