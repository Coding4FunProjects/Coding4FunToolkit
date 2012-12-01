using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Coding4Fun.Toolkit.Test.WindowsStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		private void ColorControl_ColorChanged(object sender, Windows.UI.Color color)
		{
			_colorFromEvent.Fill = new SolidColorBrush(color);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//ColorControl.Color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
			//this.co
			_myColorControl.Color = Colors.Red;
		}
    }
}
