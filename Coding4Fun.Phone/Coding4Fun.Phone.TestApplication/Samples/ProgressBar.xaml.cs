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
    public partial class ProgressBar : PhoneApplicationPage
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        private void ToggleBar_Click(object sender, RoutedEventArgs e)
        {
            ToggleProgressBar.Visibility = (ToggleProgressBar.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleParent_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleProgressBar.Visibility == Visibility.Collapsed)
                ToggleProgressBar.Visibility = Visibility.Visible;

            parentToPerformanceProgressBar.Visibility = (parentToPerformanceProgressBar.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
            
        }

        private void ToggleAnimationBar_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleProgressBar.Visibility == Visibility.Visible)
                fadeOut.Begin();
            else
            {
                fadeIn.Begin();
            }
        }
    }
}