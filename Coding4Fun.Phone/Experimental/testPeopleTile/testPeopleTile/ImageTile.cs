using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Linq;

namespace testPeopleTile
{
    public class ImageTile : Button
    {
    	DispatcherTimer _timer = new DispatcherTimer();
        Random _rand = new Random();
    	List<ImageTileState> _bigList = new List<ImageTileState>();
    	
		Dictionary<string, Uri> _currentlyDisplayed = new Dictionary<string, Uri>();
		
		List<string> _availableSpotsOnGrid = new List<string>();
		List<string> _available2xSpotsOnGrid = new List<string>();
		List<string> _inuse2xSpotsOnGrid = new List<string>();
		
		//readonly string[] _imgIds = { "00", "01", "02", "10", "11", "12", "20", "21", "22" };
    	//readonly string[] _largeImgIds = { "00", "01", "10", "11" };
        
		Grid _imageContainer;

        private bool _showLargeImage;
        private string _lastLargeId;

        public ImageTile()
		{
            DefaultStyleKey = typeof(ImageTile);

            _timer.Interval = TimeSpan.FromSeconds(1);
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageContainer = (Grid)GetTemplateChild("ImageContainer");

			GridSizeChanged();
			_timer.Tick += timer_Tick;
			//_timer.Tick += ToggleImageTick;
        }

		void ToggleImageTick(object sender, EventArgs e)
		{
			if (_imageContainer == null || ItemsSource == null || ItemsSource.Count <= 0)
				return;

			// needs to have smarter "random" :-)
			int row = _rand.Next(Rows);
			int col = _rand.Next(Columns);

			var img = CreateImage();
			img.SetValue(Grid.ColumnProperty, col);
			img.SetValue(Grid.RowProperty, row);

			_imageContainer.Children.Add(img);
			var sb = new Storyboard();
			var tileState = new ImageTileState {Storyboard = sb, Row = row, Column = col};

			switch (AnimationType)
			{
				case
					ImageTileAnimationType.Fade:
					CreateDoubleAnimations(sb, img, "Opacity", 0, 1, 500);
					break;
				case ImageTileAnimationType.HorizontalExpand:
					img.Projection = new PlaneProjection();
					CreateDoubleAnimations(sb, img.Projection, "RotationY", 270, 360, 500);
					break;
				case ImageTileAnimationType.VerticalExpand:
					img.Projection = new PlaneProjection();
					CreateDoubleAnimations(sb, img.Projection, "RotationX", 270, 360, 500);
					break;
			}

			_bigList.Add(tileState);

			sb.Begin();
			sb.Completed += sb_Completed;
		}

    	private Image CreateImage()
		{
			var img = new Image
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Stretch = Stretch.UniformToFill,
				Name = Guid.NewGuid().ToString()
			};

			img.Source = GetRandomImage(img.Name);

			return img;
		}

        void timer_Tick(object sender, EventArgs e)
        {
            if (_imageContainer != null && ItemsSource != null && ItemsSource.Count > 0)
            {
                int col = -1;
                int row = -1;
                string id = null;

                // first run or when available positons have been filled
                if (_availableSpotsOnGrid.Count == 0)
                {
                    // iterate through the rows and columns and create list of availble position for 1x1 images
                    for (int i = 0; i < this.Rows; i++)
                    {
                        for (int j = 0; j < this.Columns; j++)
                        {
                            _availableSpotsOnGrid.Add(String.Format("{0}{1}", i, j));
                        }
                    }

                    // iterate through rows and columns and create list of positions suitable for 2x2 images
                    for (int i = 0; i < this.Rows-1; i++)
                        for (int j = 0; j < this.Columns-1; j++)
                            _available2xSpotsOnGrid.Add(String.Format("{0}{1}", i, j));
                    
                    if (_showLargeImage)
                    {
                        // every alternate iteration, show 1 large image so every second time code will enter this part
                        // if you have 16 images, it will show once in 32 images.. ie 

                        string largeid = null;
                        while (true)
                        {
                            // randomly select 1 vaible position for 2x2 image
                            largeid = _available2xSpotsOnGrid[_rand.Next(0, _available2xSpotsOnGrid.Count)];
                            if(largeid != _lastLargeId)
                                break;
                        }

                        _lastLargeId = largeid;

                        int largeRow = int.Parse(_lastLargeId.Substring(0, 1));
                        int largeCol = int.Parse(_lastLargeId.Substring(1, 1));

                        _inuse2xSpotsOnGrid.Clear();


                        // now assign positions which will be occupied by 2x2 image
                        for (int i = largeRow; i <= largeRow + 1; i++)
                            for (int j = largeCol; j <= largeCol + 1; j++)
                                _available2xSpotsOnGrid.Add(string.Format("{0}{1}", i, j));
                    }
                    
                    _showLargeImage = !_showLargeImage;
                }

                // the top left position of 2x2 image should be the last to be invalidated.. (at least base when it was scaled earlier)
                // remove it so its not selected until the end
                bool hasImageRemoved = false;

                if (_showLargeImage && _availableSpotsOnGrid.Count > 1)
                {
                    _availableSpotsOnGrid.Remove(_lastLargeId);
                    hasImageRemoved = true;
                }
                
                int iRand = _rand.Next(0, _availableSpotsOnGrid.Count);
                id = _availableSpotsOnGrid[iRand];

                _availableSpotsOnGrid.RemoveAt(iRand);

                if (hasImageRemoved)
                    _availableSpotsOnGrid.Remove(_lastLargeId);

                Image img = null;

                int index = _inuse2xSpotsOnGrid.IndexOf(id);

                if(!_showLargeImage && index > -1)
                {
                    // if showing large image and selected position matches 1 of the 4 2x2 positions, display the image using top left
                    if (_inuse2xSpotsOnGrid.Count == 4)
                    {
                        id = _inuse2xSpotsOnGrid[0]; // top left position for 2x2 image
                        
                        foreach (string val in _inuse2xSpotsOnGrid)
                            _availableSpotsOnGrid.Remove(val);

                        _inuse2xSpotsOnGrid.Clear();
                
                        img = new Image 
						{
							Source = GetRandomImage(_lastLargeId), 
						};

						img.SetValue(Grid.ColumnSpanProperty, 2);
						img.SetValue(Grid.RowSpanProperty, 2);
                    }
                }
                else
                {
                    // any other positon can be filled as usual
                    img = new Image { Source = GetRandomImage(id) };
                }

                row = int.Parse(id.Substring(0, 1));
                col = int.Parse(id.Substring(1, 1));

                var sb = new Storyboard();
                var tileState = new ImageTileState();

            	if (img != null)
            	{
            		img.SetValue(Grid.ColumnProperty, col);
            		img.SetValue(Grid.RowProperty, row);

            		img.HorizontalAlignment = HorizontalAlignment.Center;
            		img.VerticalAlignment = VerticalAlignment.Center;
            		img.Stretch = Stretch.UniformToFill;
            		img.Name = Guid.NewGuid().ToString();

            		_imageContainer.Children.Add(img);

					switch (AnimationType)
					{
						case
							ImageTileAnimationType.Fade:
							CreateDoubleAnimations(sb, img, "Opacity", 0, 1, 500);
							break;
						case ImageTileAnimationType.HorizontalExpand:
							img.Projection = new PlaneProjection();
							CreateDoubleAnimations(sb, img.Projection, "RotationY", 270, 360, 500);
							break;
						case ImageTileAnimationType.VerticalExpand:
							img.Projection = new PlaneProjection();
							CreateDoubleAnimations(sb, img.Projection, "RotationX", 270, 360, 500);
							break;
					}
            	}

            	tileState.Storyboard = sb;
                tileState.Row = row;
                tileState.Column = col;

                _bigList.Add(tileState);

            	sb.Begin();
                sb.Completed += sb_Completed;
            }
        }

        private static void CreateDoubleAnimations(Storyboard sb, DependencyObject target, string propertyPath, double fromValue = 0, double toValue = 0, int speed = 500)
        {
            var doubleAni = new DoubleAnimation
            {
                To = toValue,
                From = fromValue,
                Duration = new Duration(TimeSpan.FromMilliseconds(speed)),
            };

            Storyboard.SetTarget(doubleAni, target);
            Storyboard.SetTargetProperty(doubleAni, new PropertyPath(propertyPath));

            sb.Children.Add(doubleAni);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            var itemStoryboard = sender as Storyboard;
            var result = _bigList.FirstOrDefault(x => x.Storyboard == itemStoryboard);

            var items =
                _imageContainer.Children.Where(
                    x => (int)x.GetValue(Grid.RowProperty) == result.Row && (int)x.GetValue(Grid.ColumnProperty) == result.Column).
                    ToArray();

            for (int i = 0; i < items.Count() - 1; i++)
            {
				var img = items[i] as Image;
				if (img == null)
					continue;

				_imageContainer.Children.Remove(img);
				_currentlyDisplayed.Remove(img.Name);

            }
        }

        private BitmapImage GetRandomImage(string id)
        {
        	Uri item;

            do
            {
            	int index = _rand.Next(0, ItemsSource.Count);

            	item = ItemsSource[index];
			} 
			while (_currentlyDisplayed.ContainsValue(item) &&	// does it contain it already
				_currentlyDisplayed.Count < ItemsSource.Count);	// if does, are we out of items, if so, allow it to slide, 

            _currentlyDisplayed[id] = item;

            return GetImage(item);
        }

        private static BitmapImage GetImage(Uri file)
        {
            BitmapImage retImage;

            try
            {
                retImage = new BitmapImage(file);
				retImage.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(retImage_ImageFailed);
            }
            catch (Exception)
            {
                retImage = new BitmapImage(new Uri("Images/PlaceholderPhoto.png", UriKind.Relative));
            }

            return retImage;
        }

		static void retImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
		{
            //(sender as BitmapImage).UriSource = new Uri("Images/PlaceholderPhoto.png", UriKind.Relative);
		}

		private static void OnGridSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var tile = dependencyObject as ImageTile;

			if (tile != null)
				tile.GridSizeChanged();
		}

		private void GridSizeChanged()
		{
			if (_imageContainer != null)
			{
				int colCount = _imageContainer.ColumnDefinitions.Count;
				int rowCount = _imageContainer.RowDefinitions.Count;

				// more col in grid than new value
				// remove
				if(colCount > Columns)
				{
					for (int i = _imageContainer.ColumnDefinitions.Count - 1; i >= Columns; i--)
					{
						_imageContainer.ColumnDefinitions.RemoveAt(i);
					}
				}
				// less col in grid than new value
				// adding new ones
				else if (colCount < Columns)
				{
					for (int i = 0; i < Columns - colCount; i++)
					{
						_imageContainer.ColumnDefinitions.Add(new ColumnDefinition());
					}
				}

				// more row in grid than new value
				// remove
				if (rowCount > Rows)
				{
					for (int i = _imageContainer.RowDefinitions.Count - 1; i >= Rows; i--)
					{
						_imageContainer.RowDefinitions.RemoveAt(i);
					}
				}
				// less col in grid than new value
				// adding new ones
				else if (rowCount < Rows)
				{
					for (int i = 0; i < Rows - rowCount; i++)
					{
						_imageContainer.RowDefinitions.Add(new RowDefinition());
					}
				}
			}

		}

    	public int Columns
		{
			get { return (int)GetValue(ColumnProperty); }
			set { SetValue(ColumnProperty, value); }
		}

		public static readonly DependencyProperty ColumnProperty =
			DependencyProperty.Register("Columns", typeof(int), typeof(ImageTile), new PropertyMetadata(3, OnGridSizeChanged));

		public int Rows
		{
			get { return (int)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.Register("Rows", typeof(int), typeof(ImageTile), new PropertyMetadata(3, OnGridSizeChanged));

        public List<Uri> ItemsSource
        {
            get { return (List<Uri>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
		public static readonly DependencyProperty ItemsSourceProperty = 
			DependencyProperty.Register("ItemsSource", typeof(List<Uri>), 
			typeof(ImageTile), new PropertyMetadata(null));

		public ImageTileAnimationType AnimationType
        {
			get { return (ImageTileAnimationType)GetValue(AnimationTypeProperty); }
            set { SetValue(AnimationTypeProperty, value); }
        }
		public static readonly DependencyProperty AnimationTypeProperty =
			DependencyProperty.Register("AnimationType", typeof(ImageTileAnimationType),
			typeof(ImageTile), new PropertyMetadata(null));

        public bool IsFrozen
        {
            get { return (bool)GetValue(IsFrozenProperty); }
            set { SetValue(IsFrozenProperty, value); }
        }
		public static readonly DependencyProperty IsFrozenProperty =
			DependencyProperty.Register("IsFrozen", typeof(bool?), typeof(ImageTile),
			new PropertyMetadata(OnIsFrozenPropertyChanged));

        public int AnimationInterval
        {
            get { return (int)GetValue(AnimationIntervalProperty); }
            set { SetValue(AnimationIntervalProperty, value); }
        }
		public static readonly DependencyProperty AnimationIntervalProperty =
			DependencyProperty.Register("AnimationInterval", typeof(int), typeof(ImageTile),
			new PropertyMetadata(AnimationIntervalPropertyChanged));

        private static void OnIsFrozenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ImageTile tile = dependencyObject as ImageTile;

			if (tile != null && tile._timer != null)
            {
                if (tile.IsFrozen)
                    tile._timer.Stop();
                else
                    tile._timer.Start();
            }
        }

        private static void AnimationIntervalPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ImageTile tile = dependencyObject as ImageTile;

			if (tile != null && tile._timer != null)
			{
				bool isEnabled = tile._timer.IsEnabled;
				tile._timer.Stop();

				tile._timer.Interval = TimeSpan.FromMilliseconds(tile.AnimationInterval);

				if (isEnabled)
					tile._timer.Start();
			}
        }
    }
}