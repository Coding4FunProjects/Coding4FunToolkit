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

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Slider : Page
    {
        public Slider()
        {
            this.InitializeComponent();
        }

        private void ResultSlider_ValueChanged(object sender, Controls.PropertyChangedEventArgs<double> e)
        {
            SliderResult.Text = e.NewValue.ToString();
        }

        private void ResultWithStepSlider_ValueChanged(object sender, Controls.PropertyChangedEventArgs<double> e)
        {
            SliderWithStepResult.Text = e.NewValue.ToString();
        }
    }
}
