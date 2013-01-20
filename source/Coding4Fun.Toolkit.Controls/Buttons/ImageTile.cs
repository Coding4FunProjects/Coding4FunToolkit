using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Coding4Fun.Toolkit.Controls.Common;

#if WINDOWS_STORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

#endif

namespace Coding4Fun.Toolkit.Controls
{
	public class ImageTile : ButtonBase, IDisposable
	{
		readonly DispatcherTimer _changeImageTimer = new DispatcherTimer();
		readonly Random _rand = new Random();

		readonly Dictionary<int, Uri> _imageCurrentLocation = new Dictionary<int, Uri>();

		readonly List<Uri> _imagesBeingShown = new List<Uri>();
		readonly List<int> _availableSpotsOnGrid = new List<int>();
		readonly List<ImageTileState> _animationTracking = new List<ImageTileState>();

		private int _largeImageIndex = -1;
		private bool _createAnimation = true;

		ImageTileLayoutStates _imageTileLayoutState = ImageTileLayoutStates.Unknown;

		Grid _imageContainer;

		public event EventHandler<ExceptionRoutedEventArgs> ImageFailed;

        public ImageTile()
		{
            DefaultStyleKey = typeof(ImageTile);
		}

#if WINDOWS_STORE
		protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            _imageContainer = (Grid)GetTemplateChild("ImageContainer");

			GridSizeChanged();
			ResetGridStateManagement();

			_createAnimation = false;

			if (!DevelopmentHelpers.IsDesignMode)
			{
				for (var i = 0; i < Rows; i++)
				{
					for (var j = 0; j < Columns; j++)
					{
						CycleImage(i, j);
					}
				}

#if WINDOWS_STORE
				var rootFrame = Window.Current.Content as Frame;
#elif WINDOWS_PHONE
				var rootFrame = Application.Current.RootVisual as Frame;
#endif

				if (rootFrame != null)
					rootFrame.Navigated += FrameNavigated;
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

#if WINDOWS_STORE
		void ChangeImageTimerTick(object sender, object e)
#elif WINDOWS_PHONE
		void ChangeImageTimerTick(object sender, EventArgs e)
#endif
		{
	        CycleImage();
        }

	    public void CycleImage(int row = -1, int col = -1)
	    {
		    if (_imageContainer == null || ItemsSource == null || ItemsSource.Count <= 0)
			    return;

		    int index;
			bool isLargeImage;

		    CalculateNextValidItem(out index, ref row, ref col, out isLargeImage);

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
						ControlHelper.CreateDoubleAnimations(sb, img, "Opacity", 0, 1, AnimationDuration);
						break;
					case ImageTileAnimationTypes.HorizontalExpand:
						img.Projection = new PlaneProjection();
						ControlHelper.CreateDoubleAnimations(sb, img.Projection, "RotationY", 270, 360, AnimationDuration);
						break;
					case ImageTileAnimationTypes.VerticalExpand:
						img.Projection = new PlaneProjection();
						ControlHelper.CreateDoubleAnimations(sb, img.Projection, "RotationX", 270, 360, AnimationDuration);
						break;
				}

				sb.Completed += AnimationCompleted;
				sb.Begin();
			}
	    }


        private void CalculateNextValidItem(out int index, ref int row, ref int col, out bool isLargeImage)
        {
            isLargeImage = false;

            if (row == -1 && col == -1)
            {
                if (_availableSpotsOnGrid.Count == 0)
                {
                    ResetGridStateManagement();

                    isLargeImage = _imageTileLayoutState == ImageTileLayoutStates.BigImage;
                }

                var largeTileSpotCandidates = _availableSpotsOnGrid.Where(IsValidLargeTilePosition).ToList();
                var selectionSet = isLargeImage ? largeTileSpotCandidates : _availableSpotsOnGrid;
                var location = _rand.Next(0, selectionSet.Count);

                index = selectionSet[location];
                GetRowAndColumnForIndex(index, out row, out col);
            }
            else
            {
                index = CalculateIndex(row, col);
            }

            if (isLargeImage)
            {
                _largeImageIndex = index;

                for (var i = 0; i < LargeTileRows; ++i)
                {
                    for (var j = 0; j < LargeTileColumns; ++j)
                    {
                        _availableSpotsOnGrid.Remove(CalculateIndex(row + i, col + j));
                    }
                }
            }
            else
            {
                _availableSpotsOnGrid.Remove(index);
            }

			Debug.WriteLine(_availableSpotsOnGrid.Count);
        }

        private bool IsValidLargeTilePosition(int index)
        {
            int row, column;
            GetRowAndColumnForIndex(index, out row, out column);

            return column <= Columns - LargeTileColumns && row <= Rows - LargeTileRows;
        }
        
	    private void ResetGridStateManagement()
    	{
    		// first run or when available positions have been filled
    		if (_availableSpotsOnGrid.Count != 0) 
				return;

			var currentTimerState = _changeImageTimer.IsEnabled;
			_changeImageTimer.Stop();
			
			AlterCycleState();

			if (_imageTileLayoutState == ImageTileLayoutStates.ForceOverwriteOfBigImage)
			{
				_availableSpotsOnGrid.Add(_largeImageIndex);
			}
			else
			{
			    var spotCount = Rows*Columns;

                for (var i = 0; i < spotCount; ++i)
                {
                    _availableSpotsOnGrid.Add(i);
                }

				if (_imageTileLayoutState == ImageTileLayoutStates.AllButBigImage)
				{
					_availableSpotsOnGrid.Remove(_largeImageIndex);
				}
			}

			Debug.WriteLine(_imageTileLayoutState);
			Debug.WriteLine(_availableSpotsOnGrid.Count);

			if (currentTimerState)
				_changeImageTimer.Start();
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
                                Name = Guid.NewGuid().ToString(),
								//Opacity = .5
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
    		var tileState = new ImageTileState 
			{
				Storyboard = sb, 
				Row = row, 
				Column = col, 
				ForceLargeImageCleanup = forceLargeImageCleanup
			};

    		_animationTracking.Add(tileState);
    	}

#if WINDOWS_STORE
		void AnimationCompleted(object sender, object e)
#elif WINDOWS_PHONE
		void AnimationCompleted(object sender, EventArgs e)
#endif
        {
            var itemStoryboard = sender as Storyboard;

            var result = _animationTracking.FirstOrDefault(x => x.Storyboard == itemStoryboard);

			if (result.ForceLargeImageCleanup)
			{
				for (int i = 0; i < LargeTileRows; i++)
				{
					for (int j = 0; j < LargeTileColumns; j++)
					{
						if (i == 0 && j == 0)
							continue;

						RemoveOldImagesFromGrid(i + result.Row, j + result.Column, true);
					}
				}
			}

	        RemoveOldImagesFromGrid(result.Row, result.Column);

	        _animationTracking.Remove(result);
        }

    	private void RemoveOldImagesFromGrid(int row, int col, bool forceRemoval = false)
    	{
    		var items =
    			_imageContainer.Children.Where(
    				x => (int) x.GetValue(Grid.RowProperty) == row
						&& (int) x.GetValue(Grid.ColumnProperty) == col).
    				ToArray();

			var offset = forceRemoval ? 0 : 1;

			for (int i = 0; i < items.Length - offset; i++)
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

			var imageSourceCount = ItemsSource.Count;
			var maxAvailableSlots = Rows * Columns;
			int maxLoopCounter = 0;

			do
			{
				imgUri = ItemsSource[_rand.Next(imageSourceCount)];
				maxLoopCounter++;
			}
			while (AllowRandomImageFetchToContinue(
						index,
						maxAvailableSlots, 
						imageSourceCount, 
						maxLoopCounter, 
						imgUri));	// if does, are we out of items, if so, allow it to slide, 

			_imageCurrentLocation[index] = imgUri;
			_imagesBeingShown.Add(imgUri);

            return imgUri;
        }

        private void GetRowAndColumnForIndex(int index, out int row, out int column)
        {
            column = index%Columns;
            row = (index - column)/Rows;
        }

		private bool AllowRandomImageFetchToContinue(int targetIndex, int maxAvailableSlots, int imageSourceCount, int maxLoopCounter, Uri imgUri)
		{
			// this is in a do while loop, 
			// true == another pass
			// false == continue with work

			var tooManyRevs = maxLoopCounter >= 10;			

			// too many passes, hard stop
			if (tooManyRevs)
				return false;

			var moreSlotsThanImages = imageSourceCount <= maxAvailableSlots;

			if (moreSlotsThanImages)
			{
				var imageAtIndexIsTheSame = _imageCurrentLocation.ContainsKey(targetIndex) && _imageCurrentLocation[targetIndex] == imgUri;

				// keep going if same image at same location
				if (imageAtIndexIsTheSame)
					return true;

				// image source could have changed
				// so if the image is being shown already, 
				// we'll do another pass until we hit max looping
				var imageBeingShown = _imagesBeingShown.Contains(imgUri);

				return imageBeingShown;
			}

			var containsImageAtSlot = _imageCurrentLocation.ContainsValue(imgUri);

			// finally, if there is that image on the board ...
			return containsImageAtSlot;
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
			var currentState = _changeImageTimer.IsEnabled;

			_changeImageTimer.Stop();

			_changeImageTimer.Interval = TimeSpan.FromMilliseconds(ImageCycleInterval);

			if (currentState)
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

		public void Dispose()
		{
			_changeImageTimer.Stop();
		}

		void FrameNavigated(object sender, NavigationEventArgs e)
		{
			Dispose();
		}
	}
}