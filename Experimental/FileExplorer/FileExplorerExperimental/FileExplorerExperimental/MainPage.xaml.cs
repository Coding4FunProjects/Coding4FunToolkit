using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using Microsoft.Phone.Storage;
using Windows.Storage;

namespace FileExplorerExperimental
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ExplorerControl.OnDismiss += ExplorerControl_OnDismiss;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExplorerControl.Show();
        }

        void ExplorerControl_OnDismiss(Control.Interop.StorageTarget target, object file)
        {
            Debug.WriteLine(target);

            if (file != null)
            {
                switch (target)
                {
                    case Control.Interop.StorageTarget.ExternalStorage:
                        {
                            ExternalStorageFile _stFile = (ExternalStorageFile)file;
                            Debug.WriteLine(_stFile.Path);
                            break;
                        }
                    case Control.Interop.StorageTarget.IsolatedStorage:
                        {
                            StorageFile _stFile = (StorageFile)file;
                            Debug.WriteLine(_stFile.Path);
                            break;
                        }
                }
            }
        }
    }
}