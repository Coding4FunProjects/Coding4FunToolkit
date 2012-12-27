using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Toolkit.Controls
{
	public abstract class ToggleButtonBase : CheckBox, IButtonBase
	{
		protected ImageBrush OpacityImageBrush;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			OpacityImageBrush = GetTemplateChild(ButtonBaseConstants.OpacityImageBrushName) as ImageBrush;
			var contentBody = GetTemplateChild(ButtonBaseConstants.ContentBodyName) as ContentControl;

			ButtonBaseHelper.ApplyTemplate(this, OpacityImageBrush, contentBody, Stretch, ImageSourceProperty);
        }

        #region dependency properties

		public Brush SelectionBrush
		{
			get { return (Brush)GetValue(SelectionBrushProperty); }
			set { SetValue(SelectionBrushProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectionBrush.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectionBrushProperty =
			DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(ToggleButtonBase), new PropertyMetadata(new SolidColorBrush()));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ToggleButtonBase), new PropertyMetadata(Orientation.Vertical));

		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ToggleButtonBase), new PropertyMetadata(Stretch.Fill, OnStretch));

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToggleButtonBase),
            new PropertyMetadata(OnImageSource));

		public double ButtonWidth
		{
			get { return (double)GetValue(ButtonWidthProperty); }
			set { SetValue(ButtonWidthProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonWidthProperty =
			DependencyProperty.Register("ButtonWidth", typeof(double), typeof(ToggleButtonBase), new PropertyMetadata(72d));

		public double ButtonHeight
		{
			get { return (double)GetValue(ButtonHeightProperty); }
			set { SetValue(ButtonHeightProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ButtonHeightProperty =
			DependencyProperty.Register("ButtonHeight", typeof(double), typeof(ToggleButtonBase), new PropertyMetadata(72d));
        #endregion

        #region dp onchange callbacks
        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = o as ToggleButtonBase;

			if (sender == null)
				return;

			ButtonBaseHelper.OnImageChange(e, sender.OpacityImageBrush);
        }

		private static void OnStretch(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as ToggleButtonBase;

			if (sender == null)
				return;

			ButtonBaseHelper.OnStretch(e, sender.OpacityImageBrush);
		}
        #endregion
	}
}
