using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Common
{

	public class BaseHeader : Control
	{
		public BaseHeader()
		{
			DefaultStyleKey = typeof (BaseHeader);
		}

		public string Title
		{
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof (string), typeof (BaseHeader), new PropertyMetadata(""));

		public Visibility ApplicationTitleVisibility
		{
			get { return (Visibility) GetValue(ApplicationTitleVisibilityProperty); }
			set { SetValue(ApplicationTitleVisibilityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ApplicationTitleVisibility.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ApplicationTitleVisibilityProperty =
			DependencyProperty.Register("ApplicationTitleVisibility", typeof (Visibility), typeof (BaseHeader),
			                            new PropertyMetadata(Visibility.Visible));


		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var but = GetTemplateChild("backButton") as Button;

			if (but != null)
			{
				but.Click += GoBack;

				but.IsEnabled = DesignMode.DesignModeEnabled || AbleToGoBack;
			}
		}

		public bool AbleToGoBack
		{
			get
			{
				var page = GetParentsPage();

				// Use the navigation frame to return to the previous page
				if (page != null && page.Frame != null)
					return page.Frame.CanGoBack;

				return false;
			}
		}

		private Page GetParentsPage()
		{
			var temp = this as FrameworkElement;
			while (temp != null && temp.Parent != null)
			{

				if (temp.Parent is Page)
					return temp.Parent as Page;

				temp = temp.Parent as FrameworkElement;
			}

			return null;
		}

		public void GoBack(object sender, RoutedEventArgs e)
		{
			var page = GetParentsPage();

			// Use the navigation frame to return to the previous page
			if (page != null && page.Frame != null && page.Frame.CanGoBack)
				page.Frame.GoBack();
		}
	}
}