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
        int _lastUsedCol = -1;
        int _lastUsedRow = -1;
        string[] imgIds = { "00", "01", "02", "10", "11", "12", "20", "21", "22" };
        Grid imageContainer = null;
        List<string> availableIds = new List<string>();

        public PeopleTile()
		{
            DefaultStyleKey = typeof(PeopleTile);
            timer.Interval = TimeSpan.FromSeconds(1);
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

                if(availableIds.Count == 0)
                    availableIds.AddRange(imgIds);

                int i = _rand.Next(0, availableIds.Count);
                id = availableIds[i];
                availableIds.RemoveAt(i);

                row = int.Parse(id.Substring(0, 1));
                col = int.Parse(id.Substring(1, 1));
                
                //while (true)
                //{
                //    col = GetNonRepeatRandomValue(0, 3, _lastUsedCol);
                //    row = GetNonRepeatRandomValue(0, 3, _lastUsedRow);

                //    id = string.Format("{0}{1}", row, col);

                //    //if (_currentlyDisplayed.Count == 9)
                //    //    break;

                //    if (!_currentlyDisplayed.ContainsKey(id))
                //        break;
                //}

                var img = new Image { Source = GetRandomImage(id) };

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
                    //var items =
                    //imageContainer.Children.Where(
                    //x => (int)x.GetValue(Grid.RowProperty) == row && (int)x.GetValue(Grid.ColumnProperty) == col).
                    //ToArray();

                    //try
                    //{
                    //    if (items.Count() > 0)
                    //    {
                    //        Image oldimg = (Image)items[0];
                    //        oldimg.Projection = new PlaneProjection();
                    //        CreateDoubleAnimations(sb, img.Projection, "RotationX", 0, 90, 500);
                    //    }
                    //}
                    //catch { }
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

            if(items.Count() > 1)
            {
                Image img = (Image)items[0];
                //_currentlyDisplayed.Remove(((BitmapImage)(img.Source)).UriSource);

                imageContainer.Children.Remove(img);
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
                tile.timer.Interval = TimeSpan.FromSeconds(tile.AnimationInterval);
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