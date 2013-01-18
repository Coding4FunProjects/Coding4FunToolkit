using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Toolkit.Controls
{
	public abstract partial class ToggleButtonBase : IImageSourceButton
	{
		private bool IsContentEmpty(object content)
		{
			return content == null && ImageSource == null;
		}

		private void ApplyingTemplate()
		{
			UpdateImageSource();
		}

        #region dependency properties
		
		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ToggleButtonBase), new PropertyMetadata(Stretch.None, OnUpdate));

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToggleButtonBase),
			new PropertyMetadata(null, OnUpdate));
		#endregion

		#region dp onchange callbacks

		private static void OnUpdate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as ToggleButtonBase;

			if (sender == null)
				return;

			sender.UpdateImageSource();
		}

		#endregion

		private void UpdateImageSource()
		{
			var hostContainer = GetTemplateChild(ButtonBaseConstants.ContentHostName) as Grid;
			var hostContainerDisabled = GetTemplateChild(ButtonBaseConstants.DisabledContentHostName) as Grid;

			var contentEnabled = GetTemplateChild(ButtonBaseConstants.EnabledContentControlName) as FrameworkElement;
			var contentDisabled = GetTemplateChild(ButtonBaseConstants.DisabledContentControlName) as FrameworkElement;

			ButtonBaseHelper.UpdateImageSource(contentEnabled, hostContainer, ImageSource, Stretch);
			ButtonBaseHelper.UpdateImageSource(contentDisabled, hostContainerDisabled, ImageSource, Stretch);
		}
	}
}