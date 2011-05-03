using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using SilverlightColorPicker;

namespace Coding4Fun.Phone.Controls
{
    public class ColorHexagonPicker : ColorBaseControl
    {
        Rectangle _focusedRectangle;
        bool _layoutGenerated;

        public ColorHexagonPicker()
        {
            DefaultStyleKey = typeof(ColorHexagonPicker);

            Loaded += ColorHexagonPicker_Loaded;
        }

        void ColorHexagonPicker_Loaded(object sender, RoutedEventArgs e)
        {
            _layoutGenerated = true;

            GenerateLayout();
        }

        public void GenerateLayout()
        {
            if (!_layoutGenerated)
                return;

            var totalSteps = ColorBrightnessSteps + ColorDarknessSteps;
            
            GreyScaleBody = null;
            ColorBody = null;

            if (totalSteps > 0)
            {
                var colorHexBody = new StackPanel();

                #region building color hexagon grid

                // top
                for (var i = 0; i < totalSteps; i++)
                {
                    var items = totalSteps + i + 1;

                    colorHexBody.Children.Add(CreateChildren(items, CalculateOffsetForY(totalSteps, i)));
                }

                // middle
                colorHexBody.Children.Add(CreateChildren(totalSteps * 2 + 1, 0));

                // bottom
                for (var i = totalSteps - 1; i >= 0; i--)
                {
                    var items = totalSteps + i + 1;

                    colorHexBody.Children.Add(CreateChildren(items, -CalculateOffsetForY(totalSteps, i)));
                }
                #endregion
                ColorBody = colorHexBody;
            }

            if (GreyScaleSteps > 0)
            {
                var greyScaleHexBody = new StackPanel();
                greyScaleHexBody.Margin = new Thickness(0, ColorSize, 0, 0);

                #region building grey scale grid

                var topGrey = CreateHorizontalStackPanel();
                var bottomGrey = CreateHorizontalStackPanel();

                var totalGreySteps = GreyScaleSteps + 2; // including pure white and pure black
                var greyStep = 255 / (double)totalGreySteps;

                for (var i = 0; i <= totalGreySteps; i++)
                {
                    var step = (byte)(greyStep * i);
                    var rect = CreateRectangle(Color.FromArgb(255, step, step, step));

                    if (i % 2 == 0)
                        topGrey.Children.Add(rect);
                    else
                        bottomGrey.Children.Add(rect);
                }

                greyScaleHexBody.Children.Add(topGrey);
                greyScaleHexBody.Children.Add(bottomGrey);
                #endregion

                GreyScaleBody = greyScaleHexBody;
            }
        }

        #region helper methods
        private double CalculateOffsetForY(int totalSteps, int i)
        {
            return ColorSize * (totalSteps - i - 1);
        }

        private double CalculateDistance(double x1, double y1)
        {
            return Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2));
        }

        private double CalculateAngle(double x1, double y1)
        {
            // assume init point is 1,0 which is red
            var angle = Math.Atan2(y1, x1) * (180 / Math.PI);
            return (angle > 0) ? angle : angle + 360;
        }

        private static StackPanel CreateHorizontalStackPanel()
        {
            var item = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal
            };

            return item;
        }

        private float CalculateStep(int steps, double distance)
        {
            return (float)(distance / ((steps) * ColorSize));
        }
        
        private StackPanel CreateChildren(int items, double yOffset)
        {
            var panel = CreateHorizontalStackPanel();
            var half = items / 2.0f;

            for (var j = 1; j <= items; j++)
            {
                var xOffset = (j - half - .5) * ColorSize;
                var distance = CalculateDistance(xOffset, yOffset);
                var angle = (float)CalculateAngle(xOffset, yOffset);
                Color color;

                if (ColorSize * ColorBrightnessSteps >= distance)
                {
                    color = ColorSpace.ConvertHsvToRgb(angle, CalculateStep(ColorBrightnessSteps, distance), 1);
                }
                else
                {
                    var max = (ColorBrightnessSteps + ColorDarknessSteps + 1) * ColorSize;
                    distance = max - distance;

                    color = ColorSpace.ConvertHsvToRgb(angle, 1, CalculateStep(ColorDarknessSteps + 1, distance));
                }
                
                panel.Children.Add(CreateRectangle(color));
            }

            return panel;
        }

        private Rectangle CreateRectangle(Color color)
        {
            var rect = new Rectangle
                       {
                           Width = ColorSize,
                           Height = ColorSize,
                           StrokeThickness = 3,
                           Stroke = new SolidColorBrush(color),
                           Fill = new SolidColorBrush(color)
                       };

            rect.MouseEnter += Rectangle_MouseEnter;

            return rect;
        }
        #endregion

        void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            var rect = sender as Rectangle;

            if (rect != null)
            {
                rect.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                if (_focusedRectangle != null && _focusedRectangle != rect)
                {
                    _focusedRectangle.Stroke = _focusedRectangle.Fill;
                }

                _focusedRectangle = rect;

                ColorChanging(((SolidColorBrush)rect.Fill).Color);
            }
        }

        #region dependancy properties
        public int ColorDarknessSteps
        {
            get { return (int)GetValue(ColorDarknessStepsProperty); }
            set { SetValue(ColorDarknessStepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorDarknessSteps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorDarknessStepsProperty =
            DependencyProperty.Register("ColorDarknessSteps", typeof(int), typeof(ColorHexagonPicker), new PropertyMetadata(2, OnLayoutDependentPropertyChanged));

        public int ColorBrightnessSteps
        {
            get { return (int)GetValue(ColorBrightnessStepsProperty); }
            set { SetValue(ColorBrightnessStepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorBrightnessSteps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBrightnessStepsProperty =
            DependencyProperty.Register("ColorBrightnessSteps", typeof(int), typeof(ColorHexagonPicker), new PropertyMetadata(4, OnLayoutDependentPropertyChanged));

        public int GreyScaleSteps
        {
            get { return (int)GetValue(GreyScaleStepsProperty); }
            set { SetValue(GreyScaleStepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GreyScaleSteps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GreyScaleStepsProperty =
            DependencyProperty.Register("GreyScaleSteps", typeof(int), typeof(ColorHexagonPicker), new PropertyMetadata(20, OnLayoutDependentPropertyChanged));

        public double ColorSize
        {
            get { return (double)GetValue(ColorSizeProperty); }
            set { SetValue(ColorSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorSizeProperty =
            DependencyProperty.Register("ColorSize", typeof(double), typeof(ColorHexagonPicker), new PropertyMetadata(24d, OnLayoutDependentPropertyChanged));

        protected object ColorBody
        {
            get { return (object)GetValue(ColorBodyProperty); }
            set { SetValue(ColorBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Body.  This enables animation, styling, binding, etc...
        protected static readonly DependencyProperty ColorBodyProperty =
            DependencyProperty.Register("ColorBody", typeof(object), typeof(ColorHexagonPicker), new PropertyMetadata(null));

        protected object GreyScaleBody
        {
            get { return (object)GetValue(GreyScaleBodyProperty); }
            set { SetValue(GreyScaleBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GreyScaleBody.  This enables animation, styling, binding, etc...
        protected static readonly DependencyProperty GreyScaleBodyProperty = 
            DependencyProperty.Register("GreyScaleBody", typeof(object), typeof(ColorHexagonPicker), new PropertyMetadata(null));

        private static void OnLayoutDependentPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as ColorHexagonPicker;

            if (sender != null && e.NewValue != e.OldValue)
                sender.GenerateLayout();
        }
        #endregion

    }
}
