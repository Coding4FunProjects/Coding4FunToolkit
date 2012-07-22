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
		
		List<string> _availableIds = new List<string>();
		List<string> _availableLargeIds = new List<string>();
		
		readonly string[] _imgIds = { "00", "01", "02", "10", "11", "12", "20", "21", "22" };
    	readonly string[] _largeImgIds = { "00", "01", "10", "11" };
        
		Grid _imageContainer;
        BitmapImage _largeTileImage;

        private bool _showLargeImage;
        private string _lastLargeId;

        public ImageTile()
		{
            DefaultStyleKey = typeof(ImageTile);

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageContainer = (Grid)GetTemplateChild("ImageContainer");

			GridSizeChanged();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (_imageContainer != null && ItemsSource != null && ItemsSource.Count > 0)
            {
                int col = -1;
                int row = -1;
                string id = null;

                if (_availableIds.Count == 0)
                {
                    _availableIds.AddRange(_imgIds);

                    if (_showLargeImage)
                    {
                        string largeid = null;
                        while (true)
                        {
                            largeid = _largeImgIds[_rand.Next(0, _largeImgIds.Length)];
                            if(largeid != _lastLargeId)
                                break;
                        }

                        _lastLargeId = largeid;

                        int largeRow = int.Parse(_lastLargeId.Substring(0, 1));
                        int largeCol = int.Parse(_lastLargeId.Substring(1, 1));

                        _availableLargeIds.Clear();

                        _largeTileImage = GetRandomImage(_lastLargeId);

                        for (int i = largeRow; i <= largeRow + 1; i++)
                        {
                            for (int j = largeCol; j <= largeCol + 1; j++)
                            {
                                _availableLargeIds.Add(string.Format("{0}{1}", i, j));
                            }
                        }
                    }
                    
                    _showLargeImage = !_showLargeImage;
                }

                bool hasImageRemoved = false;

                if (_showLargeImage && _availableIds.Count > 1)
                {
                    _availableIds.Remove(_lastLargeId);
                    hasImageRemoved = true;
                }
                
                int iRand = _rand.Next(0, _availableIds.Count);
                id = _availableIds[iRand];

                _availableIds.RemoveAt(iRand);

                if (hasImageRemoved)
                    _availableIds.Remove(_lastLargeId);

                Image img = null;
                int index = _availableLargeIds.IndexOf(id);

                if(!_showLargeImage && index > -1)
                {
                    if (_availableLargeIds.Count == 4)
                    {
                        id = _availableLargeIds[0];
                        
                        foreach (string val in _availableLargeIds)
                            _availableIds.Remove(val);

                        _availableLargeIds.Clear();
                
                        img = new Image 
						{ 
							Source = _largeTileImage, 
							//RenderTransform = new ScaleTransform { ScaleX = 2, ScaleY = 2 } 
						};

						img.SetValue(Grid.ColumnSpanProperty, 2);
						img.SetValue(Grid.RowSpanProperty, 2);
                    }
                }
                else
                {
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
						case ImageTileAnimationType.Flip:
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
                _imageContainer.Children.Remove(items[i]);
            }
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


        private BitmapImage GetRandomImage(string id)
        {
        	Uri item;

            do
            {
            	int index = _rand.Next(0, ItemsSource.Count);

            	item = ItemsSource[index];
            } while (_currentlyDisplayed.ContainsValue(item));

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