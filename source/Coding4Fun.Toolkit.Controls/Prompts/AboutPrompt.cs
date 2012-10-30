using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Coding4Fun.Toolkit.Controls.Common;

namespace Coding4Fun.Toolkit.Controls
{
    public class AboutPrompt : ActionPopUp<object, PopUpResult>
    {
        /// <summary>
        /// Should control show the bottom buttons to act as a prompt.
        /// IE: Example usage would be if control wanted to be used in control on a pivot rather than a popup.
        /// </summary>
        public bool IsPromptMode
        {
            get { return (bool)GetValue(IsPromptModeProperty); }
            set { SetValue(IsPromptModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPromptMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPromptModeProperty =
            DependencyProperty.Register("IsPromptMode", typeof(bool), typeof(AboutPrompt), new PropertyMetadata(true, OnIsPromptModeChanged));

       public object WaterMark
        {
            get { return (object)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(object), typeof(AboutPrompt), new PropertyMetadata(null));

        public string VersionNumber
        {
            get { return (string)GetValue(VersionNumberProperty); }
            set { SetValue(VersionNumberProperty, value); }
        }
        public static readonly DependencyProperty VersionNumberProperty =
            DependencyProperty.Register("VersionNumber", typeof(object), typeof(AboutPrompt), new PropertyMetadata("v" + PhoneHelper.GetAppAttribute("Version").Replace(".0.0", "")));

        public object Body
        {
            get { return GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Body.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(object), typeof(AboutPrompt), new PropertyMetadata(null));

        public object Footer
        {
            get { return GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Footer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(object), typeof(AboutPrompt), new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AboutPrompt), new PropertyMetadata(PhoneHelper.GetAppAttribute("Title")));

        public AboutPrompt()
        {
            DefaultStyleKey = typeof(AboutPrompt);
            
            var okButton = new RoundButton();
            okButton.Click += ok_Click;

            ActionPopUpButtons.Add(okButton);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetIsPromptMode(IsPromptMode);
        }

        private static void OnIsPromptModeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((AboutPrompt)o);
            if (sender != null && sender.ActionButtonArea != null && e.NewValue != e.OldValue)
                sender.SetIsPromptMode((bool)e.NewValue);
        }

        private void SetIsPromptMode(bool value)
        {
            if(ActionButtonArea != null)
                ActionButtonArea.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<object, PopUpResult> { PopUpResult = PopUpResult.Ok });
        }

        public void Show(string authorName, string twitterName = null, string emailAddress = null, string websiteUrl = null)
        {
			var aboutItems = new List<AboutPromptItem> { new AboutPromptItem { Role = "me", AuthorName = authorName } };

        	if(!string.IsNullOrEmpty(twitterName))
				aboutItems.Add(new AboutPromptItem { Role = "twitter", WebSiteUrl = "http://www.twitter.com/" + twitterName.TrimStart('@') });

			if(!string.IsNullOrEmpty(websiteUrl))
				aboutItems.Add(new AboutPromptItem { Role = "web", WebSiteUrl = websiteUrl });
			
			if(!string.IsNullOrEmpty(emailAddress))
				aboutItems.Add(new AboutPromptItem { Role = "email", EmailAddress = emailAddress });

			Show(aboutItems.ToArray());
        }

		public void Show(params AboutPromptItem[] people)
        {
            if (people != null && people.Length > 0)
            {
                var panel = new StackPanel
                                {
                                    HorizontalAlignment = HorizontalAlignment.Stretch,
                                    VerticalAlignment = VerticalAlignment.Stretch
                                };

                for (var i = people.Length - 1; i >= 0; i--)
                    panel.Children.Insert(0, people[i]);

                Body = panel;
            }

            base.Show();
        }
    }
}