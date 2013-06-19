using System;
using System.Windows.Data;

namespace FileExplorerExperimental.Control.Interop.Converters
{
    public class ExplorerTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isFolder = (bool)value;

            if (isFolder)
                return new Uri("/Assets/Icons/folder.png", UriKind.Relative);
            else
                return new Uri("/Assets/Icons/file.png", UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
