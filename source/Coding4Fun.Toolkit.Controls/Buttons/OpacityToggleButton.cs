using System;
using System.Windows;

namespace Coding4Fun.Toolkit.Controls
{
	public class OpacityToggleButton : ToggleButtonBase
	{
		public OpacityToggleButton()
		{
			DefaultStyleKey = typeof(OpacityToggleButton);
		}

		#region dependency properties

		public Duration AnimationDuration
		{
			get { return (Duration)GetValue(AnimationDurationProperty); }
			set { SetValue(AnimationDurationProperty, value); }
		}

		// Using a DependencyProperty as the backing store for AnimationDuration.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AnimationDurationProperty =
			DependencyProperty.Register("AnimationDuration", typeof(Duration), typeof(OpacityToggleButton), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(100))));

		public double UncheckedOpacity
		{
			get { return (double)GetValue(UncheckedOpacityProperty); }
			set { SetValue(UncheckedOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for UncheckedOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty UncheckedOpacityProperty =
			DependencyProperty.Register("UncheckedOpacity", typeof(double), typeof(OpacityToggleButton), new PropertyMetadata(.5));

		public double CheckedOpacity
		{
			get { return (double)GetValue(CheckedOpacityProperty); }
			set { SetValue(CheckedOpacityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CheckedOpacity.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CheckedOpacityProperty =
			DependencyProperty.Register("CheckedOpacity", typeof(double), typeof(OpacityToggleButton), new PropertyMetadata(1.0));

		#endregion
	}
}
