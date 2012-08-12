using System.Windows.Media.Animation;

namespace Coding4Fun.Phone.Controls
{
	public struct ImageTileState
	{
		public Storyboard Storyboard { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }

		public bool ForceLargeImageCleanup { get; set; }
	}
}