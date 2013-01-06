#if WINDOWS_STORE

using Windows.UI.Xaml;

#elif WINDOWS_PHONE

using System.Windows;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public class Tile : ButtonBase
    {
		public Tile()
		{
			DefaultStyleKey = typeof (Tile);
		}

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Tile), new PropertyMetadata(TextWrapping.NoWrap));
    }
}
