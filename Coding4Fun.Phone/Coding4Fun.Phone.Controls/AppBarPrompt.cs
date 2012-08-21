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
                Background = CheckAppBarBackgroundColour(Page.ApplicationBar.BackgroundColor);
                Foreground = CheckAppBarForegroundColour(Page.ApplicationBar.ForegroundColor);
                return Page.ApplicationBar.IsVisible;
            }
            return false;
        }

        /// <summary>
        /// Checks the app bar foreground colour.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        private Brush CheckAppBarForegroundColour(Color color)
        {
            if (color.ToString().Equals("#00000000")) // Default system theme no colour is given
            {
                // Check to see whether we're in the dark theme.
                bool isDark = ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible);
                color = isDark ? Colors.White : Colors.Black;
            }
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// Checks the app bar background colour.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        private Brush CheckAppBarBackgroundColour(Color color)
        {
            if (color.ToString().Equals("#00000000")) // Default system theme no colour is given
            {
                // Check to see whether we're in the dark theme.
                bool isDark = ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible); 
                color = isDark ? Color.FromArgb(255, 33, 32, 33) : Color.FromArgb(255, 223, 223, 223);
            }
            return new SolidColorBrush(color);
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
