using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class ColorControls : PhoneApplicationPage
    {
        public ColorControls()
        {
            InitializeComponent();
        }

        private void ColorSlider_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Color/ColorSlider.xaml");
        }

        private void ColorPicker_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Color/ColorPicker.xaml");
        }

        private void ColorHexPicker_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Color/ColorHexPicker.xaml");
        }

        private void NavigateTo(string page)
        {
            NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }
    }
}