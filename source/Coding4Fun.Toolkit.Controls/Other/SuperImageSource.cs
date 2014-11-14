#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Media;

#endif

namespace Coding4Fun.Toolkit.Controls
{
    /// <summary>
    /// The SuperImageSource is what's used to provide the SuperImage with a list of sources 
    /// from which to choose the most suitable image based on the application's current scale
    /// </summary>
    public class SuperImageSource : DependencyObject
    {
        #region Dependency Properties
        #region MinScale Property
        public static readonly DependencyProperty MinScaleProperty = DependencyProperty.Register(
            "MinScale",
            typeof (int),
            typeof (SuperImageSource),
            new PropertyMetadata(default(int)));

        /// <summary>
        /// Gets or sets the min scale.
        /// </summary>
        /// <value>
        /// The min scale.
        /// </value>
        public int MinScale
        {
            get { return (int) GetValue(MinScaleProperty); }
            set { SetValue(MinScaleProperty, value); }
        }
        #endregion

        #region MaxScale Property
        public static readonly DependencyProperty MaxScaleProperty = DependencyProperty.Register(
            "MaxScale",
            typeof (int),
            typeof (SuperImageSource),
            new PropertyMetadata(default(int)));

        /// <summary>
        /// Gets or sets the max scale.
        /// </summary>
        /// <value>
        /// The max scale.
        /// </value>
        public int MaxScale
        {
            get { return (int) GetValue(MaxScaleProperty); }
            set { SetValue(MaxScaleProperty, value); }
        }
        #endregion

        #region Source Property
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
			typeof(ImageSource),
            typeof (SuperImageSource),
			new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public ImageSource Source
        {
			get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        #endregion

        #region IsDefault Property
        public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register(
            "IsDefault",
            typeof (bool),
            typeof (SuperImageSource),
            new PropertyMetadata(default(bool)));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsDefault
        {
            get { return (bool) GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }
        #endregion
        #endregion
    }
}