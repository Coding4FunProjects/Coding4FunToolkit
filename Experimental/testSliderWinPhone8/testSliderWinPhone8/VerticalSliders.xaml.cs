using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace testSliderWinPhone8
{
	public partial class VerticalSliders : PhoneApplicationPage
	{
		public VerticalSliders()
		{
			InitializeComponent();
		}

		private void ColorClick(object sender, RoutedEventArgs e)
		{
			NavigationHelper.NavigateTo("ColorSliders.xaml");
		}

		private void VerticalClick(object sender, RoutedEventArgs e)
		{
			NavigationHelper.NavigateTo("VerticalSliders.xaml");
		}

		private void HorizontalClick(object sender, RoutedEventArgs e)
		{
			NavigationHelper.NavigateTo("MainPage.xaml");
		}
	}
}