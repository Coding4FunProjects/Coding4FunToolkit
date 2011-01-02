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
    public partial class Overlays : PhoneApplicationPage
    {
        public Overlays()
        {
            InitializeComponent();
        }

        private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DING!", "dong", MessageBoxButton.OKCancel);
        }

        private void ShowOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Show();
        }

        private void HideOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Hide();
        }
    }
}