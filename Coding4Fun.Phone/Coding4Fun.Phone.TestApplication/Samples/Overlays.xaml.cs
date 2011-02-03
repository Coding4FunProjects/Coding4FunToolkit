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
            DataContext = this;
        }

        private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
        }

        private void GestureListener_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("TAP!", "Testing with Gesture Tap", MessageBoxButton.OKCancel);
        }

        private void ShowOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Show();
        }

        private void HideOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Hide();
        }

        private void DirectVis(object sender, RoutedEventArgs e)
        {
            progressOverlay.Visibility = Visibility.Visible;
        }

        private void DirectCol(object sender, RoutedEventArgs e)
        {
            progressOverlay.Visibility = Visibility.Collapsed;
        }

        private void DataBindVis(object sender, RoutedEventArgs e)
        {
            OverlayVis = Visibility.Visible;
        }

        private void DataBindCol(object sender, RoutedEventArgs e)
        {
            OverlayVis = Visibility.Collapsed;
        }



        public Visibility OverlayVis
        {
            get { return (Visibility)GetValue(OverlayVisProperty); }
            set { SetValue(OverlayVisProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for OverlayVis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayVisProperty =
            DependencyProperty.Register("OverlayVis", typeof(Visibility), typeof(Overlays), new PropertyMetadata(Visibility.Visible));

        
    }
}