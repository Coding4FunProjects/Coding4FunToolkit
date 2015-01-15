#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml.Controls;
#elif WINDOWS_PHONE
using System.Windows.Controls;
#endif

namespace Coding4Fun.Toolkit.Controls
{
    public class ColorSliderThumb : Control
    {
        public ColorSliderThumb()
		{
            DefaultStyleKey = typeof(ColorSliderThumb);
		}
    }
}
