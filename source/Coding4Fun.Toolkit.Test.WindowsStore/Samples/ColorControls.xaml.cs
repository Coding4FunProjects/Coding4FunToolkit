using System;
using System.Collections.Generic;

using Coding4Fun.Toolkit.Test.WindowsStore.Common;
using Coding4Fun.Toolkit.Test.WindowsStore.Samples.Color;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ColorControls
	{
		public ColorControls()
		{
			InitializeComponent();
		}

		private void ColorHex(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(ColorHexPicker));
		}
	}
}
