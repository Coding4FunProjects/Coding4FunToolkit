using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Phone.Controls
{
    public class RoundButton : Button
    {
        protected ImageBrush OpacityImageBrush;
        private const string OpacityImageBrushName = "OpacityImageBrush";

		public RoundButton()
		{
			DefaultStyleKey = typeof(RoundButton);
		}

		public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            
            OpacityImageBrush = GetTemplateChild(OpacityImageBrushName) as ImageBrush;

			if (OpacityImageBrush != null && ImageSource != null)
            {
				OpacityImageBrush.ImageSource = ImageSource;
            }
        }

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(RoundButton), new PropertyMetadata(null, OnImageSource));

		private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as RoundButton;

			if (sender == null || sender.OpacityImageBrush == null)
				return;

			var brush = e.NewValue as ImageBrush;
			sender.OpacityImageBrush = brush;
		}
    }
}
