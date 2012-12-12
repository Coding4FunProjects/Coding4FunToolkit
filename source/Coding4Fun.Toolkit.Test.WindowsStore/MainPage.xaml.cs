using System;
using System.Collections.Generic;
using Coding4Fun.Toolkit.Test.WindowsStore.Common;
using Coding4Fun.Toolkit.Test.WindowsStore.Samples;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Coding4Fun.Toolkit.Test.WindowsStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
	public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

		private void ColorHex(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(ColorControls));
		}
    }
}
