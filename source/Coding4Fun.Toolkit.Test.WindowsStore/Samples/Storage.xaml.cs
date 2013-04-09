using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Coding4Fun.Toolkit.Storage;
using Coding4Fun.Toolkit.Test.WindowsStore.Test;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;


namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class Storage
	{
		private const string MyDataFileName = "MyData.dat";

		public Storage()
		{
			InitializeComponent();
		}

		private async void LoadClick(object sender, RoutedEventArgs e)
		{
			var data = await Serializer.Open<TestSerializeClass>(MyDataFileName);

			stringData.Text = (!string.IsNullOrEmpty(data.StringData)) ? data.StringData : string.Empty;
			intData.Text = data.IntData.ToString(CultureInfo.InvariantCulture);

			dateTimeData.Text = data.DateTimeData.ToString();
			timeSpanData.Text = data.TimeSpanData.ToString();
			//dateTimeData.Value = data.DateTimeData;
			//timeSpanData.Value = data.TimeSpanData;
		}

		private void SaveClick(object sender, RoutedEventArgs e)
		{
			var data = new TestSerializeClass();

			data.StringData = stringData.Text;

			int tempInt;
			int.TryParse(intData.Text, out tempInt);
			data.IntData = tempInt;

			//if (dateTimeData.Value != null)
			//	data.DateTimeData = dateTimeData.Value.Value;

			//if (timeSpanData.Value != null)
			//	data.TimeSpanData = timeSpanData.Value.Value;

			data.DateTimeData = DateTime.Now;
			data.TimeSpanData = DateTime.Now.Subtract(DateTime.Today);

			Serializer.Save(MyDataFileName, data);

			
			//var prompt = new MessagePrompt { Title = "Saved", Message = "data saved" };
			//prompt.Show();
		}

		private void ClearClick(object sender, RoutedEventArgs e)
		{
			stringData.Text = "";
			intData.Text = "";
			//dateTimeData.Value = null;
			//timeSpanData.Value = null;

			dateTimeData.Text = "";
			timeSpanData.Text = "";
		}

		private async void DeleteClick(object sender, RoutedEventArgs e)
		{
			var storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;

			var files = await storageFolder.GetFilesAsync(CommonFileQuery.OrderByName);
			var file = files.FirstOrDefault(x => x.Name == MyDataFileName) ??
					   await storageFolder.CreateFileAsync(MyDataFileName, CreationCollisionOption.ReplaceExisting);


			await file.DeleteAsync();
		}
	}
}