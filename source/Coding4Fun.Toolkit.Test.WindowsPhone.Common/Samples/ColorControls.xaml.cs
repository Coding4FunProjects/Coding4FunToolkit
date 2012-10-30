using System;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
    public partial class ColorControls : PhoneApplicationPage
    {
        public ColorControls()
        {
            InitializeComponent();
        }

        private void ColorSliderClick(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Color/ColorSlider.xaml");
        }

        private void ColorPickerClick(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Color/ColorPicker.xaml");
        }

        private void ColorHexPickerClick(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Color/ColorHexPicker.xaml");
        }

        private void NavigateTo(string page)
        {
            NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }
    }
}