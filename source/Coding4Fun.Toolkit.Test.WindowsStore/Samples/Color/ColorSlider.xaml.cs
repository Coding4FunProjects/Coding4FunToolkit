using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Color
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ColorSlider : Page
    {
        public ColorSlider()
        {
            this.InitializeComponent();
        }

        private void ColorHorizontalSlider_ColorChanged(object sender, Windows.UI.Color color)
        {
            ColorSliderHorizontalFromEvent.Fill = new SolidColorBrush(color);
            ColorSliderSetViaEvent.Color = color;
        }

        private void ColorVerticalSlider_ColorChanged(object sender, Windows.UI.Color color)
        {
            ColorSliderVerticalFromEvent.Fill = new SolidColorBrush(color);
            ColorSliderVerticalClone.Color = color;
        }
    }
}
