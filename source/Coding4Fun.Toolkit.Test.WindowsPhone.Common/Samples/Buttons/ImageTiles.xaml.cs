using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Buttons
{
	public partial class ImageTiles : PhoneApplicationPage
	{
		public ImageTiles()
		{
			InitializeComponent();

			SetItemSource(15);
			//SetItemSource((int) data.Value);
        }

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			fadeTile.CycleImage();
		}

		private void DataValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			SetItemSource((int) e.NewValue);
		}

		private void SetItemSource(int amount)
		{
			var items = new List<Uri>();

			for (int i = 0; i <= amount; i++)
			{
				items.Add(new Uri(String.Format("../../Media/images/{0}.jpg", i), UriKind.Relative));
			}

			fadeTile.ItemsSource = items;
		} 
	}
}