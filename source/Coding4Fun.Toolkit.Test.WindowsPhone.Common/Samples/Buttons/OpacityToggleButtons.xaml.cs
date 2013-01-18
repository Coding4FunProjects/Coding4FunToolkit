using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Buttons
{
	public partial class OpacityToggleButtons : PhoneApplicationPage
	{
		public OpacityToggleButtons()
		{
			InitializeComponent();

			ToggleChecked(null, null);
		}

		bool _isRed;
		private void ToggleBackgroundClick(object sender, RoutedEventArgs e)
		{
			_isRed = !_isRed;

			LayoutRoot.Background = new SolidColorBrush(_isRed ? Colors.Red : Colors.Transparent);
		}

		private void ToggleChecked(object sender, RoutedEventArgs e)
		{
			var isChecked = ToggleCheck.IsChecked.HasValue && ToggleCheck.IsChecked.Value;

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