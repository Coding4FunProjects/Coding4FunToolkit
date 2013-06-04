using Windows.UI.Xaml.Input;

using Coding4Fun.Toolkit.Test.WindowsStore.Samples;

namespace Coding4Fun.Toolkit.Test.WindowsStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
	public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

		private void ColorControlsTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ColorControls));
		}

		private void ButtonControlsTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ButtonControls));
		}

		private void AudioWrappersTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(Samples.Audio));
		}

		private void StorageTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(Samples.Storage));
		}

		private void ChatBubbleTapped(object sender, TappedRoutedEventArgs e)
		{
			Frame.Navigate(typeof(ChatBubbleControls));
		}

        private void SuperImageTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof (Samples.SuperImage));
        }
    }
}
