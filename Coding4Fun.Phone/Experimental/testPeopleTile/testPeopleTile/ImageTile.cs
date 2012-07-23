using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace testPeopleTile
{
    public class ImageTile : Button
    {
    	DispatcherTimer _timer = new DispatcherTimer();
        Random _rand = new Random();
    	List<ImageTileState> _animationTracking = new List<ImageTileState>();
    	
		Dictionary<string, Uri> _currentlyDisplayed = new Dictionary<string, Uri>();
		
		List<string> _availableSpotsOnGrid = new List<string>();
		private string _largeImageId;
		private string _lastId;

		Grid _imageContainer;

        private bool _isLargeImageShowing = true;
		private bool _showNewLargeImage = true;
		private bool _forceOverwriteOfLargeImageOnNextIteration = false;

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
        }

        void timer_Tick(object sender, EventArgs e)
        {
        	if (_imageContainer == null || ItemsSource == null || ItemsSource.Count <= 0)
				return;

        	ResetGridStateManagement();

        	string id;
        	// allow one cycle before large image gets overwritten
        	// prevent new cycle and same image being used two times in a row
        	do
        	{
        		id = _availableSpotsOnGrid[_rand.Next(0, _availableSpotsOnGrid.Count)];
        	}
			while (_lastId == id || (_showNewLargeImage && _largeImageId == id && _availableSpotsOnGrid.Count > 1));

        	_lastId = id;

        	int row = int.Parse(id.Substring(0, 1));
        	int col = int.Parse(id.Substring(1, 1));

        	var img = CreateImage(row, col);

        	img.Source = GetRandomImage(id);
        	_availableSpotsOnGrid.Remove(id);

        	// first valid large image
        	if (!_isLargeImageShowing &&
        	    row != (Rows - 1) &&
        	    col != (Columns - 1))
        	{
        		_largeImageId = id;

        		img.SetValue(Grid.ColumnSpanProperty, 2);
        		img.SetValue(Grid.RowSpanProperty, 2);

        		// removing other spots so it doesn't have stuff on top of it right away
        		CleanUpLargeImageData(row, col + 1);
        		CleanUpLargeImageData(row + 1, col);
        		CleanUpLargeImageData(row + 1, col + 1);

        		_isLargeImageShowing = true;
        	}

        	var sb = new Storyboard();
        	TrackAnimationForImageRemoval(col, row, sb);

        	switch (AnimationType)
        	{
        		case
        			ImageTileAnimationType.Fade:
        			CreateDoubleAnimations(sb, img, "Opacity", 0, 1, AnimationDuration);
        			break;
        		case ImageTileAnimationType.HorizontalExpand:
        			img.Projection = new PlaneProjection();
        			CreateDoubleAnimations(sb, img.Projection, "RotationY", 270, 360, AnimationDuration);
        			break;
        		case ImageTileAnimationType.VerticalExpand:
        			img.Projection = new PlaneProjection();
        			CreateDoubleAnimations(sb, img.Projection, "RotationX", 270, 360, AnimationDuration);
        			break;
        	}

        	_imageContainer.Children.Add(img);

        	sb.Begin();
        	sb.Completed += AnimationCompleted;
        }

    	private void ResetGridStateManagement()
    	{
    		// first run or when available positons have been filled
    		if (_availableSpotsOnGrid.Count != 0) 
				return;

    		if (_forceOverwriteOfLargeImageOnNextIteration)
    		{
    			_availableSpotsOnGrid.Add(_largeImageId);
    			_forceOverwriteOfLargeImageOnNextIteration = false;
    		}
    		else
    		{
    			// iterate through the rows and columns and create list of availble position for 1x1 images
    			for (int i = 0; i < Rows; i++)
    			{
    				for (int j = 0; j < Columns; j++)
    				{
    					_availableSpotsOnGrid.Add(string.Format("{0}{1}", i, j));
    				}
    			}

    			// allows us to go 2 cylces of images before showing another large image
    			// do we want to make this a user setting?
    			_showNewLargeImage = !_showNewLargeImage;

    			if (_showNewLargeImage)
    			{
    				_isLargeImageShowing = false;
    			}
    			else
    			{
    				_availableSpotsOnGrid.Remove(_largeImageId);

    				if (_largeImageId != null)
    					_forceOverwriteOfLargeImageOnNextIteration = true;
    			}
    		}
    	}

    	private void CleanUpLargeImageData(int row, int col)
    	{
    		_availableSpotsOnGrid.Remove(string.Format("{0}{1}", row, col));
    		RemoveOldImagesFromGrid(row, col);
    	}

    	private static Image CreateImage(int row, int col)
    	{
    		var img = new Image
    		            	{
    		            		HorizontalAlignment = HorizontalAlignment.Center,
    		            		VerticalAlignment = VerticalAlignment.Center,
    		            		Stretch = Stretch.UniformToFill,
    		            		Name = Guid.NewGuid().ToString()
    		            	};

			img.SetValue(Grid.ColumnProperty, col);
			img.SetValue(Grid.RowProperty, row);

    		return img;
    	}

    	private void TrackAnimationForImageRemoval(int col, int row, Storyboard sb)
    	{
    		var tileState = new ImageTileState {Storyboard = sb, Row = row, Column = col};

    		_animationTracking.Add(tileState);
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

        void AnimationCompleted(object sender, EventArgs e)
        {
            var itemStoryboard = sender as Storyboard;
            var result = _animationTracking.FirstOrDefault(x => x.Storyboard == itemStoryboard);

            RemoveOldImagesFromGrid(result.Row, result.Column);

        	_animationTracking.Remove(result);
        }

    	private void RemoveOldImagesFromGrid(int row, int column)
    	{
    		var items =
    			_imageContainer.Children.Where(
    				x => (int) x.GetValue(Grid.RowProperty) == row 
						&& (int) x.GetValue(Grid.ColumnProperty) == column).
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
			while (
				_currentlyDisplayed.ContainsValue(item) &&	// does it contain it already
				_currentlyDisplayed.Count < ItemsSource.Count);	// if does, are we out of items, if so, allow it to slide, 

            _currentlyDisplayed[id] = item;

            return GetImage(item);
        }

        private static BitmapImage GetImage(Uri file)
        {
            BitmapImage img;

            try
            {
                img = new BitmapImage(file);
				img.ImageFailed += ImageLoadFail;
            }
            catch (Exception)
            {
                img = new BitmapImage(new Uri("Images/PlaceholderPhoto.png", UriKind.Relative));
            }

            return img;
        }

		static void ImageLoadFail(object sender, ExceptionRoutedEventArgs e)
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
			if (_imageContainer == null)
				return;

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

        public int AnimationDuration
        {
            get { return (int)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }
		public static readonly DependencyProperty AnimationDurationProperty =
			DependencyProperty.Register("AnimationDuration", typeof(int), typeof(ImageTile),
			new PropertyMetadata(500));

		public int ImageCycleInterval
		{
			get { return (int)GetValue(ImageCycleIntervalProperty); }
			set { SetValue(ImageCycleIntervalProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ImageCycleInterval.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ImageCycleIntervalProperty =
			DependencyProperty.Register("ImageCycleInterval", typeof(int), typeof(ImageTile), new PropertyMetadata(1000, ImageCycleIntervalPropertyChanged));

        private static void OnIsFrozenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ImageTile tile = dependencyObject as ImageTile;

        	if (tile == null || tile._timer == null) 
				return;

        	if (tile.IsFrozen)
        		tile._timer.Stop();
        	else
        		tile._timer.Start();
        }

        private static void ImageCycleIntervalPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ImageTile tile = dependencyObject as ImageTile;

        	if (tile == null || tile._timer == null)
				return;

        	bool isEnabled = tile._timer.IsEnabled;
        	tile._timer.Stop();

        	tile._timer.Interval = TimeSpan.FromMilliseconds(tile.ImageCycleInterval);

        	if (isEnabled)
        		tile._timer.Start();
        }
    }
}