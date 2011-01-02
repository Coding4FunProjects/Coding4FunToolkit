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
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void TimeSpan_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Timespan.xaml");
        }

        private void Overlays_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Overlays.xaml");
        }

        private void Prompts_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Prompts.xaml");
        }

        private void Progress_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/ProgressBar.xaml");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/AboutItems.xaml");
        }

        private void Memory_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Memory.xaml");
        }
        
        private void NavigateTo(string page)
        {
            this.NavigationService.Navigate(new Uri(page, UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo("/Samples/Buttons.xaml");
        }

    }
}