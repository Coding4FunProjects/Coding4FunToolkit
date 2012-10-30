using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Coding4Fun.Toolkit.Controls.Common;

using Microsoft.Phone.Tasks;

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

		public AboutPromptItem()
        {
            DefaultStyleKey = typeof(AboutPromptItem);
        }

        public override void OnApplyTemplate()
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

        private static void SetVisibility(TextBlock control)
        {
            if (control != null)
                control.Visibility = (string.IsNullOrEmpty(control.Text)) ? Visibility.Collapsed : Visibility.Visible;
        }

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

        private const string Http = "http://";
        private const string Https = "https://";
        private const string Twitter = "www.twitter.com";
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
        
        protected internal void email_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var email = new EmailComposeTask
            {
                To = EmailAddress,
                Subject = PhoneHelper.GetAppAttribute("Title") + " Feedback"
            };

            email.Show();
        }
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
        protected internal void websiteClick_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            NavigateTo(WebSiteUrl);
        }

        private static void NavigateTo(string uri)
        {
            var web = new WebBrowserTask { Uri = new Uri(uri) };
            
			web.Show();
        }
    }
}
