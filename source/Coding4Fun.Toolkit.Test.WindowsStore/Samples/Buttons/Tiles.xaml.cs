using System;

using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class Tiles
	{
		public Tiles()
		{
			InitializeComponent();
		}

		private async void TileClick(object sender, RoutedEventArgs e)
		{
			var msg = new MessageDialog("You clicked the tile!");
			await msg.ShowAsync();
		}
	}
}
