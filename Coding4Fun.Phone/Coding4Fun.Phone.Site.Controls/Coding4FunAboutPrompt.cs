using Coding4Fun.Phone.Controls;

namespace Coding4Fun.Phone.Site.Controls
{
    public class Coding4FunAboutPrompt : AboutPrompt
    {
        public Coding4FunAboutPrompt()
        {
            DataContext = this;

            WaterMark = new Coding4FunWaterMark();
            Footer = new Coding4FunFooter();
        }
    }
}
