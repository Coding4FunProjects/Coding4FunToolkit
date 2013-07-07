using System;
using System.Windows;
using System.Windows.Data;

namespace FileExplorerExperimental.Control.Interop.Converters
{
    public class SelectionModeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SelectionMode mode = (SelectionMode)value;

            // We only need to make sure that the open button is visible
            // for situations where the user needs to select a folder, or 
            // multiple files.
            if (mode == SelectionMode.File)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
