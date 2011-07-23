using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.ButtonSamples
{
    public partial class RoundButtons : PhoneApplicationPage
    {
        public RoundButtons()
        {
            InitializeComponent();
            DataContext = this;
        }

        static readonly ImageSource CheckIcon = new BitmapImage(new Uri("/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
        static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));

        public ImageSource RoundButtonImage
        {
            get { return (ImageSource)GetValue(RoundButtonImageProperty); }
            set { SetValue(RoundButtonImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RoundButtonImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoundButtonImageProperty =
            DependencyProperty.Register("RoundButtonImage", typeof(ImageSource), typeof(RoundButtons), new PropertyMetadata(RepeatIcon));

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ding!");
        }

        private void ToggleDirectRoundButtonImage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RoundButton;

            if (button != null)
            {
                RoundButtonImage = (RoundButtonImage != CheckIcon) ? CheckIcon : RepeatIcon;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundRect.Visibility = (BackgroundRect.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}