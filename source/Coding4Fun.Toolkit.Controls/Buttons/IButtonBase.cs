#if WINDOWS_STORE

using Windows.UI.Xaml.Controls;

#elif WINDOWS_PHONE

using System.Windows.Controls;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public interface IButtonBase
	{
		object Label { get; set; }
	}

	public interface IAppBarButton
	{
		double ButtonWidth { get; set; }
		double ButtonHeight { get; set; }

		Orientation Orientation { get; set; }
	}

	internal struct ButtonBaseConstants
	{
		public const string ContentBodyName = "ContentBody";
		public const string DisabledContentControlName = "DisabledContent";
		public const string EnabledContentControlName = "EnabledContent";

		public const string ContentHostName = "ContentHost";
		public const string DisabledContentHostName = "DisabledContentHost";
		
		public const string EnabledHolderName = "EnabledHolder";
		public const string DisabledHolderName = "DisabledHolder";
		
		public const string ContentTitleName = "ContentTitle";
	}
}
