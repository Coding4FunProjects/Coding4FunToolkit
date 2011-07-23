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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.ButtonSamples
{
    public partial class RoundToggleButtons : PhoneApplicationPage
    {
        public RoundToggleButtons()
        {
            InitializeComponent();
            DataContext = this;
        }

        static readonly ImageSource CheckIcon = new BitmapImage(new Uri("/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
        static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));

        public ImageSource RoundToggleButtonImage
        {
            get { return (ImageSource)GetValue(RoundToggleButtonImageProperty); }
            set { SetValue(RoundToggleButtonImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RoundToggleButtonImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoundToggleButtonImageProperty =
            DependencyProperty.Register("RoundToggleButtonImage", typeof(ImageSource), typeof(RoundToggleButtons), new PropertyMetadata(RepeatIcon));

        private void RoundToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ding!");
        }

        private void ToggleDirectRoundToggleButtonImage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RoundToggleButton;

            if (button != null)
            {
                RoundToggleButtonImage = (RoundToggleButtonImage != CheckIcon) ? CheckIcon : RepeatIcon;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundRect.Visibility = (BackgroundRect.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}