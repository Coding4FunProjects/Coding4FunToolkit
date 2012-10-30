using System.Windows;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
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