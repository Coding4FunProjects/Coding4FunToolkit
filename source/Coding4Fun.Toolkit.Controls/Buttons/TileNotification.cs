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
#if !WINDOWS_STORE
    [ContentProperty("Content")]
#endif
	public class TileNotification : Control
    {
        public TileNotification()
		{
            DefaultStyleKey = typeof(TileNotification);
		}

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(TileNotification), new PropertyMetadata(null));
    }
}
