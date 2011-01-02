using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls
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
