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

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Buttons : PhoneApplicationPage
    {
        static readonly ImageSource CheckIcon = new BitmapImage(new Uri("/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
        static readonly ImageSource RepeatIcon = new BitmapImage(new Uri("/Media/icons/appbar.repeat.png", UriKind.RelativeOrAbsolute));



        public ImageSource RoundButtonImage
        {
            get { return (ImageSource)GetValue(RoundButtonImageProperty); }
            set { SetValue(RoundButtonImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RoundButtonImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoundButtonImageProperty =
            DependencyProperty.Register("RoundButtonImage", typeof(ImageSource), typeof(Buttons), new PropertyMetadata(RepeatIcon));



        public ImageSource RoundToggleButtonImage
        {
            get { return (ImageSource)GetValue(RoundToggleButtonImageProperty); }
            set { SetValue(RoundToggleButtonImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RoundToggleButtonImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoundToggleButtonImageProperty =
            DependencyProperty.Register("RoundToggleButtonImage", typeof(ImageSource), typeof(Buttons), new PropertyMetadata(RepeatIcon));

        public Buttons()
        {
            InitializeComponent();
            DataContext = this;
        }

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
        
        private void ToggleDirectRoundToggleButtonImage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RoundToggleButton;

            if (button != null)
            {
                RoundToggleButtonImage = (RoundButtonImage != CheckIcon) ? CheckIcon : RepeatIcon;
            }
        }
    }
}