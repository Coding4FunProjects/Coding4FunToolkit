using System.Windows;

using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Site.Controls;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class AboutItems : PhoneApplicationPage
    {
        public AboutItems()
        {
            InitializeComponent();
        }

		private void About_Click(object sender, RoutedEventArgs e)
		{
			var about = new AboutPrompt();
			about.Completed += baseObject_Completed;
			about.Show();
		}
		
		private void C4F_Click(object sender, RoutedEventArgs e)
		{
			var about = new Coding4FunAboutPrompt();
			about.Show("Clint Rutkas", "ClintRutkas", "Clint@Rutkas.com", "http://betterthaneveryone.com");
		}

		void baseObject_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
		{
			// result
		}
    }
}