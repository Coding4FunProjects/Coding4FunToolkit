using Coding4Fun.Toolkit.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone
{
    public class Coding4FunAboutPrompt : AboutPrompt
    {
        public Coding4FunAboutPrompt()
        {
            WaterMark = new Coding4FunWaterMark();
            Footer = new Coding4FunFooter();
        }
    }
}
