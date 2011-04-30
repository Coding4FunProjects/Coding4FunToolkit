using System.Windows.Media;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Color : PhoneApplicationPage
    {
        public Color()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ColorSlider_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorSliderFromEvent.Fill = new SolidColorBrush(color);
        }

        private void ColorPicker_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorPickerFromEvent.Fill = new SolidColorBrush(color);
        }
    }
}