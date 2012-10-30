﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Buttons
{
	public partial class RoundToggleButtons : PhoneApplicationPage
	{
		static readonly ImageSource AddIcon = new BitmapImage(new Uri("/Media/icons/appbar.add.rest.png", UriKind.RelativeOrAbsolute));
		static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));

		public RoundToggleButtons()
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
			DependencyProperty.Register("RoundButtonImageSource", typeof(ImageSource), typeof(RoundToggleButtons), new PropertyMetadata(AddIcon));

		private void RoundButtonBasicClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Ding!");
		}

		private void ToggleRoundButtonImageClick(object sender, RoutedEventArgs e)
		{
			RoundButtonImageSource = (RoundButtonImageSource != AddIcon) ? AddIcon : RepeatIcon;
		}

		private void RoundToggleButtonUnchecked(object sender, RoutedEventArgs e)
		{
			SetIsEnableToType<RoundToggleButton>(disabledRoundToggleButtons, false);
		}

		private void RoundToggleButtonChecked(object sender, RoutedEventArgs e)
		{
			SetIsEnableToType<RoundToggleButton>(disabledRoundToggleButtons, true);
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