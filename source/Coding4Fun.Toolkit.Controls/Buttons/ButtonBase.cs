#if WINDOWS_STORE

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#elif WINDOWS_PHONE

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

		public object Title
		{
			get { return GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(object), typeof(ButtonBase), new PropertyMetadata(new object()));
#endregion
	}
}
