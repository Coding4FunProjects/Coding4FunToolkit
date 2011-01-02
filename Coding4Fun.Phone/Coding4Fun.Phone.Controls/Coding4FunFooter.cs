using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;

namespace Coding4Fun.Phone.Controls
{
    public class Coding4FunFooter : Control
    {
        private const string c4fLogoName = "c4fLogo";
        protected Image c4fLogo;

        public Coding4FunFooter()
        {
            DefaultStyleKey = typeof(Coding4FunFooter);
            DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (c4fLogo != null)
                c4fLogo.ManipulationCompleted -= c4fLogo_ManipulationCompleted;

            c4fLogo = GetTemplateChild(c4fLogoName) as Image;

            if (c4fLogo != null)
                c4fLogo.ManipulationCompleted += c4fLogo_ManipulationCompleted;
        }

        void c4fLogo_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            NavigateTo("http://www.coding4fun.com");
        }

        private static void NavigateTo(string uri)
        {
            var web = new WebBrowserTask { URL = uri };
            web.Show();
        }
    }
}
