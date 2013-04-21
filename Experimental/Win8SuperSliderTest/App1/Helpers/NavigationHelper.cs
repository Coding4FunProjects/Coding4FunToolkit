using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace testSliderWinPhone8
{
	public static class NavigationHelper
	{
		public static void NavigateTo(string page)
		{
			if (!page.StartsWith("/"))
				page = "/" + page;

			RootVisual.Navigate(new Uri(page, UriKind.RelativeOrAbsolute));
		}

		static internal Frame RootVisual
		{
			get { return Application.Current.RootVisual as Frame; }
		}
	}
}
