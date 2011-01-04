using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls
{
    public class RoundButton : Button
    {
        public RoundButton()
        {
            DefaultStyleKey = typeof(RoundButton);
            DataContext = this;
        }

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(RoundButton), new PropertyMetadata(null));
    }
}
