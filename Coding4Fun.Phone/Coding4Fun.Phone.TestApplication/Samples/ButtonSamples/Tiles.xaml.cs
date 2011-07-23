using System.Windows;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.ButtonSamples
{
    public partial class Tiles : PhoneApplicationPage
    {
        public Tiles()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            trexStoryboard.Begin();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You clicked the tile!");
        }

    }
}