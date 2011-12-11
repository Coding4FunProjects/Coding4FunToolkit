using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Slider : PhoneApplicationPage
    {
        public Slider()
        {
            InitializeComponent();
        }

        private void ResultSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderResult.Text = e.NewValue.ToString();
        }

		private void ResultWithStepSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
			SliderWithStepResult.Text = e.NewValue.ToString();
        }
		

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			sliderVisTest.Visibility = (sliderVisTest.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
		}
    }
}