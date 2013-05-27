using System.Windows;

namespace Coding4Fun.Toolkit.Controls.Common
{
	public static class ApplicationSpace
	{
		public static int ScaleFactor()
		{
			return Application.Current.Host.Content.ScaleFactor;
		}
	}
}
