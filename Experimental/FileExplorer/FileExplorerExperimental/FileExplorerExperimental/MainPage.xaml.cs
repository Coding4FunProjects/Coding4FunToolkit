using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace FileExplorerExperimental
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExplorerControl.OnDismiss += ExplorerControl_OnDismiss;
            ExplorerControl.Show();
        }

        void ExplorerControl_OnDismiss(Microsoft.Phone.Storage.ExternalStorageFile file)
        {
            throw new NotImplementedException();
        }
    }
}