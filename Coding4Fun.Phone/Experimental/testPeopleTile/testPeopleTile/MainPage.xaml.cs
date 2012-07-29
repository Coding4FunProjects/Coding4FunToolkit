using System;
using System.Collections.Generic;

using Microsoft.Phone.Controls;

namespace testPeopleTile
{
    public partial class MainPage : PhoneApplicationPage
    {
		public MainPage()
        {
            InitializeComponent();

			SetItemSource((int) data.Value);
        }

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			fadeTile.CycleImage();
		}

		private void data_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
		{
			SetItemSource((int) e.NewValue);
		}

		private void SetItemSource(int amount)
		{
			var items = new List<Uri>();

			for (int i = 0; i <= amount; i++)
			{
				items.Add(new Uri(String.Format("Images/{0}.jpg", i), UriKind.Relative));
			}

			fadeTile.ItemsSource = items;
		}
    }
}