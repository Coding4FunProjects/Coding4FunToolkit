using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
	public partial class Binding : PhoneApplicationPage
	{
		public Binding()
		{
			InitializeComponent();
			DataContext = this;
		}



		public string TestProp
		{
			get { return (string)GetValue(TestPropProperty); }
			set { SetValue(TestPropProperty, value); }
		}

		// Using a DependencyProperty as the backing store for TestProp.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TestPropProperty =
			DependencyProperty.Register("TestProp", typeof(string), typeof(Binding), new PropertyMetadata(""));
	}
}