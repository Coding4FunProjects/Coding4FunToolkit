using System.ComponentModel;
namespace FileExplorerExperimental.Control.Interop
{
    public class FileExplorerItem : INotifyPropertyChanged
    {
        private string _name;
        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _path;
        public string Path 
        {
            get
            {
                return _path;
            }
            set
            {
                if (value != _path)
                {
                    _path = value;
                    NotifyPropertyChanged("Path");
                }
            }
        }

        private bool _isFolder;
        public bool IsFolder 
        {
            get
            {
                return _isFolder;
            }
            set
            {
                if (value != _isFolder)
                {
                    _isFolder = value;
                    NotifyPropertyChanged("IsFolder");
                }
            }
        }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        public override string ToString()
        {
            return Name;
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
