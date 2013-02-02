using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Phone.TestApplication.Test
{
	public class InheritedTextBox : TextBox
	{
		public InheritedTextBox()
		{
			Background = new SolidColorBrush(Colors.Red);
		}
	}
}
