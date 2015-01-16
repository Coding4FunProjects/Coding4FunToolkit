using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class Overlay : Page
    {
        public Overlay()
        {
            this.InitializeComponent();

            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Application.Current.DebugSettings.EnableFrameRateCounter = true;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            Application.Current.DebugSettings.EnableFrameRateCounter = false;
        }

        bool _databindingBroke;
        const string databindingError = "Databinding was removed on prior call";

        private void DirectVis(object sender, RoutedEventArgs e)
        {
            progressOverlay.Visibility = Visibility.Visible;
            _databindingBroke = true;
        }

        private void DirectCol(object sender, RoutedEventArgs e)
        {
            progressOverlay.Visibility = Visibility.Collapsed;
            _databindingBroke = true;
        }

        private void DataBindVis(object sender, RoutedEventArgs e)
        {
            CheckDataBinding();
            OverlayVis = Visibility.Visible;
        }

        private void DataBindCol(object sender, RoutedEventArgs e)
        {
            CheckDataBinding();
            OverlayVis = Visibility.Collapsed;
        }

        private async void CheckDataBinding()
        {
            if (_databindingBroke)
                await new MessageDialog(databindingError).ShowAsync();
        }

        public Visibility OverlayVis
        {
            get { return (Visibility)GetValue(OverlayVisProperty); }
            set { SetValue(OverlayVisProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OverlayVis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayVisProperty =
            DependencyProperty.Register("OverlayVis", typeof(Visibility), typeof(Overlay), new PropertyMetadata(Visibility.Visible));


        private void ShowOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Show();
            _databindingBroke = true;
        }

        private void HideOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Hide();
            _databindingBroke = true;
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await new MessageDialog("Testing with Gesture Tap", "TAP!!").ShowAsync();
        }

        private async void Ding_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("CLICK!", "Testing with Click Event").ShowAsync();
        }
    }
}
