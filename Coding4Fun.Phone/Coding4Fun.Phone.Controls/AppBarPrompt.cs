using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Clarity.Phone.Extensions;

namespace Coding4Fun.Phone.Controls
{
    public class AppBarPrompt : PopUp<string, PopUpResult> 
    {
        protected StackPanel Body;
		private const string BodyName = "Body";

		private static readonly Color NullColor = Color.FromArgb(0, 0, 0, 0);
		readonly AppBarPromptAction[] _theActions;

		public AppBarPrompt()
		{
			DefaultStyleKey = typeof (AppBarPrompt);
		}

	    public AppBarPrompt(params AppBarPromptAction[] actions) : this()
        {
            IsAppBarVisible = !CheckForApplicationBar();
            IsBackKeyOverride = false;
            
            AnimationType = DialogService.AnimationTypes.Swivel;
            _theActions = actions;
        }

        /// <summary>
        /// Checks for application bar.
        /// </summary>
        /// <returns></returns>
        private bool CheckForApplicationBar()
        {
            if (Page.ApplicationBar != null)
            {
                VerifyAppBarBackgroundColor();
				VerifyAppBarForegroundColor();
                
				return Page.ApplicationBar.IsVisible;
            }

            return false;
        }

	    /// <summary>
	    /// Checks the app bar foreground colour.
	    /// </summary>
	    /// <returns></returns>
	    private void VerifyAppBarForegroundColor()
        {
			var color = Page.ApplicationBar.ForegroundColor;

			if (color != NullColor) // Default system theme no colour is given
			{
				Foreground = new SolidColorBrush(Page.ApplicationBar.BackgroundColor);
			}
        }

	    /// <summary>
	    /// Checks the app bar background colour.
	    /// </summary>
	    /// <returns></returns>
	    private void VerifyAppBarBackgroundColor()
        {
			var color = Page.ApplicationBar.BackgroundColor;

			if (color != NullColor) // Default system theme no colour is given
			{
				Background = new SolidColorBrush(Page.ApplicationBar.BackgroundColor);
			}
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get the stackpanel from the template so we can populate its contents with the 
            // provided actions.
			Body = GetTemplateChild(BodyName) as StackPanel;

			if (Body != null)
			{
				foreach (var action in _theActions)
				{
					action.Parent = this;
					var menuItem = new AppBarPromptItem
						{
							Content = action.Content,
							Command = action.Command,
							Foreground = Foreground
						};

					Body.Children.Add(menuItem);
				}
			}
        }

        internal void Close()
        {
            Hide();
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <value>The frame.</value>
        private static PhoneApplicationFrame Frame
        {
            get
            {
                return Application.Current.RootVisual as PhoneApplicationFrame;
            }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>The page.</value>
        private static PhoneApplicationPage Page
        {
            get
            {
                return Frame.Content as PhoneApplicationPage;
            }
        }
    }
}
