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
    public partial class ColorPicker : PhoneApplicationPage
    {
        public ColorPicker()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ColorPicker_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            ColorPickerFromEvent.Fill = new SolidColorBrush(color);
        }
    }
}