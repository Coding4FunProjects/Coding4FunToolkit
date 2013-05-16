using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable CheckNamespace
namespace Coding4Fun.Toolkit.Controls
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// The SuperImage allows you to have a number of different things active. These include a placeholder
    /// image to be displayed while the main image is loading, and the option of providing multiple images
    /// and letting the SuperImage choose the most appropriate image based on the application's current scale
    /// </summary>
    [TemplatePart(Name = MainImage, Type = typeof(Image))]
    [TemplatePart(Name = PlaceBorder, Type = typeof(Border))]
    public class SuperImage : Control
    {
        #region Constants
        public const string MainImage = "MainImage";

        public const string PlaceBorder = "PlaceBorder";
        #endregion

        #region Private Properties
        /// <summary>
        /// The _image
        /// </summary>
        private Image _image;

        /// <summary>
        /// The _place border
        /// </summary>
        private Border _placeBorder;

        private bool _frontImageLoaded;
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
            typeof(List<SuperImageSource>), 
            typeof (SuperImage), 
            new PropertyMetadata(default(List<SuperImageSource>), OnSourcesChanged));

        /// <summary>
        /// Gets or sets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        public List<SuperImageSource> Sources
        {
            get { return (List<SuperImageSource>)GetValue(SourcesProperty); }
            set { SetValue(SourcesProperty, value); }
        }
        #endregion

        #region PlaceholderSource Property
        public static readonly DependencyProperty PlaceholderSourceProperty = DependencyProperty.Register(
            "PlaceholderSource", 
            typeof (ImageSource), 
            typeof (SuperImage), 
            new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// Gets or sets the placeholder source.
        /// </summary>
        /// <value>
        /// The placeholder source.
        /// </value>
        public ImageSource PlaceholderSource
        {
            get { return (ImageSource) GetValue(PlaceholderSourceProperty); }
            set { SetValue(PlaceholderSourceProperty, value); }
        }
        #endregion

        #region Source Property
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", 
            typeof (ImageSource), 
            typeof (SuperImage), 
            new PropertyMetadata(default(ImageSource)));

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

        #region PlaceholderStretch Property
        public static readonly DependencyProperty PlaceholderStretchProperty =  DependencyProperty.Register(
            "PlaceholderStretch", 
            typeof (Stretch), 
            typeof (SuperImage), 
            new PropertyMetadata(default(Stretch)));

        /// <summary>
        /// Gets or sets the placeholder stretch.
        /// </summary>
        /// <value>
        /// The placeholder stretch.
        /// </value>
        public Stretch PlaceholderStretch
        {
            get { return (Stretch) GetValue(PlaceholderStretchProperty); }
            set { SetValue(PlaceholderStretchProperty, value); }
        }
        #endregion
        #endregion

        #region Private Methods
        private static void OnSourcesChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            // If the initial source and new values are null, do nothing
            if (source == null || e.NewValue == null) return;

            var si = source as SuperImage;

            // If the source isn't a SuperImage or the SuperImage's image in the template is null, do nothing
            if (si == null || si._image == null) return;

            var sources = (List<SuperImageSource>) e.NewValue;

            si.OnSourcesPropertyChanged(sources);
        }

        private void OnSourcesPropertyChanged(List<SuperImageSource> sources)
        {
            // As we are loading the images, we need to display the placeholder image if one is set
            _frontImageLoaded = false;
            UpdateBackImageVisibility();

            // If there are no SuperImageSources, do nothing
            if (!sources.Any()) return;

            // Get the current application's scale
            var scale = SuperImageExtensions.GetCurrentScale();

            // Get the first SuperImageSource whose min/max scales best match the current application's scale
            var selectedImageSource = sources.FirstOrDefault(x => scale >= x.MinScale && scale <= x.MaxScale);

            // If no best match exists for the current application scale, then we need to check for a default SuperImageSource
            if (selectedImageSource == default(SuperImageSource))
            {
                selectedImageSource = sources.FirstOrDefault(x => x.IsDefault);

                // If there isn't a default SuperImageSource, then do nothing
                if (selectedImageSource == default(SuperImageSource)) return;
            }

            // Create the bitmap image, this will check whether the SuperImageSource's uri is looking in isolatedstorage
            // if not, it will create the bitmap image from the uri provided
            var img = selectedImageSource.Source.GetImageFromUri();

            // Set the SuperImage's image's source
            _image.Source = img;

            // Hide the placeholder image if it's set
            _frontImageLoaded = true;
            UpdateBackImageVisibility();
        }

        private void OnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            _frontImageLoaded = true;
            UpdateBackImageVisibility();
        }

        private void OnImageFailed(object sender, RoutedEventArgs routedEventArgs)
        {
            _frontImageLoaded = false;
            UpdateBackImageVisibility();
        }

        private void UpdateBackImageVisibility()
        {
            // We hide the border not the Image as the border could be being used and as the 
            // PlaceholderImage is within the border, we get a twofer.
            if (_placeBorder != null)
            {
                _placeBorder.Visibility = _frontImageLoaded ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        #endregion
        
        public SuperImage()
        {
            DefaultStyleKey = typeof(SuperImage);
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_image != null)
            {
                _image.ImageOpened -= OnImageOpened;
                _image.ImageFailed -= OnImageFailed;
            }

            // Get template parts
            _image = GetTemplateChild(MainImage) as Image;
            _placeBorder = GetTemplateChild(PlaceBorder) as Border;
            
            // Reset whether the front image has loaded
            _frontImageLoaded = false;

            // Hook up to new elements
            if (_image != null)
            {
                _image.ImageOpened += OnImageOpened;
                _image.ImageFailed += OnImageFailed;
            }

            UpdateBackImageVisibility();
        }
    }
}
