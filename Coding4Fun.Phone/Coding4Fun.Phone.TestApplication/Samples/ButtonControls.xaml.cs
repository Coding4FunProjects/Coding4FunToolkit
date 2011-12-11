using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class ButtonControls : PhoneApplicationPage
    {
        public ButtonControls()
        {
            InitializeComponent();

			DataContext = this;
        }

		static readonly ImageSource CheckIcon = new BitmapImage(new Uri("/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
		static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));

		public ImageSource RoundButtonImage
		{
			get { return (ImageSource)GetValue(RoundButtonImageProperty); }
			set { SetValue(RoundButtonImageProperty, value); }
		}

		// Using a DependencyProperty as the backing store for RoundButtonImage.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RoundButtonImageProperty =
			DependencyProperty.Register("RoundButtonImage", typeof(ImageSource), typeof(ButtonControls), new PropertyMetadata(RepeatIcon));

		private void RoundButtonBasicClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Ding!");
		}

		private void ToggleRoundButtonImageClick(object sender, RoutedEventArgs e)
		{
			RoundButtonImage = (RoundButtonImage != CheckIcon) ? CheckIcon : RepeatIcon;
		}

		private void TileClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("You clicked the tile!");
		}

		private void PivotSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var item = ButtonPivot.SelectedItem as PivotItem;

			if (item != null && (string) item.Tag == "tile")
				trexStoryboard.Begin();
			else
				trexStoryboard.Stop();

		}

		bool _isRed;
		private void ToggleBackgroundClick(object sender, RoutedEventArgs e)
        {
			_isRed = !_isRed;

			LayoutRoot.Background = new SolidColorBrush(_isRed ? Colors.Red : Colors.Transparent);
        }
    }
}