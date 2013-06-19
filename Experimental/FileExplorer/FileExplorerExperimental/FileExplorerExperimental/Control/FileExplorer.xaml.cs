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
using Windows.Storage;
using System;

namespace FileExplorerExperimental.Control
{
    public partial class FileExplorer : UserControl, INotifyPropertyChanged
    {
        PhoneApplicationPage _currentPage;
        PhoneApplicationFrame _currentFrame;

        ExternalStorageDevice _currentStorageDevice;

        Stack<ExternalStorageFolder> _externalFolderTree { get; set; }
        Stack<StorageFolder> _internalFolderTree { get; set; }

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

        private StorageTarget _storageTarget;
        public StorageTarget StorageTarget
        {
            get
            {
                return _storageTarget;
            }
            set
            {
                if (_storageTarget != value)
                {
                    _storageTarget = value;
                    NotifyPropertyChanged("StorageTarget");
                }
            }
        }

        #endregion

        public FileExplorer()
        {
            InitializeComponent();
        }

        void Initialize()
        {
            this.DataContext = this;
            CurrentItems = new ObservableCollection<FileExplorerItem>();

            LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
            LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;

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

            InitializeStorageContainers();
        }

        async void InitializeStorageContainers()
        {
            if (StorageTarget == Interop.StorageTarget.ExternalStorage)
            {
                _externalFolderTree = new Stack<ExternalStorageFolder>();

                try
                {

                    var storageAssets = await ExternalStorage.GetExternalStorageDevicesAsync();
                    _currentStorageDevice = storageAssets.FirstOrDefault();


                    if (_currentStorageDevice != null)
                        GetTreeForExternalFolder(_currentStorageDevice.RootFolder);
                }
                catch
                {
                    Debug.WriteLine("EXT_STORAGE_ERROR: There was a problem accessing external storage.");
                }
            }
            else
            {
                _internalFolderTree = new Stack<StorageFolder>();

                try
                {
                    GetTreeForInternalFolder(ApplicationData.Current.LocalFolder);
                }
                catch
                {
                    Debug.WriteLine("INT_STORAGE_ERROR: There was a problem accessing internal storage.");
                }
            }
        }

        /// <summary>
        /// Will retrieve the full folder and file tree for a folder from the internal storage.
        /// </summary>
        /// <param name="folder">The instance of the folder for which the tree will be retrieved.</param>
        async void GetTreeForInternalFolder(StorageFolder folder)
        {
            CurrentItems.Clear();

            var folderList = await folder.GetFoldersAsync();

            foreach (StorageFolder _folder in folderList)
            {
                CurrentItems.Add(new FileExplorerItem() { IsFolder = true, Name = _folder.Name, Path = _folder.Path });
            }

            var fileList = await folder.GetFilesAsync();
            if (fileList != null)
            {
                foreach (StorageFile _file in fileList)
                {
                    CurrentItems.Add(new FileExplorerItem() { IsFolder = false, Name = _file.Name, Path = _file.Path });
                }
            }

            if (!_internalFolderTree.Contains(folder))
                _internalFolderTree.Push(folder);

            CurrentPath = _internalFolderTree.First().Path;
        }

        async void GetTreeForExternalFolder(ExternalStorageFolder folder)
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

            if (!_externalFolderTree.Contains(folder))
                _externalFolderTree.Push(folder);

            CurrentPath = _externalFolderTree.First().Path;
        }

        async void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCore.SelectedItem != null)
            {
                FileExplorerItem item = (FileExplorerItem)lstCore.SelectedItem;
                if (item.IsFolder)
                {
                    if (StorageTarget == Interop.StorageTarget.ExternalStorage)
                    {
                        GetTreeForExternalFolder(await _externalFolderTree.First().GetFolderAsync(item.Name));
                    }
                    else
                    {
                        GetTreeForInternalFolder(await _internalFolderTree.First().GetFolderAsync(item.Name));
                    }
                }
                else
                {
                    if (StorageTarget == Interop.StorageTarget.ExternalStorage)
                    {
                        ExternalStorageFile file = await _currentStorageDevice.GetFileAsync(item.Path);

                        Dismiss(file);
                    }
                }
            }
        }

        void TreeUp(object sender, RoutedEventArgs e)
        {
            if (StorageTarget == Interop.StorageTarget.ExternalStorage)
            {
                if (_externalFolderTree.Count > 1)
                {
                    _externalFolderTree.Pop();
                    GetTreeForExternalFolder(_externalFolderTree.First());
                }
            }
            else
            {
                if (_internalFolderTree.Count > 1)
                {
                    _internalFolderTree.Pop();
                    GetTreeForInternalFolder(_internalFolderTree.First());
                }
            }
        }


        public void Show()
        {
            Initialize();
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
