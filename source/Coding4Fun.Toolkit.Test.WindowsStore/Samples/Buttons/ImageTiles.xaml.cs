using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ImageTiles
	{
		public ImageTiles()
		{
			InitializeComponent();

			SetItemSource(15);
		}

		private void SetItemSource(int amount)
		{
			var items = new List<Uri>();

			for (int i = 0; i <= amount; i++)
			{
				items.Add(new Uri(String.Format("ms-appx:/Media/images/{0}.jpg", i)));
			}

			fadeTile.ItemsSource = items;
		}
	}
}
