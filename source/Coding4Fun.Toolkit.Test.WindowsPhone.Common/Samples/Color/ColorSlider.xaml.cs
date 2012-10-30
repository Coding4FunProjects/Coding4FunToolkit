using System.Windows.Media;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Color
{
    public partial class ColorSlider : PhoneApplicationPage
    {
        public ColorSlider()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ColorHorizontalSlider_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorSliderHorizontalFromEvent.Fill = new SolidColorBrush(color);
            ColorSliderSetViaEvent.Color = color;
        }

        private void ColorVerticalSlider_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorSliderVerticalFromEvent.Fill = new SolidColorBrush(color);
            ColorSliderVerticalClone.Color = color;
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