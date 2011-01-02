using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls
{
    public class AboutPrompt : PopUp<object>
    {
        private const string OkButtonName = "okButton";
        protected Button okButton;

        public object WaterMark
        {
            get { return (object)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaterMark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(object), typeof(AboutPrompt), new PropertyMetadata(null));

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
            DependencyProperty.Register("Title", typeof(string), typeof(AboutPrompt), new PropertyMetadata("About"));

        public AboutPrompt()
        {
            DefaultStyleKey = typeof(AboutPrompt);
            DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (okButton != null)
                okButton.Click -= ok_Click;

            okButton = GetTemplateChild(OkButtonName) as Button;

            if (okButton != null)
                okButton.Click += ok_Click;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<object> { PopUpResult=PopUpResult.OK });
        }

        public void Show(string authorName, string twitterName, string emailAddress, string websiteUrl)
        {
            Show(new[] { 
                new AboutPersonItem { Role="me", AuthorName=authorName},
                new AboutPersonItem { Role="twitter", WebSiteUrl="http://www.twitter.com/" + twitterName.TrimStart('@')},
                new AboutPersonItem { Role="web", WebSiteUrl=websiteUrl},
                new AboutPersonItem { Role="email", EmailAddress=emailAddress},
            });
        }

        public void Show(params AboutPersonItem[] people)
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
