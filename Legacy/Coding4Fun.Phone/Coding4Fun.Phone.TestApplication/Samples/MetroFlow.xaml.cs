using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Coding4Fun.Phone.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
	public partial class MetroFlow : PhoneApplicationPage
	{
		public MetroFlow()
		{
			InitializeComponent();
			
			SetColIndexSliderMax();

			loadImageAtRunTime.ImageSource = new BitmapImage(new Uri(@"..\media\images\trex_360width.jpg", UriKind.RelativeOrAbsolute));

			loadDataAtRunTime.Items.Add(new MetroFlowData { Title = "Test 1", ImageUri = new Uri(@"..\media\images\Robot.jpg", UriKind.RelativeOrAbsolute) });
			loadDataAtRunTime.Items.Add(new MetroFlowData { Title = "Test 2", ImageUri = new Uri(@"..\media\images\trex_360width.jpg", UriKind.RelativeOrAbsolute) });
			loadDataAtRunTime.Items.Add(new MetroFlowData { Title = "Test 3", ImageUri = new Uri(@"..\media\images\lamp.jpg", UriKind.RelativeOrAbsolute) });
			loadDataAtRunTime.Items.Add(new MetroFlowData
			{
				Title = "coding4fun",
				ImageUri = new Uri(@"..\media\headwhite_100.png", UriKind.RelativeOrAbsolute)
			});

			loadDataAtRunTime.Items.Add(new MetroFlowData
			{
				Title = "Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet",
				ImageUri = new Uri(@"..\media\images\Seattle.jpg", UriKind.RelativeOrAbsolute)
			});

			loadDataAtRunTime.SelectedColumnIndex = 2;
		}

		private void SetColIndexSliderMax()
		{
			colIndex.Maximum = loadDataAtDesignTime.Items.Count - 1;
		}

		private void MetroFlow_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var item = ((MetroFlowData)e.AddedItems[0]);

			SelectionChanged.Text = (item != null) ? item.Title : "EMPTY!";
		}

		private void MetroFlow_SelctionTap(object sender, SelectionTapEventArgs e)
		{
			SelectionTap.Text = e.Data.Title;
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			loadDataAtDesignTime.Items.Add(new MetroFlowData { Title = DateTime.Now.ToString(), ImageUri = new Uri(@"..\media\images\trex_360width.jpg", UriKind.RelativeOrAbsolute) });

			SetColIndexSliderMax();
		}

		private void Remove_Click(object sender, RoutedEventArgs e)
		{
			if (loadDataAtDesignTime.Items.Count > 0)
				loadDataAtDesignTime.Items.RemoveAt(loadDataAtDesignTime.Items.Count - 1);

			SetColIndexSliderMax();
		}
	}
}