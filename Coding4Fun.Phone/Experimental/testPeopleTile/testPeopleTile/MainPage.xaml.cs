using System;
using System.Collections.Generic;

using Microsoft.Phone.Controls;

namespace testPeopleTile
{
    public partial class MainPage : PhoneApplicationPage
    {
    	readonly List<Uri> _imgUriList = new List<Uri>();

		public MainPage()
        {
            InitializeComponent();

			//for (int i = 0; i < allImages.Count(); i++)
			//{
			//    var img = allImages[i] as Image;

			//    var index = i + 1;
			//    if (img != null)
			//        img.Source = GetImage(index);

			//    _currentlyDisplayed.Add(GetFileUrl(index));
			//}

            for (int i = 1; i < 16; i++)
            {
                _imgUriList.Add(new Uri(String.Format("Images/{0}.jpg", i), UriKind.Relative));
            }

            fadeTile.ItemsSource = _imgUriList;
			verticalExpandTile.ItemsSource = _imgUriList;
			horizontalExpandTile.ItemsSource = _imgUriList;
        }
    }
}