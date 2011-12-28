using System;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Phone.Tasks;

namespace Coding4Fun.Phone.Site.Controls
{
    public class Coding4FunFooter : Control
    {
        private const string C4FLogoName = "c4fLogo";
        protected Image C4FLogo;

        public Coding4FunFooter()
        {
            DefaultStyleKey = typeof(Coding4FunFooter);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (C4FLogo != null)
                C4FLogo.ManipulationCompleted -= c4fLogo_ManipulationCompleted;

            C4FLogo = GetTemplateChild(C4FLogoName) as Image;

            if (C4FLogo != null)
                C4FLogo.ManipulationCompleted += c4fLogo_ManipulationCompleted;
        }

        void c4fLogo_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            NavigateTo("http://www.coding4fun.com");
        }

        private static void NavigateTo(string uri)
        {
            var web = new WebBrowserTask { Uri = new Uri(uri) };
            
			web.Show();
        }
    }
}
