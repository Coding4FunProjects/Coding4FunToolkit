using System;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Tasks;
#endif

using Coding4Fun.Toolkit.Controls.Common;


namespace Coding4Fun.Toolkit.Controls
{
    public class AboutPromptItem : Control
    {
        TextBlock _emailAddress;
        TextBlock _website;
        TextBlock _author;

        private const string EmailAddressName = "emailAddress";
        private const string WebsiteName = "website";
        private const string AuthorTxtBlockName = "author";

		private const string Http = "http://";
		private const string Https = "https://";
		private const string Twitter = "www.twitter.com";

		public AboutPromptItem()
        {
            DefaultStyleKey = typeof(AboutPromptItem);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            if (_website != null)
                _website.ManipulationCompleted -= websiteClick_ManipulationCompleted;

            if (_emailAddress != null)
                _emailAddress.ManipulationCompleted -= email_ManipulationCompleted;
            
            _emailAddress = GetTemplateChild(EmailAddressName) as TextBlock;
            _website = GetTemplateChild(WebsiteName) as TextBlock;
            _author = GetTemplateChild(AuthorTxtBlockName) as TextBlock;

            SetVisibility(_emailAddress);
            SetVisibility(_website);
            SetVisibility(_author);

            if (_emailAddress != null)
                _emailAddress.ManipulationCompleted += email_ManipulationCompleted;

            if (_website != null)
                _website.ManipulationCompleted += websiteClick_ManipulationCompleted;
        }

        
        
        #region Control Events

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private async void email_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var mailto = new Uri(String.Format("mailto:?to={0}&subject={1} Feedback", EmailAddress, ManifestHelper.GetDisplayName()));
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
#elif WINDOWS_PHONE
		protected internal void email_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
			var email = new EmailComposeTask
			{
				To = EmailAddress,
				Subject = PhoneHelper.GetAppAttribute("Title") + " Feedback"
			};

			email.Show();
		}
#endif

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private async void websiteClick_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var web = new Uri(WebSiteUrl);
            await Windows.System.Launcher.LaunchUriAsync(web);
        }
#elif WINDOWS_PHONE
		protected internal void websiteClick_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var web = new WebBrowserTask { Uri = new Uri(WebSiteUrl) };

            web.Show();
		}
#endif


        #endregion

        #region helper methods

        private static void SetVisibility(TextBlock control)
		{
			if (control != null)
				control.Visibility = (string.IsNullOrEmpty(control.Text)) ? Visibility.Collapsed : Visibility.Visible;
		}

		#endregion

		#region Dependency Property Callbacks
		#endregion

		#region Dependency Properties / Properties

		#region website

		public string WebSiteUrl
		{
			get { return _webSiteUrl; }
			set
			{
				_webSiteUrl = value;
				WebSiteDisplay = value;
				SetVisibility(_website);
			}
		}
		private string _webSiteUrl;

		protected internal string WebSiteDisplay
		{
			get { return (string)GetValue(WebSiteDisplayProperty); }
			set
			{
				if (value != null)
				{
					value = value.ToLowerInvariant().TrimEnd('/');

					if (value.StartsWith(Https))
						value = value.Remove(0, Https.Length);

					if (value.StartsWith(Http))
						value = value.Remove(0, Http.Length);

					if (!string.IsNullOrEmpty(value) && value.StartsWith(Twitter))
						value = "@" + value.Remove(0, Twitter.Length).TrimStart('/');
				}

				SetValue(WebSiteDisplayProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for WebSiteUrl.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WebSiteDisplayProperty =
			DependencyProperty.Register("WebSiteDisplay", typeof(string), typeof(AboutPromptItem), new PropertyMetadata(""));
		#endregion
		#region role
		public string Role
		{
			get { return (string)GetValue(RoleProperty); }
			set
			{
				if (value != null)
					value = value.ToLowerInvariant();
				SetValue(RoleProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for Role.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RoleProperty =
			DependencyProperty.Register("Role", typeof(string), typeof(AboutPromptItem), new PropertyMetadata(""));
		#endregion
		#region email
		public string EmailAddress
		{
			get { return (string)GetValue(EmailAddressProperty); }
			set
			{
				if (value != null)
					value = value.ToLowerInvariant();

				SetValue(EmailAddressProperty, value);
				SetVisibility(_emailAddress);
			}
		}

		// Using a DependencyProperty as the backing store for EmailAddress.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EmailAddressProperty =
			DependencyProperty.Register("EmailAddress", typeof(string), typeof(AboutPromptItem), new PropertyMetadata(""));

		#endregion
		#region author name

		public string AuthorName
		{
			get { return (string)GetValue(AuthorNameProperty); }
			set
			{
				if (value != null)
					value = value.ToLowerInvariant();

				SetValue(AuthorNameProperty, value);
				SetVisibility(_author);
			}
		}

		// Using a DependencyProperty as the backing store for EmailAddress.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AuthorNameProperty =
			DependencyProperty.Register("AuthorName", typeof(string), typeof(AboutPromptItem), new PropertyMetadata(""));

		#endregion

		#endregion
        
	}
}
