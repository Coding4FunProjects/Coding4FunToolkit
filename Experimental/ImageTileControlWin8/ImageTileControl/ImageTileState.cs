
using Windows.UI.Xaml.Media.Animation;
namespace ImageTileControl
{
	public struct ImageTileState
	{
		public Storyboard Storyboard { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }

		public bool ForceLargeImageCleanup { get; set; }
	}
}