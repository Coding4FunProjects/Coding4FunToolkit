using System.Globalization;
using System.IO.IsolatedStorage;
using System.Windows;

using Coding4Fun.Toolkit.Controls;
using Coding4Fun.Toolkit.Storage;
using Coding4Fun.Toolkit.Test.WindowsPhone.Test;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Toolkit.Test.WindowsPhone.Samples
{
    public partial class Storage : PhoneApplicationPage
    {
        private const string MyDataFileName = "Data.FooBar";

        public Storage()
        {
            InitializeComponent();
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            var data = Serialize.Open<TestSerializeClass>(MyDataFileName);

            stringData.Text = (!string.IsNullOrEmpty(data.StringData)) ? data.StringData : string.Empty;
            intData.Text = data.IntData.ToString(CultureInfo.InvariantCulture);
            dateTimeData.Value = data.DateTimeData;
            timeSpanData.Value = data.TimeSpanData;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var data = new TestSerializeClass();

            data.StringData = stringData.Text;

            int tempInt;
            int.TryParse(intData.Text, out tempInt);
            data.IntData = tempInt;

            if (dateTimeData.Value != null) 
                data.DateTimeData = dateTimeData.Value.Value;

            if (timeSpanData.Value != null) 
                data.TimeSpanData = timeSpanData.Value.Value;

            Serialize.Save(MyDataFileName, data);

            var prompt = new MessagePrompt {Title = "Saved", Message = "data saved"};
            prompt.Show();
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            stringData.Text = "";
            intData.Text = "";
            dateTimeData.Value = null;
            timeSpanData.Value = null;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            // verify stuff won't blow up if file isn't there
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFile.FileExists(MyDataFileName))
                    storageFile.DeleteFile(MyDataFileName);
            }
        }
    }
}