using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImageTileMemoryIssueWinStoreTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        private readonly string[] _flickrImages = new[]
			{
				"http://farm8.staticflickr.com/7210/6948063601_9c06c977b7_k_d.jpg",
				"http://farm3.staticflickr.com/2762/4445148236_16ef33835e_b_d.jpg",
				"http://farm5.staticflickr.com/4012/4445153474_6ec68c3692_b_d.jpg",
				"http://farm5.staticflickr.com/4020/4445152626_6bf11c6bcb_b_d.jpg",
				"http://farm5.staticflickr.com/4050/4444381435_7a25970e82_b_d.jpg",
				"http://farm5.staticflickr.com/4072/4444380917_257105dcd7_b_d.jpg",
				"http://farm5.staticflickr.com/4031/4444379947_66fd848c75_b_d.jpg",
				"http://farm5.staticflickr.com/4058/4445150306_0f0ab6e920_b_d.jpg",
				"http://farm5.staticflickr.com/4046/4444378913_1cdf61b016_b_d.jpg",
				"http://farm5.staticflickr.com/4058/4445148506_3f0d18bafe_b_d.jpg",
				"http://farm5.staticflickr.com/4069/4444378479_45fbb0e488_b_d.jpg",
				"http://farm3.staticflickr.com/2797/4445148868_3b3a00f0b4_b_d.jpg",

			};

		private DispatcherTimer _timer;
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			Loaded += MainPageLoaded;
		}

		private ObservableCollection<Item> _items; 
		void TimerTick(object sender, object e)
		{
			CreateNewAvatars();
		}
		private void CreateNewAvatars()
		{
			if(_items == null)
				_items = new ObservableCollection<Item>();

			_items.Clear();
			for (int j = 0; j < 50; j++)
			{
				var item = new Item();
				var items = new List<Uri>();

				for (int i = 0; i < _flickrImages.Count(); i++)
				{
					items.Add(new Uri(_flickrImages[i], UriKind.RelativeOrAbsolute));
				}

				item.Avatars = items;
				_items.Add(item);
			}

			data.ItemsSource = _items;
		}

		void MainPageLoaded(object sender, RoutedEventArgs e)
		{
			TimerTick(null, null);

			_timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 10) };
			_timer.Tick += TimerTick;
			_timer.Start();
		}
    }
}
