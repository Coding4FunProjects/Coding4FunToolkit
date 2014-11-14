#if WINDOWS_STORE || WINDOWS_PHONE_APP

using Windows.UI.Xaml.Media.Animation;

#elif WINDOWS_PHONE

using System.Windows.Media.Animation;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public struct ImageTileState
	{
		public Storyboard Storyboard { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }

		public bool ForceLargeImageCleanup { get; set; }
	}
}