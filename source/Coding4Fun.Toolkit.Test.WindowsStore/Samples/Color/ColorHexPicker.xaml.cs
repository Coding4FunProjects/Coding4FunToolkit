using System;
using System.Collections.Generic;
using Coding4Fun.Toolkit.Test.WindowsStore.Common;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Color
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
	public sealed partial class ColorHexPicker
    {
        public ColorHexPicker()
        {
            InitializeComponent();

			DataContext = this;
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
