using System.Windows;
using System.Windows.Controls;

using Coding4Fun.Phone.Controls.Converters;

namespace Coding4Fun.Phone.Controls
{
    /// <summary>
    /// this is a slightly modified version of Jeff Wilcox
    /// http://www.jeff.wilcox.name/performanceprogressbar/
    /// Performance Progress Bar
	/// 
	/// You only need to set Visibility, 
	/// IsIndeterminate will automatically turn off if Visibilty.Collapsed.
    /// </summary>
    public class PerformanceProgressBar : ProgressBar
    {
        public PerformanceProgressBar()
        {
            DefaultStyleKey = typeof (PerformanceProgressBar);
            Unloaded += PerformanceProgressBar_Unloaded;
			Loaded += PerformanceProgressBar_Loaded;
        }

		void PerformanceProgressBar_Loaded(object sender, RoutedEventArgs e)
		{
			var visToBool = new VisibilityToBooleanConverter();
			IsIndeterminate = (bool) visToBool.Convert(Visibility, null,null, null);

			var binding = new System.Windows.Data.Binding
			{
				Source = this,
				Path = new PropertyPath("Visibility"),
				Converter = visToBool
			};

			SetBinding(IsIndeterminateProperty, binding);
		}

        void PerformanceProgressBar_Unloaded(object sender, RoutedEventArgs e)
        {
            IsIndeterminate = false;
        }
    }
}
