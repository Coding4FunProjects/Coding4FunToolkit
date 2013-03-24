using System;
using System.Windows.Controls;
using System.Windows.Media;

using Clarity.Phone.Extensions;

namespace Coding4Fun.Toolkit.Controls
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
			MainBodyDelay = TimeSpan.FromMilliseconds(100);
		}

	    public AppBarPrompt(params AppBarPromptAction[] actions) : this()
        {
            AnimationType = DialogService.AnimationTypes.Swivel;

			_theActions = actions;
        }

	    /// <summary>
	    /// Checks the app bar background color.
	    /// </summary>
	    /// <returns></returns>
	    private void VerifyAppBarBackgroundColor()
        {
			var color = PopUpService.Page.ApplicationBar.BackgroundColor;

			if (color != NullColor) // Default system theme no color is given
			{
				Background = new SolidColorBrush(color);
			}
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

			VerifyAppBarBackgroundColor();
			
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
						};

					Body.Children.Add(menuItem);
				}
			}
        }
    }
}
