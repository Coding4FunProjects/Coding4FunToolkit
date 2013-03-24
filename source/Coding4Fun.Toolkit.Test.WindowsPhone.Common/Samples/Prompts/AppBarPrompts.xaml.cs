using System;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples.Prompts
{
    public partial class AppBarPrompts : PhoneApplicationPage
    {
        public AppBarPrompts()
        {
            InitializeComponent();
			baseColor = ApplicationBar.BackgroundColor;
        }

	    private System.Windows.Media.Color baseColor; 
        private void twoOptions_Click(object sender, EventArgs e)
        {
			ApplicationBar.BackgroundColor = baseColor;

            new AppBarPrompt(new AppBarPromptAction("option 1", () => { })
                             , new AppBarPromptAction("option 2", () => { })).Show();
        }

        private void threeOptions_Click(object sender, EventArgs e)
        {
	        ApplicationBar.BackgroundColor = Colors.Green;

			new AppBarPrompt(new AppBarPromptAction("option 1", () => { })
							 , new AppBarPromptAction("option 2", () => { })
							 , new AppBarPromptAction("option 3", () => { })) 
							 { Foreground = new SolidColorBrush(Colors.Orange) }.Show();
        }

        private void fourOptions_Click(object sender, EventArgs e)
        {
			ApplicationBar.BackgroundColor = baseColor;

            new AppBarPrompt(new AppBarPromptAction("option 1", () => { })
                             , new AppBarPromptAction("option 2", () => { })
                             , new AppBarPromptAction("option 3", () => { })
                             , new AppBarPromptAction("option 4", () => { })) { Foreground = new SolidColorBrush(Colors.Green) }.Show();
        }
    }
}