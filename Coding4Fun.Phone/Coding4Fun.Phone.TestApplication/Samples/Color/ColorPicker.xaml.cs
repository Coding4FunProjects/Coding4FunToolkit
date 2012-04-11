using System.Windows;
using System.Windows.Media;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Color
{
    public partial class ColorPicker : PhoneApplicationPage
    {
        public ColorPicker()
        {
            InitializeComponent();

            DataContext = this;
            Loaded += ColorPicker_Loaded;
        }

        void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPickerSetOnLoadTest.Color = Colors.Magenta;
        }

        private void ColorPicker_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorPickerFromEvent.Fill = new SolidColorBrush(color);
            pickerClone.Color = color;
        }

        private void pickerVisibilityToggle_Click(object sender, RoutedEventArgs e)
        {
            pickerVisibilityToggle.Visibility = (pickerVisibilityToggle.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible; 
        }

		private void Cyan_Click(object sender, RoutedEventArgs e)
		{
			SetColor(Colors.Cyan);
		}

		private void Yellow_Click(object sender, RoutedEventArgs e)
		{
			SetColor(Colors.Yellow);
		}

		private void Orange_Click(object sender, RoutedEventArgs e)
		{
			SetColor(Colors.Orange);
		}

		private void Magenta_Click(object sender, RoutedEventArgs e)
		{
			SetColor(Colors.Magenta);
		}

		private void SetColor(System.Windows.Media.Color color)
		{
			ColorPickerSetOnLoadTest.Color = color;
			ColorPickerSetOnXamlLoadTest.Color = color;
		}


		private void ToggleSwitch_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
			IsEnabledViaEvent.IsEnabled = false;
		}

		private void ToggleSwitch_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			IsEnabledViaEvent.IsEnabled = true;
		}
    }
}