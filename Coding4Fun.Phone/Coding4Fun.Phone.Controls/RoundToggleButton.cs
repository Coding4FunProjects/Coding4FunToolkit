using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Phone.Controls
{
    public class RoundToggleButton : CheckBox 
	{
		protected ImageBrush OpacityImageBrush;
        private const string OpacityImageBrushName = "OpacityImageBrush";

        protected ContentControl ContentBody;
        private const string ContentBodyName = "ContentBody";

        public RoundToggleButton()
		{
			DefaultStyleKey = typeof(RoundToggleButton);
        }

		public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OpacityImageBrush = GetTemplateChild(OpacityImageBrushName) as ImageBrush;
            ContentBody = GetTemplateChild(ContentBodyName) as ContentControl;

            if (ImageSource == null)
                ImageSource = new BitmapImage(new Uri("/Coding4Fun.Phone.Controls;component/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute));
            else
                SetImageBrush(ImageSource);

			SetStretch(Stretch);

            if (ContentBody != null)
            {
                var bottom = -(ContentBody.FontSize / 8.0);
                var top = -(ContentBody.FontSize / 2.0) - bottom;

                ContentBody.Margin = new Thickness(0, top, 0, bottom);
            }
        }

        #region dependency properties
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RoundToggleButton), new PropertyMetadata(Orientation.Vertical));

		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(RoundToggleButton), new PropertyMetadata(Stretch.Fill, OnStretch));

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(RoundToggleButton),
            new PropertyMetadata(
                //new BitmapImage(new Uri("/Coding4Fun.Phone.Controls;component/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute)),
                OnImageSource));

		public double ButtonWidth
		{
			get { return (double)GetValue(ButtonWidthProperty); }
			set { SetValue(ButtonWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonWidthProperty =
			DependencyProperty.Register("ButtonWidth", typeof(double), typeof(RoundToggleButton), new PropertyMetadata(72d));

		public double ButtonHeight
		{
			get { return (double)GetValue(ButtonHeightProperty); }
			set { SetValue(ButtonHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonHeightProperty =
			DependencyProperty.Register("ButtonHeight", typeof(double), typeof(RoundToggleButton), new PropertyMetadata(72d));
        #endregion

        #region dp onchange callbacks
        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as RoundToggleButton;

            if (sender == null || e.NewValue == e.OldValue)
                return;

            sender.SetImageBrush(e.NewValue as ImageSource);
        }

        private void SetImageBrush(ImageSource brush)
        {
            if (OpacityImageBrush == null)
                return;

            OpacityImageBrush.ImageSource = brush;
        }

		private static void OnStretch(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as RoundToggleButton;

			if (sender == null || e.NewValue == e.OldValue)
				return;

			sender.SetStretch((Stretch)e.NewValue);
		}

		private void SetStretch(Stretch stretch)
		{
			if (OpacityImageBrush == null)
				return;

			OpacityImageBrush.Stretch = stretch;
		}
        #endregion
	}
}
