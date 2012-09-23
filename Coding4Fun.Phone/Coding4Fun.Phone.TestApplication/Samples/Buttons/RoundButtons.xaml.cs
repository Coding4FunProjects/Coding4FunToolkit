using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Coding4Fun.Phone.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Buttons
{
	public partial class RoundButtons : PhoneApplicationPage
	{
		static readonly ImageSource CheckIcon = new BitmapImage(new Uri("/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
		static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));

		public RoundButtons()
		{
			InitializeComponent();
		}

		public ImageSource RoundButtonImage
		{
			get { return (ImageSource)GetValue(RoundButtonImageProperty); }
			set { SetValue(RoundButtonImageProperty, value); }
		}

		// Using a DependencyProperty as the backing store for RoundButtonImage.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RoundButtonImageProperty =
			DependencyProperty.Register("RoundButtonImage", typeof(ImageSource), typeof(RoundButtons), new PropertyMetadata(RepeatIcon));

		private void RoundButtonBasicClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Ding!");
		}

		private void ToggleRoundButtonImageClick(object sender, RoutedEventArgs e)
		{
			RoundButtonImage = (RoundButtonImage != CheckIcon) ? CheckIcon : RepeatIcon;
		}

		private void RoundButtonUnchecked(object sender, RoutedEventArgs e)
		{
			SetIsEnableToType<RoundButton>(disabledRoundButtons, false);
		}

		private void RoundButtonChecked(object sender, RoutedEventArgs e)
		{
			SetIsEnableToType<RoundButton>(disabledRoundButtons, true);
		}

		private static void SetIsEnableToType<T>(FrameworkElement target, bool isEnabled) where T : Control
		{
			var children = target.GetLogicalChildrenByType<T>(false);

			foreach (var child in children)
				child.IsEnabled = isEnabled;
		}

		bool _isRed;
		private void ToggleBackgroundClick(object sender, RoutedEventArgs e)
		{
			_isRed = !_isRed;

			LayoutRoot.Background = new SolidColorBrush(_isRed ? Colors.Red : Colors.Transparent);
		}
	}
}