using Windows.UI.Xaml.Input;

using Coding4Fun.Toolkit.Test.WindowsStore.Samples.Color;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ColorControls
	{
		public ColorControls()
		{
			InitializeComponent();
		}

		private void ColorHex(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ColorHexPicker));
		}
	}
}
