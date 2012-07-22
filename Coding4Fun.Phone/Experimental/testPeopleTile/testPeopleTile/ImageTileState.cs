using System.Windows.Media.Animation;

namespace testPeopleTile
{
	public struct ImageTileState
	{
		public Storyboard Storyboard { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }
	}
}
