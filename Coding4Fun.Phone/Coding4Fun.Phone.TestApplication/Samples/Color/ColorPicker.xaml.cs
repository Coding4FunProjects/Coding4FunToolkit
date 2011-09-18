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
            ColorPickerOnLoadTest.Color = Colors.Blue;
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
    }
}