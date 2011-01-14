﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Coding4Fun.Phone.Controls.Data;

using Microsoft.Phone.Tasks;

namespace Coding4Fun.Phone.Controls
{
    public class AboutPersonItem : Control
    {
        protected TextBlock emailAddress;
        protected TextBlock website;
        protected TextBlock author;

        private const string EmailAddressName = "emailAddress";
        private const string WebsiteName = "website";
        private const string AuthorTxtBlockName = "author";

        public AboutPersonItem()
        {
            DefaultStyleKey = typeof(AboutPersonItem);
            DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (website != null)
                website.ManipulationCompleted -= websiteClick_ManipulationCompleted;

            if (emailAddress != null)
                emailAddress.ManipulationCompleted -= email_ManipulationCompleted;
            
            emailAddress = GetTemplateChild(EmailAddressName) as TextBlock;
            website = GetTemplateChild(WebsiteName) as TextBlock;
            author = GetTemplateChild(AuthorTxtBlockName) as TextBlock;

            SetVisibility(emailAddress);
            SetVisibility(website);
            SetVisibility(author);

            if (emailAddress != null)
                emailAddress.ManipulationCompleted += email_ManipulationCompleted;

            if (website != null)
                website.ManipulationCompleted += websiteClick_ManipulationCompleted;
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
                SetVisibility(website);
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
            DependencyProperty.Register("WebSiteDisplay", typeof(string), typeof(AboutPersonItem), new PropertyMetadata(""));
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
            DependencyProperty.Register("Role", typeof(string), typeof(AboutPersonItem), new PropertyMetadata(""));
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
                SetVisibility(emailAddress);
            }
        }

        // Using a DependencyProperty as the backing store for EmailAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmailAddressProperty =
            DependencyProperty.Register("EmailAddress", typeof(string), typeof(AboutPersonItem), new PropertyMetadata(""));
        
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
                SetVisibility(author);
            }
        }

        // Using a DependencyProperty as the backing store for EmailAddress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorNameProperty =
            DependencyProperty.Register("AuthorName", typeof(string), typeof(AboutPersonItem), new PropertyMetadata(""));

        #endregion
        protected internal void websiteClick_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            navigateTo(WebSiteUrl);
        }

        private void navigateTo(string uri)
        {
            var web = new WebBrowserTask { URL = uri };
            web.Show();
        }
    }
}