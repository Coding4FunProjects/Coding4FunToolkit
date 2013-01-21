#if WINDOWS_STORE

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public class TileNotification : ContentControl
    {
        public TileNotification()
		{
            DefaultStyleKey = typeof(TileNotification);
		}
    }
}
