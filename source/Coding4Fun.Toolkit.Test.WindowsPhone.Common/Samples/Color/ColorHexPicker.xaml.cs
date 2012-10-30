using System.Windows;
using System.Windows.Media;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Color
{
    public partial class ColorHexPicker : PhoneApplicationPage
    {
        public ColorHexPicker()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ColorControl_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorFromEvent.Fill = new SolidColorBrush(color);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ColorControl.Color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
        }
    }
}