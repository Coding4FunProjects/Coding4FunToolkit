#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public abstract class ButtonBase : Button, IButtonBase
	{
#if WINDOWS_STORE
		protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
		{
			base.OnApplyTemplate();
		}

#region dependency properties

		public object Label
		{
			get { return GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register("Label", typeof (object), typeof (ButtonBase), new PropertyMetadata(string.Empty));

#endregion
	}
}