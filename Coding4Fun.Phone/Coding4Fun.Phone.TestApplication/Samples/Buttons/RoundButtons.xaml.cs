﻿using System;
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
		static readonly ImageSource AddIcon = new BitmapImage(new Uri("/Media/icons/appbar.add.rest.png", UriKind.RelativeOrAbsolute));
		static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));

		public RoundButtons()
		{
			InitializeComponent();

			DataContext = this;
		}

		public ImageSource RoundButtonImageSource
		{
			get { return (ImageSource)GetValue(RoundButtonImageSourceProperty); }
			set { SetValue(RoundButtonImageSourceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for RoundButtonImageSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RoundButtonImageSourceProperty =
			DependencyProperty.Register("RoundButtonImageSource", typeof(ImageSource), typeof(RoundButtons), new PropertyMetadata(AddIcon));

		private void RoundButtonBasicClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Ding!");
		}

		private void ToggleRoundButtonImageClick(object sender, RoutedEventArgs e)
		{
			RoundButtonImageSource = (RoundButtonImageSource != AddIcon) ? AddIcon : RepeatIcon;
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