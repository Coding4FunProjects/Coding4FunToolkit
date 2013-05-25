using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls.Common;

// ReSharper disable CheckNamespace
namespace Coding4Fun.Toolkit.Controls
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// The SuperImage allows you to have a number of different things active. These include a placeholder
    /// image to be displayed while the main image is loading, and the option of providing multiple images
    /// and letting the SuperImage choose the most appropriate image based on the application's current scale
    /// </summary>
    [TemplatePart(Name = PrimaryImage, Type = typeof(Image))]
	[TemplatePart(Name = PlaceholderBorder, Type = typeof(Border))]
    public class SuperImage : Control
    {
        #region Constants
        public const string PrimaryImage = "PrimaryImage";

		public const string PlaceholderBorder = "PlaceholderBorder";
        #endregion

        #region Private Properties
        /// <summary>
        /// The _primaryImage
        /// </summary>
        private Image _primaryImage;

        /// <summary>
        /// The _place border
        /// </summary>
        private Border _placeholderBorder;

        private bool _isPrimaryImageLoaded;
        #endregion
        
        #region Dependency Properties
        #region Stretch Property
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch",
            typeof(Stretch),
            typeof(SuperImage),
            new PropertyMetadata(default(Stretch)));

        /// <summary>
        /// Gets or sets the stretch.
        /// </summary>
        /// <value>
        /// The stretch.
        /// </value>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        #endregion

        #region Sources Property
        public static readonly DependencyProperty SourcesProperty = DependencyProperty.Register(
            "Sources",
			typeof(ObservableCollection<SuperImageSource>), 
            typeof (SuperImage),
			new PropertyMetadata(null, OnSourcesChanged));

        /// <summary>
        /// Gets or sets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
		public ObservableCollection<SuperImageSource> Sources
        {
			get { return (ObservableCollection<SuperImageSource>)GetValue(SourcesProperty); }
            set { SetValue(SourcesProperty, value); }
        }
        #endregion

        #region PlaceholderImageSource Property
        public static readonly DependencyProperty PlaceholderSourceProperty = DependencyProperty.Register(
            "PlaceholderImageSource", 
            typeof (ImageSource), 
            typeof (SuperImage), 
            new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// Gets or sets the placeholder source.
        /// </summary>
        /// <value>
        /// The placeholder source.
        /// </value>
        public ImageSource PlaceholderImageSource
        {
            get { return (ImageSource) GetValue(PlaceholderSourceProperty); }
            set { SetValue(PlaceholderSourceProperty, value); }
        }
        #endregion

		#region PlaceholderOpacity Property
		public double PlaceholderOpacity
		{
			get { return (double)GetValue(PlaceholderOpacityProperty); }
			set { SetValue(PlaceholderOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for PlaceholderOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PlaceholderOpacityProperty =
			DependencyProperty.Register("PlaceholderOpacity", typeof(double), typeof(SuperImage), new PropertyMetadata(1.0));
		#endregion

		#region Source Property
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", 
            typeof (ImageSource), 
            typeof (SuperImage),
			new PropertyMetadata(default(ImageSource), OnSourceChanged));

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        #endregion

        #region PlaceholderBackground Property
        public static readonly DependencyProperty PlaceholderBackgroundProperty = DependencyProperty.Register(
            "PlaceholderBackground", 
            typeof (SolidColorBrush), 
            typeof (SuperImage), 
            new PropertyMetadata(default(SolidColorBrush)));

        /// <summary>
        /// Gets or sets the placeholder background.
        /// </summary>
        /// <value>
        /// The placeholder background.
        /// </value>
        public SolidColorBrush PlaceholderBackground
        {
            get { return (SolidColorBrush) GetValue(PlaceholderBackgroundProperty); }
            set { SetValue(PlaceholderBackgroundProperty, value); }
        }
        #endregion

        #region PlaceholderImageStretch Property
        public static readonly DependencyProperty PlaceholderStretchProperty =  DependencyProperty.Register(
            "PlaceholderImageStretch", 
            typeof (Stretch), 
            typeof (SuperImage), 
            new PropertyMetadata(default(Stretch)));

        /// <summary>
        /// Gets or sets the placeholder stretch.
        /// </summary>
        /// <value>
        /// The placeholder stretch.
        /// </value>
        public Stretch PlaceholderImageStretch
        {
            get { return (Stretch) GetValue(PlaceholderStretchProperty); }
            set { SetValue(PlaceholderStretchProperty, value); }
        }
        #endregion
        #endregion

        #region Private Methods
		private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			// If the initial source and new values are null, do nothing
			if (obj == null || e.NewValue == null || e.NewValue == e.OldValue)
				return;

			var si = obj as SuperImage;

			// If the source isn't a SuperImage or the SuperImage's image in the template is null, do nothing
			if (si == null || si._primaryImage == null)
				return;

			si.OnSourcePropertyChanged();
		}

		private void OnSourcePropertyChanged()
	    {
			_isPrimaryImageLoaded = false;
			UpdatePlaceholderImageVisibility();

			var img = Source.GetImageFromUri();

			// Set the SuperImage's image's source
			_primaryImage.Source = img;
	    }

	    private static void OnSourcesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            // If the initial source and new values are null, do nothing
			if (obj == null || e.NewValue == null || e.NewValue == e.OldValue) 
				return;

			var si = obj as SuperImage;

            // If the source isn't a SuperImage or the SuperImage's image in the template is null, do nothing
            if (si == null || si._primaryImage == null) 
				return;

            si.OnSourcesPropertyChanged();
        }

        private void OnSourcesPropertyChanged()
        {
			if (Source != null)
				return;

            // If there are no SuperImageSources, do nothing
            if (!Sources.Any()) 
				return;

            // Get the current application's scale
			var scale = ApplicationSpace.ScaleFactor();

            // Get the first SuperImageSource whose min/max scales best match the current application's scale
	        var items = Sources.Where(x =>
	                                                (scale >= x.MinScale && 0 == x.MaxScale) ||
	                                                (0 == x.MinScale && scale <= x.MaxScale) ||
	                                                (scale >= x.MinScale && scale <= x.MaxScale)
		        ).ToArray();

	        SuperImageSource selectedImageSource; 

	        if (items.Any())
	        {
				selectedImageSource = items.FirstOrDefault(x => x.IsDefault);

				// If there isn't a default SuperImageSource, then do nothing
				if (selectedImageSource == default(SuperImageSource))
					selectedImageSource = items.FirstOrDefault();
	        }
			else
	        // If no best match exists for the current application scale, then we need to check for a default SuperImageSource
	        {
		        selectedImageSource = Sources.FirstOrDefault(x => x.IsDefault);

		        // If there isn't a default SuperImageSource, then do nothing
		        if (selectedImageSource == default(SuperImageSource))
			        selectedImageSource = Sources.FirstOrDefault();

		        // If there isn't a default SuperImageSource, then do nothing
		        if (selectedImageSource == default(SuperImageSource))
			        return;
	        }

	        // Create the bitmap image, this will check whether the SuperImageSource's uri is looking in isolatedstorage
            // if not, it will create the bitmap image from the uri provided
            var img = selectedImageSource.Source.GetImageFromUri();

            // Set the SuperImage's image's source
            _primaryImage.Source = img;
        }

        private void OnPrimaryImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            _isPrimaryImageLoaded = true;
            UpdatePlaceholderImageVisibility();
        }

        private void OnPrimaryImageFailed(object sender, RoutedEventArgs routedEventArgs)
        {
            _isPrimaryImageLoaded = false;
            UpdatePlaceholderImageVisibility();
        }

        private void UpdatePlaceholderImageVisibility()
        {
            // We hide the border not the Image as the border could be being used and as the 
            // PlaceholderImage is within the border, we get a twofer.
            if (_placeholderBorder != null)
            {
                _placeholderBorder.Visibility = _isPrimaryImageLoaded ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        #endregion
        
        public SuperImage()
        {
            DefaultStyleKey = typeof(SuperImage);
	        
			Sources = new ObservableCollection<SuperImageSource>();
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_primaryImage != null)
            {
                _primaryImage.ImageOpened -= OnPrimaryImageOpened;
                _primaryImage.ImageFailed -= OnPrimaryImageFailed;
            }

            // Get template parts
            _primaryImage = GetTemplateChild(PrimaryImage) as Image;
            _placeholderBorder = GetTemplateChild(PlaceholderBorder) as Border;
            
            // Reset whether the front image has loaded
            _isPrimaryImageLoaded = false;

            // Hook up to new elements
            if (_primaryImage != null)
            {
                _primaryImage.ImageOpened += OnPrimaryImageOpened;
                _primaryImage.ImageFailed += OnPrimaryImageFailed;
            }

	        if (Source != null)
		        OnSourcePropertyChanged();
			else
		        OnSourcesPropertyChanged();
        }
    }
}
