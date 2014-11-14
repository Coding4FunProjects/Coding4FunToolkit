﻿using Coding4Fun.Toolkit.Storage;
using Coding4Fun.Toolkit.Test.WinPhone81.Common;
using Coding4Fun.Toolkit.Test.WinPhone81.Test;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Coding4Fun.Toolkit.Test.WinPhone81.Samples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Storage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private const string MyDataFileName = "MyData.dat";

        public Storage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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

        private async void SaveClick(object sender, RoutedEventArgs e)
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

            await Serializer.Save(MyDataFileName, data);

            var msg = new MessageDialog("Saved");
            await msg.ShowAsync();
            //var prompt = new MessagePrompt { Title = "", Message = "data saved" };
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
            await PlatformFileAccess.DeleteFile(MyDataFileName);

            var msg = new MessageDialog("deleted");
            await msg.ShowAsync();
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
