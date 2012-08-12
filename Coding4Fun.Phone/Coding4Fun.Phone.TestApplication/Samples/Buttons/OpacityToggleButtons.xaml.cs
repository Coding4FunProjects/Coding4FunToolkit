using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Buttons
{
	public partial class OpacityToggleButtons : PhoneApplicationPage
	{
		public OpacityToggleButtons()
		{
			InitializeComponent();
		}

		bool _isRed;
		private void ToggleBackgroundClick(object sender, RoutedEventArgs e)
		{
			_isRed = !_isRed;

			LayoutRoot.Background = new SolidColorBrush(_isRed ? Colors.Red : Colors.Transparent);
		}

		private void OpacityToggleButtonUnchecked(object sender, RoutedEventArgs e)
		{
			SetIsEnableToType<OpacityToggleButton>(opacityButtons, false);
		}

		private void OpacityToggleButtonChecked(object sender, RoutedEventArgs e)
		{
			SetIsEnableToType<OpacityToggleButton>(opacityButtons, true);
		}

		private static void SetIsEnableToType<T>(FrameworkElement target, bool isEnabled) where T : Control
		{
			var children = target.GetLogicalChildrenByType<T>(false);

			foreach (var child in children)
				child.IsEnabled = isEnabled;
		}
	}
}