using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Phone.Controls
{
	public class ImageTile : Button
	{
		readonly DispatcherTimer _changeImageTimer = new DispatcherTimer();
		readonly Random _rand = new Random();

		readonly Dictionary<int, Uri> _imageCurrentLocation = new Dictionary<int, Uri>();

		readonly List<Uri> _imagesBeingShown = new List<Uri>();
		readonly List<int> _availableSpotsOnGrid = new List<int>();
		readonly List<ImageTileState> _animationTracking = new List<ImageTileState>();

		private int _largeImageIndex = -1;		
		private int _lastIdRow = -1;
		private int _lastIdCol = -1;
		private bool _createAnimation = true;

		ImageTileLayoutStates _imageTileLayoutState = ImageTileLayoutStates.Unknown;

		Grid _imageContainer;

		public event EventHandler<ExceptionRoutedEventArgs> ImageFailed;

        public ImageTile()
		{
            DefaultStyleKey = typeof(ImageTile);
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageContainer = (Grid)GetTemplateChild("ImageContainer");

			GridSizeChanged();
			ResetGridStateManagement();

			_createAnimation = false;

			if (!DesignerProperties.IsInDesignTool)
			{
				while (_availableSpotsOnGrid.Any())
					CycleImage();
			}
	        _createAnimation = true;

			ImageCycleIntervalChanged();
			IsFrozenPropertyChanged();

			_changeImageTimer.Tick += ChangeImageTimerTick;
        }

		private int CalculateIndex(int row, int col)
		{
			return row * (Columns) + col;
		}

        void ChangeImageTimerTick(object sender, EventArgs e)
        {
	        CycleImage();
        }

	    public void CycleImage()
	    {
		    if (_imageContainer == null || ItemsSource == null || ItemsSource.Count <= 0)
			    return;

		    int index;
			int row;
			int col;
			bool isLargeImage;

		    CalculateNextValidItem(out index, out row, out col, out isLargeImage);

			var img = CreateImage(row, col, index, isLargeImage);

			_imageContainer.Children.Add(img);

			if (_createAnimation && AnimationType != ImageTileAnimationTypes.None)
			{
				var sb = new Storyboard();
				TrackAnimationForImageRemoval(row, col, sb, isLargeImage);

				switch (AnimationType)
				{
					case
						ImageTileAnimationTypes.Fade:
						CreateDoubleAnimations(sb, img, "Opacity", 0, 1, AnimationDuration);
						break;
					case ImageTileAnimationTypes.HorizontalExpand:
						img.Projection = new PlaneProjection();
						CreateDoubleAnimations(sb, img.Projection, "RotationY", 270, 360, AnimationDuration);
						break;
					case ImageTileAnimationTypes.VerticalExpand:
						img.Projection = new PlaneProjection();
						CreateDoubleAnimations(sb, img.Projection, "RotationX", 270, 360, AnimationDuration);
						break;
				}

				sb.Completed += AnimationCompleted;
				sb.Begin();
			}
	    }

        private void CalculateNextValidItem(out int index, out int row, out int col, out bool isLargeImage)
        {
            isLargeImage = false;

            if (_availableSpotsOnGrid.Count == 0)
            {
                ResetGridStateManagement();

                isLargeImage = _imageTileLayoutState == ImageTileLayoutStates.BigImage;
            }

            var lastIndex = -1;

            do
            {
                //Capture the numbers we'll feed to the random number generator so that we can ensure that they will be 
                //  non-negative (only matters if the grid is 0xN or Mx0 for some reason)
				
				// random numbers are calc between low and high bound, not including high.
				// if Rows = 3 and LargeTileRows = 2, valid spots are 0, 1
				// this means Rows - LargeTileRows = 3 - 2 = 1
 				// Since we want a value of 0 or 1, we need to add 1
                var rowMax = !isLargeImage ? Rows : Rows - LargeTileRows + 1;
				var colMax = !isLargeImage ? Columns : Columns - LargeTileColumns + 1;

                row = _rand.Next(rowMax >= 0 ? rowMax : 0);
                col = _rand.Next(colMax >= 0 ? colMax : 0);

                index = CalculateIndex(row, col);

                //If we've got the same number twice in a row, break out of the loop and assign the image
                //  let the available slots be recalc'd on the next run through
                if (lastIndex == index)
                {
                    break;
                }

                lastIndex = index;
            } while
                (
                // same spot as last image
                // is the spot open on the grid
                // is this a valid spot
                // if a large image // valid spot
                //maxLoopCounter < 5 &&
                    (
                //(_showNewLargeImage && _largeImageIndex == index) || 
                //(!_showNewLargeImage && _lastIndex == index) &&

                        // repeat if
                // index isn't there
                //  and there is at least one available spot in the grid
                        !_availableSpotsOnGrid.Contains(index) && _availableSpotsOnGrid.Count > 0
                    )
                );

            _lastIdRow = row;
            _lastIdCol = col;

            if (isLargeImage)
            {
                _largeImageIndex = index;

                for (int i = 0; i < LargeTileRows; i++)
                    for (int j = 0; j < LargeTileColumns; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;

                        _availableSpotsOnGrid.Remove(CalculateIndex(row + i, col + j));
                    }
                //_availableSpotsOnGrid.Remove(CalculateIndex(row, col + 1));
                //_availableSpotsOnGrid.Remove(CalculateIndex(row + 1, col));
                //_availableSpotsOnGrid.Remove(CalculateIndex(row + 1, col + 1));
            }

            _availableSpotsOnGrid.Remove(index);
        }

	    private void ResetGridStateManagement()
    	{
    		// first run or when available positions have been filled
    		if (_availableSpotsOnGrid.Count != 0) 
				return;

			AlterCycleState();

			if (_imageTileLayoutState == ImageTileLayoutStates.ForceOverwriteOfBigImage)
			{
				// we want to force an over write on top of the large tile
				_availableSpotsOnGrid.Add(_largeImageIndex);

				return;
			}

			for (int row = 0; row < Rows; row++)
			{
				for (int col = 0; col < Columns; col++)
				{
					_availableSpotsOnGrid.Add(CalculateIndex(row, col));
				}
			}

			if (_imageTileLayoutState == ImageTileLayoutStates.AllButBigImage)
		    {
			    _availableSpotsOnGrid.Remove(_largeImageIndex);
		    }
    	}

		private void AlterCycleState()
		{
			switch (_imageTileLayoutState)
			{
				case ImageTileLayoutStates.AllImages:
					_imageTileLayoutState = ImageTileLayoutStates.BigImage;
					break;
				case ImageTileLayoutStates.BigImage:
					_imageTileLayoutState = ImageTileLayoutStates.AllButBigImage;
					break;
				case ImageTileLayoutStates.AllButBigImage:
					_imageTileLayoutState = ImageTileLayoutStates.ForceOverwriteOfBigImage;
					break;
				default:
					_imageTileLayoutState = ImageTileLayoutStates.AllImages;
					break;
			}

		}

    	private Image CreateImage(int row, int col, int index, bool isLargeImage)
    	{
            var imgUri = GetRandomImageUri(index);
			var img = new Image
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Stretch = Stretch.UniformToFill,
                                Name = Guid.NewGuid().ToString()
                            };

            img.SetValue(Grid.ColumnProperty, col);
            img.SetValue(Grid.RowProperty, row);

            if (isLargeImage)
            {
                //System.Diagnostics.Debug.WriteLine("Large Tile");
                img.SetValue(Grid.ColumnSpanProperty, LargeTileColumns);
                img.SetValue(Grid.RowSpanProperty, LargeTileRows);
            }

            img.Source = GetImage(imgUri);
            return img;
    	}

    	private void TrackAnimationForImageRemoval(int row, int col, Storyboard sb, bool forceLargeImageCleanup)
    	{
    		var tileState = new ImageTileState {Storyboard = sb, Row = row, Column = col, ForceLargeImageCleanup = forceLargeImageCleanup};

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

			//if (result == null) 
			//    return;

			if (result.ForceLargeImageCleanup)
			{
				// TODO make this configurable
				// removing other spots so it doesn't have stuff on top of it right away

                for(int i = 0; i < LargeTileRows; i++)
                    for (int j = 0; j < LargeTileColumns; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;

                        RemoveOldImagesFromGrid(i, _lastIdCol + j);
                    }

                //RemoveOldImagesFromGrid(_lastIdRow, _lastIdCol + 1);
                //RemoveOldImagesFromGrid(_lastIdRow + 1, _lastIdCol);
                //RemoveOldImagesFromGrid(_lastIdRow + 1, _lastIdCol + 1);
			}

	        RemoveOldImagesFromGrid(result.Row, result.Column);

	        _animationTracking.Remove(result);
        }

    	private void RemoveOldImagesFromGrid(int row, int col)
    	{
    		var items =
    			_imageContainer.Children.Where(
    				x => (int) x.GetValue(Grid.RowProperty) == row
						&& (int) x.GetValue(Grid.ColumnProperty) == col).
    				ToArray();

    		for (int i = 0; i < items.Count() - 1; i++)
    		{
    			var img = items[i] as Image;

    			if (img == null)
    				continue;

    			var bitmapImage = img.Source as BitmapImage;

    			if (bitmapImage != null)
    			{
    				var imgSource = bitmapImage.UriSource;
					_imagesBeingShown.Remove(imgSource);
    			}

    			_imageContainer.Children.Remove(img);
    		}
    	}

    	private Uri GetRandomImageUri(int index)
        {
        	Uri imgUri;

			var count = ItemsSource.Count;

			int maxLoopCounter = 0;
			do
			{
				imgUri = ItemsSource[_rand.Next(count)];
				maxLoopCounter++;
			}
			while (
				maxLoopCounter < 5 &&
				// is it currently being shown?
				_imageCurrentLocation.ContainsValue(imgUri)
				||
				(
					
					_imagesBeingShown.Count < count &&
					_imagesBeingShown.Contains(imgUri)
				));	// if does, are we out of items, if so, allow it to slide, 

			_imageCurrentLocation[index] = imgUri;
			_imagesBeingShown.Add(imgUri);

            return imgUri;
        }

        private BitmapImage GetImage(Uri file)
        {
	        var img = new BitmapImage(file);
			img.ImageFailed += ImageLoadFail;

            return img;
        }

		void ImageLoadFail(object sender, ExceptionRoutedEventArgs e)
		{
			if (ImageFailed != null)
				ImageFailed.Invoke(sender, e);
		}

		#region dependency properties
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

        public int LargeTileColumns
        {
            get { return (int)GetValue(LargeTileColumnsProperty); }
			set { SetValue(LargeTileColumnsProperty, value); }
        }

        public static readonly DependencyProperty LargeTileColumnsProperty =
            DependencyProperty.Register("LargeTileColumns", typeof(int), typeof(ImageTile), new PropertyMetadata(2, OnLargeTileChanged));

        public int LargeTileRows
        {
            get { return (int)GetValue(LargeTileRowsProperty); }
			set { SetValue(LargeTileRowsProperty, value); }
        }

        public static readonly DependencyProperty LargeTileRowsProperty =
			DependencyProperty.Register("LargeTileRows", typeof(int), typeof(ImageTile), new PropertyMetadata(2, OnLargeTileChanged));

        public List<Uri> ItemsSource
        {
            get { return (List<Uri>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
		public static readonly DependencyProperty ItemsSourceProperty = 
			DependencyProperty.Register("ItemsSource", typeof(List<Uri>), 
			typeof(ImageTile), new PropertyMetadata(null));

		public ImageTileAnimationTypes AnimationType
        {
			get { return (ImageTileAnimationTypes)GetValue(AnimationTypesProperty); }
            set { SetValue(AnimationTypesProperty, value); }
        }
		public static readonly DependencyProperty AnimationTypesProperty =
			DependencyProperty.Register("AnimationType", typeof(ImageTileAnimationTypes),
			typeof(ImageTile), new PropertyMetadata(null));

        public bool IsFrozen
        {
            get { return (bool)GetValue(IsFrozenProperty); }
            set { SetValue(IsFrozenProperty, value); }
        }
		public static readonly DependencyProperty IsFrozenProperty =
			DependencyProperty.Register("IsFrozen", typeof(bool), typeof(ImageTile),
			new PropertyMetadata(false, OnIsFrozenPropertyChanged));

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
			DependencyProperty.Register("ImageCycleInterval", typeof(int), typeof(ImageTile), new PropertyMetadata(1000, OnImageCycleIntervalPropertyChanged));
		#endregion

		#region dependency property callbacks
		private static void OnIsFrozenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var tile = dependencyObject as ImageTile;

        	if (tile == null || tile._changeImageTimer == null) 
				return;

        	tile.IsFrozenPropertyChanged();
        }

		private void IsFrozenPropertyChanged()
		{
			if (IsFrozen)
				_changeImageTimer.Stop();
			else
				_changeImageTimer.Start();
		}

		private static void OnImageCycleIntervalPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var tile = dependencyObject as ImageTile;

        	if (tile == null || tile._changeImageTimer == null)
				return;

        	tile.ImageCycleIntervalChanged();
        }

		private void ImageCycleIntervalChanged()
		{
			_changeImageTimer.Stop();

			_changeImageTimer.Interval = TimeSpan.FromMilliseconds(ImageCycleInterval);

			if (_changeImageTimer.IsEnabled)
				_changeImageTimer.Start();
		}

		private static void OnLargeTileChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var tile = dependencyObject as ImageTile;

			if (tile != null && args.NewValue != args.OldValue)
			{
				tile.VerifyGridBounds();
			}
		}

		private static void OnGridSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var tile = dependencyObject as ImageTile;

			if (tile != null && args.NewValue != args.OldValue)
			{
				tile.VerifyGridBounds();
				tile.GridSizeChanged();
			}
		}

		private void GridSizeChanged()
		{
			if (_imageContainer == null)
				return;

			int colCount = _imageContainer.ColumnDefinitions.Count;
			int rowCount = _imageContainer.RowDefinitions.Count;

			// more col in grid than new value
			// remove
			if (colCount > Columns)
			{
				for (int i = colCount - 1; i >= Columns; i--)
				{
					_imageContainer.ColumnDefinitions.RemoveAt(i);

					KeepGridInSyncCol(i);
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
				for (int i = rowCount - 1; i >= Rows; i--)
				{
					_imageContainer.RowDefinitions.RemoveAt(i);

					KeepGridInSyncRow(i);
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

		private void VerifyGridBounds()
		{
			if (Rows < 1)
			{
				//Rows = 1;
				throw new ArgumentOutOfRangeException("Rows", "Rows must be greater than 0");
			}

			if (Columns < 1)
			{
				//Columns = 1;
				throw new ArgumentOutOfRangeException("Columns", "Columns must be greater than 0");
			}

			if (LargeTileRows < 1)
			{
				//LargeTileRows = 1;
				throw new ArgumentOutOfRangeException("LargeTileRows", "LargeTileRows must be greater than 0");
			}
			
			if (LargeTileRows > Rows)
			{
				//LargeTileRows = Rows;
				throw new ArgumentOutOfRangeException("LargeTileRows", "LargeTileRows must be less than or equal to Rows");
			}

			if (LargeTileColumns < 1)
			{
				//LargeTileColumns = 1;
				throw new ArgumentOutOfRangeException("LargeTileColumns", "LargeTileColumns must be greater than 0");
			}
			
			if (LargeTileColumns > Columns)
			{
				//LargeTileColumns = Columns;
				throw new ArgumentOutOfRangeException("LargeTileColumns", "LargeTileColumns must be less than or equal to Columns");
			}
		}

		private void KeepGridInSyncRow(int row)
		{
			for (int col = 0; col < Columns; col++)
			{
				KeepGridInSync(row, col);
			}
		}

		private void KeepGridInSyncCol(int col)
		{
			for (int row = 0; row < Rows; row++)
			{
				KeepGridInSync(row, col);
			}
		}

		private void KeepGridInSync(int row, int col)
		{
			var index = CalculateIndex(row, col);

			Uri imgUri;

			if (_imageCurrentLocation.TryGetValue(index, out imgUri))
			{
				_imagesBeingShown.Remove(imgUri);
				_imageCurrentLocation.Remove(index);
			}

			_availableSpotsOnGrid.Remove(index);
		}
		#endregion
	}
}