using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Phone.Controls
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
		public const string ContentBodyName = "ContentBody";
	}
}
