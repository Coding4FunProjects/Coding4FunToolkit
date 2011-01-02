
using System.Windows;
using System.Windows.Controls;
using Coding4Fun.Phone.Controls.Converters;

namespace Coding4Fun.Phone.Controls
{
    /// <summary>
    /// this is a slightly modified version of Jeff Wilcox
    /// http://www.jeff.wilcox.name/performanceprogressbar/
    /// Performance Progress Bar
    /// </summary>
    public class PerformanceProgressBar : ProgressBar
    {
        public PerformanceProgressBar()
        {
            DefaultStyleKey = typeof (PerformanceProgressBar);
            IsIndeterminate = true;
            Unloaded += PerformanceProgressBar_Unloaded;

            var binding = new System.Windows.Data.Binding
                              {
                                  Source = this,
                                  Path = new PropertyPath("Visibility"),
                                  Converter = new VisibilityToBooleanConverter()
                              };

            SetBinding(IsIndeterminateProperty, binding);
        }

        void PerformanceProgressBar_Unloaded(object sender, RoutedEventArgs e)
        {
            IsIndeterminate = false;
        }
    }
}
