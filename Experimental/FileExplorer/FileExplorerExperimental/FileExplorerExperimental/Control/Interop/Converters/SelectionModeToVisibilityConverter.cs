using System;
using System.Windows;
using System.Windows.Data;

namespace FileExplorerExperimental.Control.Interop.Converters
{
    public class SelectionModeToVisibilityConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ParentSelectionModeProperty = DependencyProperty.Register("ParentSelectionMode", typeof(SelectionMode), typeof(SelectionModeToVisibilityConverter),
            new PropertyMetadata(SelectionMode.File));

        public SelectionMode ParentSelectionMode
        {
            get
            {
                return (SelectionMode)GetValue(ParentSelectionModeProperty);
            }
            set
            {
                SetValue(ParentSelectionModeProperty, value);
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.ToString() == "selector")
            {
                FileExplorerItem item = (FileExplorerItem)value;

                if (ParentSelectionMode == SelectionMode.MultipleFiles)
                {
                    if (item.IsFolder)
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
                else if (ParentSelectionMode == SelectionMode.MultipleFolders)
                {
                    if (item.IsFolder)
                        return Visibility.Visible;
                    else
                        return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else if (parameter.ToString() == "opener")
            {
                if (ParentSelectionMode != SelectionMode.File)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
