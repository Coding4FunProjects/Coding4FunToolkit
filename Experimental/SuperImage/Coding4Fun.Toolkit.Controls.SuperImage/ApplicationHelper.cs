using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
