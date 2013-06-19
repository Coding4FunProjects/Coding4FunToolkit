using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Storage;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using FileExplorerExperimental.Control.Interop;

namespace FileExplorerExperimental.Control
{
    public partial class FileExplorer : UserControl, INotifyPropertyChanged
    {
        PhoneApplicationPage _currentPage;
        PhoneApplicationFrame _currentFrame;

        ExternalStorageDevice _currentStorageDevice;

        Stack<ExternalStorageFolder> _folderTree { get; set; }

        bool _mustRestoreApplicationBar = false;
        bool _mustRestoreSystemTray = false;

        public event OnDismissEventHandler OnDismiss;
        public delegate void OnDismissEventHandler(ExternalStorageFile file);

        #region Control Properties

        private ObservableCollection<FileExplorerItem> _currentItems;
        /// <summary>
        /// The container that carries the items currently visible to the user.
        /// </summary>
        public ObservableCollection<FileExplorerItem> CurrentItems 
        {
            get
            {
                return _currentItems;
            }
            private set
            {
                if (_currentItems != value)
                {
                    _currentItems = value;
                    NotifyPropertyChanged("CurrentItems");
                }
            }
        }

        private string _currentPath;
        /// <summary>
        /// The currently active path.
        /// </summary>
        public string CurrentPath
        {
            get
            {
                return _currentPath;
            }
            private set
            {
                if (_currentPath != value)
                {
                    _currentPath = value;
                    NotifyPropertyChanged("CurrentPath");
                }
            }
        }

        private FileExplorerExperimental.Control.Interop.SelectionMode _selectionMode;
        /// <summary>
        /// Determines whether the control will return a folder, file, or multiple files.
        /// </summary>
        public FileExplorerExperimental.Control.Interop.SelectionMode SelectionMode
        {
            get
            {
                return _selectionMode;
            }
            set
            {
                if (_selectionMode != value)
                {
                    _selectionMode = value;
                    NotifyPropertyChanged("SelectionMode");
                }
            }
        }

        #endregion

        public FileExplorer()
        {
            InitializeComponent();

            Initialize();
        }

        async void Initialize()
        {
            this.DataContext = this;

            _folderTree = new Stack<ExternalStorageFolder>();
            CurrentItems = new ObservableCollection<FileExplorerItem>();

            LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;

            try
            {
                var storageAssets = await ExternalStorage.GetExternalStorageDevicesAsync();
                _currentStorageDevice = storageAssets.FirstOrDefault();


                if (_currentStorageDevice != null)
                    GetTreeForFolder(_currentStorageDevice.RootFolder);
            }
            catch
            {
                Debug.WriteLine("For some reason the control creation process failed. We're trying to figure out why.");
            }
        }

        async void GetTreeForFolder(ExternalStorageFolder folder)
        {
            CurrentItems.Clear();

            var folderList = await folder.GetFoldersAsync();

            foreach (ExternalStorageFolder _folder in folderList)
            {
                CurrentItems.Add(new FileExplorerItem() { IsFolder = true, Name = _folder.Name, Path = _folder.Path });
            }

            foreach (ExternalStorageFile _file in await folder.GetFilesAsync())
            {
                CurrentItems.Add(new FileExplorerItem() { IsFolder = false, Name = _file.Name, Path = _file.Path });
            }

            if (!_folderTree.Contains(folder))
                _folderTree.Push(folder);

            CurrentPath = _folderTree.First().Path;
        }

        async void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCore.SelectedItem != null)
            {
                FileExplorerItem item = (FileExplorerItem)lstCore.SelectedItem;
                if (item.IsFolder)
                {
                    GetTreeForFolder(await _folderTree.First().GetFolderAsync(item.Name));
                }
                else
                {
                    ExternalStorageFile file = await _currentStorageDevice.GetFileAsync(item.Path);
                    Dismiss(file);
                }
            }
        }

        void TreeUp(object sender, RoutedEventArgs e)
        {
            if (_folderTree.Count > 1)
            {
                _folderTree.Pop();
                GetTreeForFolder(_folderTree.First());
            }
        }


        public void Show()
        {
            _currentFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            _currentPage = _currentFrame.Content as PhoneApplicationPage;

            if (SystemTray.IsVisible)
            {
                _mustRestoreSystemTray = true;
                SystemTray.IsVisible = false;
            }


            if (_currentPage.ApplicationBar != null)
            {
                if (_currentPage.ApplicationBar.IsVisible)
                    _mustRestoreApplicationBar = true;

                _currentPage.ApplicationBar.IsVisible = false;
            }

            if (_currentPage != null)
            {
                _currentPage.BackKeyPress += OnBackKeyPress;
            }

            RootPopup.IsOpen = true;
        }

        void OnBackKeyPress(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Dismiss(null);
        }

        private void Dismiss(ExternalStorageFile file)
        {
            if (_currentPage != null)
            {
                _currentPage.BackKeyPress -= OnBackKeyPress;
            }

            RootPopup.IsOpen = false;

            if (_mustRestoreApplicationBar)
                _currentPage.ApplicationBar.IsVisible = true;

            if (_mustRestoreSystemTray)
                SystemTray.IsVisible = true;

            if (OnDismiss != null)
                OnDismiss(file);
        }

        /// <summary>
        /// Handles property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(info));
                    });
            }
        }

    }
}
