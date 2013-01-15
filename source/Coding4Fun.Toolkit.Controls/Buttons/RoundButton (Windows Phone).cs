using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Coding4Fun.Toolkit.Controls
{
	public partial class RoundButton : IImageSourceButton
    {
		private Grid _hostContainer;
		private FrameworkElement _contentBody;

		private bool IsContentEmpty(object content)
		{
			return content == null && ImageSource == null;
		}

		private void ApplyingTemplate()
		{
			_hostContainer = GetTemplateChild(ButtonBaseConstants.ContentHostName) as Grid;
			_contentBody = GetTemplateChild(ButtonBaseConstants.ContentBodyName) as FrameworkElement;

			ButtonBaseHelper.UpdateImageSource(_contentBody, _hostContainer, ImageSource, Stretch);
		}
    
        #region dependency properties

		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(RoundButton), new PropertyMetadata(Stretch.None, OnUpdate));

		public ImageSource ImageSource
        {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

		// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(RoundButton),
			new PropertyMetadata(null, OnUpdate));
		#endregion

        #region dp onchange callbacks
		private static void OnUpdate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sender = o as RoundButton;

			if (sender == null)
				return;

			sender.UpdateImageSource();
		}
        #endregion	
	
		private void UpdateImageSource()
		{
			ButtonBaseHelper.UpdateImageSource(_contentBody, _hostContainer, ImageSource, Stretch);
		}
    }
}