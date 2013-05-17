using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Coding4Fun.Toolkit.Controls
{
	public class TestControl : Control
	{
		public TestControl()
        {
			DefaultStyleKey = typeof(TestControl);
        }

		#region Stretch Property
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
			"Stretch",
			typeof(Stretch),
			typeof(TestControl),
			new PropertyMetadata(default(Stretch)));

		/// <summary>
		/// Gets or sets the stretch.
		/// </summary>
		/// <value>
		/// The stretch.
		/// </value>
		public Stretch Stretch
		{
			get { return (Stretch)GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}
		#endregion

		#region Source Property
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(ImageSource),
			typeof(TestControl),
			new PropertyMetadata(default(ImageSource)));

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>
		/// The source.
		/// </value>
		public ImageSource Source
		{
			get { return (ImageSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}
		#endregion

		public ObservableCollection<Image> MyProperty
		{
			get { return (ObservableCollection<Image>)GetValue(MyPropertyProperty); }
			set { SetValue(MyPropertyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MyPropertyProperty =
			DependencyProperty.Register("MyProperty", typeof(ObservableCollection<Image>), typeof(TestControl), new PropertyMetadata(new ObservableCollection<Image>()));

	}
}
