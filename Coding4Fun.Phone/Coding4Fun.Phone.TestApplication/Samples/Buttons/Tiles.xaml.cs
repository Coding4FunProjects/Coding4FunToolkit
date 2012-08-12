using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Buttons
{
	public partial class Tiles : PhoneApplicationPage
	{
		public Tiles()
		{
			InitializeComponent();
		}

		private void TileClick(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("You clicked the tile!");
		}
	}
}