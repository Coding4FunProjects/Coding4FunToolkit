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

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class ButtonControls : PhoneApplicationPage
    {
        public ButtonControls()
        {
            InitializeComponent();
        }

        private void RoundButtons_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/ButtonSamples/RoundButtons.xaml");
        }

        private void RoundToggleButtons_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/ButtonSamples/RoundToggleButtons.xaml");
        }

        private void Tiles_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/ButtonSamples/Tiles.xaml");
        }

        private void NavigateTo(string page)
        {
            NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }
    }
}