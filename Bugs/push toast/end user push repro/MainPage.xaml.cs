using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Notification;

namespace PushNotification
{
    public partial class MainPage : PhoneApplicationPage
    {
        private HttpNotificationChannel _myChannelToast;
        private string notifiationToastChannelName = "MyChannel";
        private bool isNewToastChannel = false;

        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            InitializePushChannel();
        }
        private void InitializePushChannel()
        {
            try
            {
                _myChannelToast = HttpNotificationChannel.Find(notifiationToastChannelName);
                if (_myChannelToast == null)
                {
                    _myChannelToast = new HttpNotificationChannel(notifiationToastChannelName);
                    isNewToastChannel = true;
                }


                _myChannelToast.ChannelUriUpdated += MyChannelToastChannelToastUriUpdated;


                _myChannelToast.ErrorOccurred += MyChannelToastErrorOccurred;


                _myChannelToast.ShellToastNotificationReceived += MyChannelToastShellToastNotificationReceived;

                if (isNewToastChannel)
                    _myChannelToast.Open();


                if (!_myChannelToast.IsShellToastBound)
                    _myChannelToast.BindToShellToast();

                if (!_myChannelToast.IsShellTileBound)
                    _myChannelToast.BindToShellTile();

                if (!isNewToastChannel)
                    SendUriToSevice(_myChannelToast.ChannelUri);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }


        private void MyChannelToastErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           System.Diagnostics.Debug.WriteLine(e.Message);
                                           MessageBox.Show(String.Format("Error: {0}", e.Message));
                                       });
        }

        private void MyChannelToastChannelToastUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            SendUriToSevice(e.ChannelUri);
        }

        private void SendUriToSevice(Uri channelUri)
        {
            var _serviceUri = channelUri.ToString();

            /*  WebClient wc = new WebClient();
              wc.UploadStringCompleted += wc_UploadStringCompleted;
              wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
              wc.Encoding = Encoding.UTF8;
              Uri uri = new Uri("http://sztls-hyperv4.sztls.local/phpdebug/index.php", UriKind.Absolute);
              String textUri = String.Format("url={0}", _serviceUri);
              wc.UploadStringAsync(uri, textUri);*/


            Debug.WriteLine(channelUri);
            //textbox1.Text = channelUri.ToString();
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Debug.WriteLine(e.Error == null ? "Succesful sent" : e.Error.Message);
        }


	    private void MyChannelToastShellToastNotificationReceived(object sender, NotificationEventArgs e)
	    {
		    Dispatcher.BeginInvoke(() =>
			    {
				    StringBuilder sb = new StringBuilder();

				    foreach (string key in e.Collection.Keys)
				    {
					    sb.AppendFormat("{0}\\", e.Collection[key]);
				    }

				    var res = sb.ToString().Split(Convert.ToChar("\\"));
				    try
				    {

					    ToastPrompt toast = new ToastPrompt();

					    toast.Title = res[0];
					    toast.Message = res[1];
					    //toast.ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));
					    toast.Show();


				    }
				    catch (Exception exc)
				    {
					    Debug.WriteLine(exc);
				    }
			    });
	    }
    }
}