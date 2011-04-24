using System.Windows.Media;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class ColorSlider : PhoneApplicationPage
    {
        public ColorSlider()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ColorSlider_ColorChanged(object sender, Color c)
        {
            ColorFromEvent.Fill = new SolidColorBrush(c);
        }
    }
}