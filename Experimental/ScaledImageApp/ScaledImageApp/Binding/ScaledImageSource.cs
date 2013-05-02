using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Toolkit.Controls.Binding
{
    public class ScaledImageSource
    {
        public static object GetSource(DependencyObject obj)
        {
            return obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, object value)
        {
            obj.SetValue(SourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source",
                                                typeof(object),
                                                typeof(ScaledImageSource),
                                                new PropertyMetadata(OnSourceChanged));

        private static void OnSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Image) || e.NewValue == null) return;

            // Gets the current Phone resolution
            var resolution = ResolutionHelper.CurrentResolution;
            
            // Checks whether what has been passed in is a string or a Uri
            Uri url;
            if (e.NewValue is string)
            {
                url = new Uri(e.NewValue.ToString(), UriKind.RelativeOrAbsolute);
            }
            else if (e.NewValue is Uri)
            {
                url = (Uri)e.NewValue;
            }
            else
            {
                return;
            }

            // Sets the text to be added to the Uri
            var imageExtension = ".screen-wvga";
            switch (resolution)
            {
                case Resolutions.HD720p:
                    imageExtension = ".screen-720p";
                    break;
                case Resolutions.WXGA:
                    imageExtension = ".screen-wxga";
                    break;
            }

            // Adds the text to the uri
            var urlString = url.ToString();
            urlString = urlString.Insert(urlString.LastIndexOf(".", StringComparison.Ordinal), imageExtension);

            // Sets the image's source to be the changed Uri
            var image = (Image)sender;
            image.Source = new BitmapImage(new Uri(urlString, UriKind.Relative));
        }

        private static class ResolutionHelper
        {
            private static bool IsWvga
            {
                get
                {
                    return Application.Current.Host.Content.ScaleFactor == 100;
                }
            }

            private static bool IsWxga
            {
                get
                {
                    return Application.Current.Host.Content.ScaleFactor == 160;
                }
            }

            private static bool Is720p
            {
                get
                {
                    return Application.Current.Host.Content.ScaleFactor == 150;
                }
            }

            public static Resolutions CurrentResolution
            {
                get
                {
                    if (IsWvga) return Resolutions.WVGA;
                    if (IsWxga) return Resolutions.WXGA;
                    if (Is720p) return Resolutions.HD720p;
                    throw new InvalidOperationException("Unknown resolution");
                }
            }
        }
        public enum Resolutions { WVGA, WXGA, HD720p };
    }


}
