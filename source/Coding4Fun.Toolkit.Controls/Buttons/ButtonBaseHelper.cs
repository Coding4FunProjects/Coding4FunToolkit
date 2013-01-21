using System.Linq;

#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Shapes;

#endif

using Coding4Fun.Toolkit.Controls.Converters;

namespace Coding4Fun.Toolkit.Controls
{
	internal static partial class ButtonBaseHelper
	{
		public static void ApplyTitleOffset(ContentControl contentTitle)
		{
			if (contentTitle == null) 
				return;

			var bottom = -(contentTitle.FontSize / 8.0);
			var top = -(contentTitle.FontSize / 2.0) - bottom;

			contentTitle.Margin = new Thickness(0, top, 0, bottom);
		}


		public static void ApplyForegroundToFillBinding(ContentControl control)
		{
			if (control == null)
				return;

			var element = control.Content as FrameworkElement;

			if (element == null) 
				return;

			if (element.IsTypeOf(typeof(Shape)))
			{
				var shape = element as Shape;

				ResetVerifyAndApplyForegroundToFillBinding(control, shape);
			}
			else
			{
				var children = element.GetLogicalChildrenByType<Shape>(false);

				foreach (var child in children)
				{
					ResetVerifyAndApplyForegroundToFillBinding(control, child);
				}
			}
		}

		internal static void ResetVerifyAndApplyForegroundToFillBinding(FrameworkElement source, Shape target)
		{
			if (target != null && (target.Fill == null || target.GetBindingExpression(Shape.FillProperty) != null))
			{
				target.Fill = null;
				ApplyBinding(source, target, "Foreground", Shape.FillProperty);
			}
		}

		internal static void ApplyForegroundToFillBinding(FrameworkElement source, FrameworkElement target)
		{
			ApplyBinding(source, target, "Foreground", Shape.FillProperty);
		}


		public static void ApplyBinding(FrameworkElement source, FrameworkElement target, string propertyPath, DependencyProperty property, IValueConverter converter = null, object converterParameter = null)
		{
			if (source == null || target == null)
				return;

#if WINDOWS_STORE
			var binding = new Windows.UI.Xaml.Data.Binding();
#elif WINDOWS_PHONE
			var binding = new System.Windows.Data.Binding();;
#endif

			binding.Source = source;
			binding.Path = new PropertyPath(propertyPath);

			binding.Converter = converter;
			binding.ConverterParameter = converterParameter;

			target.SetBinding(property, binding);
		}


		public static Path CreateXamlCheck(FrameworkElement control)
		{
			var check = XamlReader.Load(@"<Path 
					xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
					Stretch=""Uniform"" 
					Data=""F1M227.2217,408.499L226.4427,407.651C226.2357,407.427,226.2467,407.075,226.4737,406.865L228.7357,404.764C228.8387,404.668,228.9737,404.615,229.1147,404.615C229.2707,404.615,229.4147,404.679,229.5207,404.792L235.7317,411.479L246.4147,397.734C246.5207,397.601,246.6827,397.522,246.8547,397.522C246.9797,397.522,247.0987,397.563,247.1967,397.639L249.6357,399.533C249.7507,399.624,249.8257,399.756,249.8447,399.906C249.8627,400.052,249.8237,400.198,249.7357,400.313L236.0087,417.963z""
					/>") as Path;

			if (check != null)
			{
				ApplyBinding(control, check, "ButtonHeight", FrameworkElement.HeightProperty, new NumberMultiplierConverter(), .25);
			}

			return check;
		}
	}
}
