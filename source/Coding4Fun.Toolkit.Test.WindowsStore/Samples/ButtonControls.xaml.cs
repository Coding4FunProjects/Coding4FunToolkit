using Windows.UI.Xaml.Input;

using Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ButtonControls
	{
		public ButtonControls()
		{
			InitializeComponent();
		}

		private void ImageTileControls(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ImageTiles));
		}

		private void TileControls(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(Tiles));
		}

		private void RoundButtonControls(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(RoundButtons));
		}
		
		private void RoundToggleButtonControls(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(RoundToggleButtons));
		}

		private void OpacityToggleButtonControls(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(OpacityToggleButtons));
		}
		
	}
}
