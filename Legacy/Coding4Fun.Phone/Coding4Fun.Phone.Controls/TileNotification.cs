using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Coding4Fun.Phone.Controls
{
    [ContentProperty("Content")]
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
