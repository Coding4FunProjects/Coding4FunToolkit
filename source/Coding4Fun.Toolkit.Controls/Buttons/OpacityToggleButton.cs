using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Coding4Fun.Toolkit.Controls
{
	public class OpacityToggleButton : ToggleButtonBase
	{
		private const string ButtonForegroundName = "ButtonForeground";

		public OpacityToggleButton()
		{
			DefaultStyleKey = typeof(OpacityToggleButton);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var buttonForeground = GetTemplateChild(ButtonForegroundName) as FrameworkElement;

			if(buttonForeground != null)
			{
				var checkedState = GetTemplateChild("Checked") as VisualState;
				var uncheckedState = GetTemplateChild("Unchecked") as VisualState;

				if (checkedState != null)
				{
					checkedState.Storyboard = new Storyboard();
					CreateDoubleAnimations(checkedState.Storyboard, buttonForeground, "Opacity", CheckedOpacity);
				}

				if (uncheckedState != null)
				{
					uncheckedState.Storyboard = new Storyboard();
					CreateDoubleAnimations(uncheckedState.Storyboard, buttonForeground, "Opacity", UncheckedOpacity);
				}
			}
		}

		#region dependency properties

		public TimeSpan AnimationDuration
		{
			get { return (TimeSpan)GetValue(AnimationDurationProperty); }
			set { SetValue(AnimationDurationProperty, value); }
		}

		// Using a DependencyProperty as the backing store for AnimationDuration.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AnimationDurationProperty =
			DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(OpacityToggleButton), new PropertyMetadata(TimeSpan.FromMilliseconds(100)));
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

		#region helpers
		private void CreateDoubleAnimations(Storyboard sb, DependencyObject target, string propertyPath, double toValue)
		{
			var doubleAni = new DoubleAnimation
			{
				To = toValue,
				Duration = AnimationDuration
			};

			Storyboard.SetTarget(doubleAni, target);
			Storyboard.SetTargetProperty(doubleAni, new PropertyPath(propertyPath));

			sb.Children.Add(doubleAni);
		}
		#endregion
	}
}
