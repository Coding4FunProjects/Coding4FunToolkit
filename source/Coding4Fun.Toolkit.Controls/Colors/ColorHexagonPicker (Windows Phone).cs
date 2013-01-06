using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Coding4Fun.Toolkit.Controls
{
    public partial class ColorHexagonPicker 
    {
		void ColorHexagonPickerLoaded(object sender, RoutedEventArgs e)
		{
			ColorHexagonPickerLoaded();
		}

		void SetRectangleEvents(UIElement rect)
		{
			// handles single click
			rect.Tap += ExecuteRectangleSelect;

			// handles person moving around
			rect.MouseEnter += ExecuteRectangleHighlight;
		}

		void ExecuteRectangleSelect(object sender, GestureEventArgs e)
		{
			ExecuteRectangleSelect(sender);
		}

		void ExecuteRectangleHighlight(object sender, MouseEventArgs e)
		{
			ExecuteRectangleHighlight(sender, e.StylusDevice.GetStylusPoints(sender as UIElement).Any());
		}
    }
}
