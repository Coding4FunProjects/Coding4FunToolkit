using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Phone.Controls;

namespace testPeopleTile
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

			var allImages = foo.Children.Where(i => i is Image).ToArray();

			for (int i = 0; i < allImages.Count(); i++)
			{
				var img = allImages[i] as Image;

				var index = i + 1;
				if (img != null)
					img.Source = GetImage(index);

				_currentlyDisplayed.Add(GetFileUrl(index));
			}
        }

		List<tester> _bigList = new List<tester>();
        Random _rand = new Random();
		
		List<string> _currentlyDisplayed = new List<string>();
		
		int _lastUsedCol = -1;
		int _lastUsedRow = -1;

		const int TotalImages = 15;

        private void ToggleImagesTap(object sender, GestureEventArgs e)
        {
        	var img = new Image {Source = GetRandomImage()};

			int col = GetNonRepeatRandomValue(0, 3, _lastUsedCol);
			int row = GetNonRepeatRandomValue(0, 3, _lastUsedRow);

            var sb = new Storyboard();
            var test = new tester();

            img.SetValue(Grid.ColumnProperty, col);
            img.SetValue(Grid.RowProperty, row);

            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.Stretch = Stretch.UniformToFill;
            img.Name = Guid.NewGuid().ToString();

            foo.Children.Add(img);

            test.sb = sb;
            test.Row = row;
            test.Column = col;

            _bigList.Add(test);
            
            CreateDoubleAnimations(sb, img, "Opacity", 0, 1);
            sb.Begin();
            sb.Completed += sb_Completed;
        }

    	private int GetNonRepeatRandomValue(int minValue, int maxValue, int lastValue)
    	{
    		int returnValue;
    		
			do
    		{
				returnValue = _rand.Next(minValue, maxValue);
			}
			while (returnValue == lastValue);

			return returnValue;
    	}

    	private BitmapImage GetRandomImage()
		{
			int index;

			do
			{
				index = _rand.Next(1, TotalImages);
			}
			while (_currentlyDisplayed.Contains(GetFileUrl(index)));

			_currentlyDisplayed.Add(GetFileUrl(index));

			return GetImage(index);
		}

		private static BitmapImage GetImage(int fileIndex)
		{
			var imgFile = GetFileUrl(fileIndex);

			return new BitmapImage(new Uri(imgFile, UriKind.RelativeOrAbsolute));
		}

    	private static string GetFileUrl(int fileIndex)
    	{
    		return string.Format("images\\{0}.jpg", fileIndex);
    	}

		void sb_Completed(object sender, EventArgs e)
		{
			var test = sender as Storyboard;
			var result = _bigList.Where(x => x.sb == test).FirstOrDefault();

			var items =
				foo.Children.Where(
					x => (int) x.GetValue(Grid.RowProperty) == result.Row && (int) x.GetValue(Grid.ColumnProperty) == result.Column).
					ToArray();

			for (var i = 0; i < items.Count() - 1; i++)
			{
				_currentlyDisplayed.Remove(((BitmapImage)((Image)items[0]).Source).UriSource.ToString());

				foo.Children.Remove(items[i]);
			}
		}

    	private static void CreateDoubleAnimations(Storyboard sb, DependencyObject target, string propertyPath, double fromValue = 0, double toValue = 0)
        {
            var doubleAni = new DoubleAnimation
            {
                To = toValue,
                From = fromValue,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
            };

            Storyboard.SetTarget(doubleAni, target);
            Storyboard.SetTargetProperty(doubleAni, new PropertyPath(propertyPath));

            sb.Children.Add(doubleAni);
        }
    }

    public struct tester
    {
        public Storyboard sb { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}