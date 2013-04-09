using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Coding4Fun.Phone.Controls;
using Coding4Fun.Phone.Storage;
using Coding4Fun.Phone.TestApplication.Test;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
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
            var data = Serializer.Open<TestSerializeClass>(MyDataFileName);

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

            Serializer.Save(MyDataFileName, data);

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