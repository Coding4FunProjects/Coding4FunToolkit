using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Coding4Fun.Toolkit.Controls
{
	public sealed partial class ColorHexagonPicker
	{
		void ColorHexagonPickerLoaded(object sender, RoutedEventArgs e)
		{
			ColorHexagonPickerLoaded();
		}

		void SetRectangleEvents(UIElement rect)
		{
			// handles single click
			rect.Tapped += ExecuteRectangleSelect;

			// handles person moving around
			rect.PointerMoved += ExecuteRectangleHighlight;
		}

		void ExecuteRectangleSelect(object sender, TappedRoutedEventArgs e)
		{
			ExecuteRectangleSelect(sender);
		}

		void ExecuteRectangleHighlight(object sender, PointerRoutedEventArgs e)
		{
			ExecuteRectangleHighlight(sender, e.Pointer.IsInContact);
		}
	}
}
