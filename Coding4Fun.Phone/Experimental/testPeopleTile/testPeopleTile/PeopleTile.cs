using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Linq;

namespace testPeopleTile
{
    public class PeopleTile : Button
    {
        DispatcherTimer timer = new DispatcherTimer();
        Random _rand = new Random();
        List<tester> _bigList = new List<tester>();
        Dictionary<string, Uri> _currentlyDisplayed = new Dictionary<string, Uri>();
        string[] imgIds = { "00", "01", "02", "10", "11", "12", "20", "21", "22" };
        string[] largeImgIds = { "00", "01", "10", "11" };
        Grid imageContainer = null;
        List<string> availableIds = new List<string>();
        List<string> availableLargeIds = new List<string>();
        BitmapImage largeTileImage = null;
        private bool bShowLargeImage = false;
        private string lastlargeid = null;

        public PeopleTile()
		{
            DefaultStyleKey = typeof(PeopleTile);
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.imageContainer = (Grid)this.GetTemplateChild("ImageContainer");
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (this.imageContainer != null && this.ItemsSource != null && this.ItemsSource.Count > 0)
            {
                int col = -1;
                int row = -1;
                string id = null;

                //int width = (int)this.imageContainer.ActualWidth / 3;
                //int height = (int)this.imageContainer.ActualHeight / 3;

                if (availableIds.Count == 0)
                {
                    availableIds.AddRange(imgIds);

                    if (bShowLargeImage)
                    {
                        lastlargeid = largeImgIds[_rand.Next(0, largeImgIds.Length)];

                        int largeRow = int.Parse(lastlargeid.Substring(0, 1));
                        int largeCol = int.Parse(lastlargeid.Substring(1, 1));

                        availableLargeIds.Clear();

                        largeTileImage = GetRandomImage(lastlargeid);

                        for (int i = largeRow; i <= largeRow + 1; i++)
                        {
                            for (int j = largeCol; j <= largeCol + 1; j++)
                            {
                                availableLargeIds.Add(string.Format("{0}{1}", i, j));
                            }
                        }
                    }
                    
                    bShowLargeImage = !bShowLargeImage;
                }

                bool bRemoved = false;
                if (bShowLargeImage && availableIds.Count > 1)
                {
                    availableIds.Remove(lastlargeid);
                    bRemoved = true;
                }

                
                int iRand = _rand.Next(0, availableIds.Count);
                id = availableIds[iRand];

                availableIds.RemoveAt(iRand);

                if (bRemoved)
                    availableIds.Remove(lastlargeid);


                Image img = null;
                int index = availableLargeIds.IndexOf(id);
                if(!bShowLargeImage && index > -1)
                {
                    if (availableLargeIds.Count == 4)
                    {
                        id = availableLargeIds[0];
                        
                        foreach (string val in availableLargeIds)
                            availableIds.Remove(val);

                        availableLargeIds.Clear();
                
                        img = new Image() { Source = largeTileImage, RenderTransform = new ScaleTransform() { ScaleX = 2, ScaleY = 2 } };
                    }
                }
                else
                {
                    img = new Image { Source = GetRandomImage(id) };
                }

                row = int.Parse(id.Substring(0, 1));
                col = int.Parse(id.Substring(1, 1));

                var sb = new Storyboard();
                var test = new tester();

                img.SetValue(Grid.ColumnProperty, col);
                img.SetValue(Grid.RowProperty, row);

                img.HorizontalAlignment = HorizontalAlignment.Center;
                img.VerticalAlignment = VerticalAlignment.Center;
                img.Stretch = Stretch.UniformToFill;
                img.Name = Guid.NewGuid().ToString();

                imageContainer.Children.Add(img);

                test.sb = sb;
                test.Row = row;
                test.Column = col;

                _bigList.Add(test);

                if (this.AnimationType == AnimationTypeDef.Fade)
                    CreateDoubleAnimations(sb, img, "Opacity", 0, 1, 500);
                else
                {
                    img.Projection = new PlaneProjection();
                    CreateDoubleAnimations(sb, img.Projection, "RotationX", 270, 360, 500);
                }
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
            var test = sender as Storyboard;
            var result = _bigList.Where(x => x.sb == test).FirstOrDefault();

            var items =
                imageContainer.Children.Where(
                    x => (int)x.GetValue(Grid.RowProperty) == result.Row && (int)x.GetValue(Grid.ColumnProperty) == result.Column).
                    ToArray();

            for (int i = 0; i < items.Count() - 1; i++)
            {
                imageContainer.Children.Remove(items[i]);
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
            int index;
            Uri item = null;
            do
            {
                index = _rand.Next(1, this.ItemsSource.Count);

                item = this.ItemsSource[index];
            }
            while (_currentlyDisplayed.ContainsValue(item));

            _currentlyDisplayed[id] = item;

            return GetImage(item);
        }

        private static BitmapImage GetImage(Uri file)
        {
            BitmapImage retImage = null;
            try
            {
                retImage = new BitmapImage(file);
            }
            catch (Exception)
            {
                retImage = new BitmapImage(new Uri("Images/PlaceholderPhoto.png", UriKind.Relative));
            }

            return retImage;
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //}

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(List<Uri>), typeof(PeopleTile), new PropertyMetadata(null));

        public static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.Register("AnimationType", typeof(AnimationTypeDef), typeof(PeopleTile), new PropertyMetadata(null));

        public static readonly DependencyProperty IsFrozenProperty = DependencyProperty.Register("IsFrozen", typeof(bool?), typeof(PeopleTile), new PropertyMetadata(OnIsFrozenPropertyChanged));

        public static readonly DependencyProperty AnimationIntervalProperty = DependencyProperty.Register("AnimationInterval", typeof(int), typeof(PeopleTile), new PropertyMetadata(AnimationIntervalPropertyChanged));

        public List<Uri> ItemsSource
        {
            get { return (List<Uri>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public AnimationTypeDef AnimationType
        {
            get { return (AnimationTypeDef)GetValue(AnimationTypeProperty); }
            set { SetValue(AnimationTypeProperty, value); }
        }

        public bool IsFrozen
        {
            get { return (bool)GetValue(IsFrozenProperty); }
            set { SetValue(IsFrozenProperty, value); }
        }

        public int AnimationInterval
        {
            get { return (int)GetValue(AnimationIntervalProperty); }
            set { SetValue(AnimationIntervalProperty, value); }
        }

        private static void OnIsFrozenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            PeopleTile tile = dependencyObject as PeopleTile;

            if (tile != null)
            {
                if (tile.IsFrozen)
                    tile.timer.Stop();
                else
                    tile.timer.Start();

                //spinnerControl.Visibility = spinnerControl.IsSpinning ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void AnimationIntervalPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            PeopleTile tile = dependencyObject as PeopleTile;

            if (tile != null)
            {
                bool bRunning = tile.timer.IsEnabled;
                tile.timer.Stop();

                tile.timer.Interval = TimeSpan.FromMilliseconds(tile.AnimationInterval);

                if (bRunning)
                    tile.timer.Start();
            }
        }
    }

    public enum AnimationTypeDef
    {
        Fade,
        Flip,
    }

    public struct tester
    {
        public Storyboard sb { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}