using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Color
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