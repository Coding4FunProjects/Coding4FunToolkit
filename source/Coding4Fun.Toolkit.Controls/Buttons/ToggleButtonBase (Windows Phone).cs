using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Toolkit.Controls
{
	public abstract partial class ToggleButtonBase : IImageSourceButton
	{
		protected ImageBrush OpacityImageBrush;
		protected ImageBrush DisabledOpacityImageBrush;

		private void ApplyingTemplate()
		{
			OpacityImageBrush = GetTemplateChild(ButtonBaseConstants.OpacityImageBrushName) as ImageBrush;
			DisabledOpacityImageBrush = GetTemplateChild(ButtonBaseConstants.DisabledOpacityImageBrushName) as ImageBrush;
			
			ButtonBaseHelper.ApplyOpacityImageBrush(this, OpacityImageBrush, ImageSourceProperty);
			ButtonBaseHelper.ApplyOpacityImageBrush(this, DisabledOpacityImageBrush, ImageSourceProperty);

			ButtonBaseHelper.ApplyStretch(OpacityImageBrush, Stretch);
			ButtonBaseHelper.ApplyStretch(DisabledOpacityImageBrush, Stretch);
        }

        #region dependency properties
		
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
            new PropertyMetadata(null, OnImageSource));
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
