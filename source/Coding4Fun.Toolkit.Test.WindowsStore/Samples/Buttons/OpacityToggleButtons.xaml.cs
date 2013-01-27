using System;

using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using Coding4Fun.Toolkit.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class OpacityToggleButtons
	{
		public OpacityToggleButtons()
		{
			InitializeComponent();
		}

		private async void TileClick(object sender, RoutedEventArgs e)
		{
			var msg = new MessageDialog("You clicked the tile!");
			await msg.ShowAsync();
		}

		bool _isRed;
		private void ToggleBackgroundClick(object sender, RoutedEventArgs e)
		{
			_isRed = !_isRed;

			Background = new SolidColorBrush(_isRed ? Colors.Red : Colors.Transparent);
		}

		private void ToggleChecked(object sender, RoutedEventArgs e)
		{
			
			var isChecked = ToggleCheck != null && ToggleCheck.IsChecked.HasValue && ToggleCheck.IsChecked.Value;

			SetIsEnableToType<ToggleButtonBase>(DisableViewStateTest, isChecked);
		}

		private static void SetIsEnableToType<T>(FrameworkElement target, bool isEnabled) where T : Control
		{
			var children = target.GetLogicalChildrenByType<T>(false);

			foreach (var child in children)
				child.IsEnabled = isEnabled;
		}
	}
}
