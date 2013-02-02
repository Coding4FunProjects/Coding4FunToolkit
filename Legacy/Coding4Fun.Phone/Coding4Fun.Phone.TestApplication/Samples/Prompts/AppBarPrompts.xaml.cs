using System;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples.Prompts
{
    public partial class AppBarPrompts : PhoneApplicationPage
    {
        public AppBarPrompts()
        {
            InitializeComponent();
        }

        private void twoOptions_Click(object sender, EventArgs e)
        {
            new AppBarPrompt(new AppBarPromptAction("option 1", () => { })
                             , new AppBarPromptAction("option 2", () => { })).Show();
        }

        private void threeOptions_Click(object sender, EventArgs e)
        {
            new AppBarPrompt(new AppBarPromptAction("option 1", () => { })
                             , new AppBarPromptAction("option 2", () => { })
                             , new AppBarPromptAction("option 3", () => { })).Show();
        }

        private void fourOptions_Click(object sender, EventArgs e)
        {
            new AppBarPrompt(new AppBarPromptAction("option 1", () => { })
                             , new AppBarPromptAction("option 2", () => { })
                             , new AppBarPromptAction("option 3", () => { })
                             , new AppBarPromptAction("option 4", () => { })).Show();
        }
    }
}