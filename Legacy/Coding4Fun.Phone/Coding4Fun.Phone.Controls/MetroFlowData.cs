using System;
using System.ComponentModel;

namespace Coding4Fun.Phone.Controls
{
	public class MetroFlowData : INotifyPropertyChanged
	{
		public Uri ImageUri
		{
			get { return _imageUri; }
			set
			{
				_imageUri = value;
				RaisePropertyChanged("ImageUri");
			}
		}
		private Uri _imageUri;

		public string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				RaisePropertyChanged("Title");
			}
		}
		private string _title;

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
