using System.Windows;

using Microsoft.Phone.Controls;
using Coding4Fun.Toolkit.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
    public partial class Slider : PhoneApplicationPage
    {
        public Slider()
        {
            InitializeComponent();
        }

        private void ResultSlider_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            SliderResult.Text = e.NewValue.ToString();
        }

		private void ResultWithStepSlider_ValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
			SliderWithStepResult.Text = e.NewValue.ToString();
        }
		

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			sliderVisTest.Visibility = (sliderVisTest.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
		}
    }
}