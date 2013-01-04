#if WINDOWS_STORE

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE

using System.Windows.Controls;
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public interface IButtonBase
	{
		Orientation Orientation { get; set; }
		Stretch Stretch { get; set; }
		ImageSource ImageSource { get; set; }
		double ButtonWidth { get; set; }
		double ButtonHeight { get; set; }
	}

	internal struct ButtonBaseConstants
	{
		public const string OpacityImageBrushName = "OpacityImageBrush";
		public const string DisabledOpacityImageBrushName = "DisabledOpacityImageBrush";
		public const string ContentBodyName = "ContentBody";
	}
}
