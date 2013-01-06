using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Toolkit.Controls
{
	public partial class RoundButton : IImageSourceButton
    {
        protected ImageBrush OpacityImageBrush;

		private void ApplyingTemplate()
        {
            base.OnApplyTemplate();

			OpacityImageBrush = GetTemplateChild(ButtonBaseConstants.OpacityImageBrushName) as ImageBrush;
			var contentBody = GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl;

			ButtonBaseHelper.ApplyTemplate(this, OpacityImageBrush, contentBody, Stretch, ImageSourceProperty);
        }
        #region dependency properties

		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(RoundButton), new PropertyMetadata(Stretch.Fill, OnStretch));

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(RoundButton), 
            new PropertyMetadata(null, OnImageSource));
		#endregion

        #region dp onchange callbacks
		private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as RoundButton;

			if (sender == null)
				return;

			ButtonBaseHelper.OnImageChange(e, sender.OpacityImageBrush);
		}

		private static void OnStretch(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as RoundButton;

			if (sender == null)
				return;

			ButtonBaseHelper.OnStretch(e, sender.OpacityImageBrush);
		}
        #endregion
    }
}