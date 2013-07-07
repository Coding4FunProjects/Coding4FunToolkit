using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using Microsoft.Phone.Storage;
using Windows.Storage;
using FileExplorerExperimental.Control.Interop;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FileExplorerExperimental
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ExplorerControl.OnDismiss += ExplorerControl_OnDismiss;
            //ExplorerControl.ExtensionRestrictions = Control.Interop.ExtensionRestrictions.InheritManifest;
            ExplorerControl.Extensions = new System.Collections.Generic.List<string>() { ".png" };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExplorerControl.Show();
            //XDocument doc = ManifestReader.GetAppManifest();
            //var list = ManifestReader.GetRegisteredExtensions();
            //Debug.WriteLine(doc.Root);
        }

        void ExplorerControl_OnDismiss(Control.Interop.StorageTarget target, object file)
        {
            if (file != null)
            {
                switch (target)
                {
                    case Control.Interop.StorageTarget.ExternalStorage:
                        {
                            if (ExplorerControl.SelectionMode == SelectionMode.File)
                            {
                                ExternalStorageFile _stFile = (ExternalStorageFile)file;
                                Debug.WriteLine(_stFile.Path); 
                            }
                            else if (ExplorerControl.SelectionMode == SelectionMode.Folder)
                            {
                                ExternalStorageFolder folder = (ExternalStorageFolder)file;
                                Debug.WriteLine(folder.Path);
                            }
                            else
                            {
                                List<FileExplorerItem> items = (List<FileExplorerItem>)file;
                                foreach (var item in items)
                                {
                                    Debug.WriteLine(item.Path);
                                }
                            }
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