using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var lectureTemplate = (DataTemplate)LayoutRoot.Resources["lectureTemplate"];
            int i = 1;
            var lectureSections = new[] { "A", "B", "C" };
            foreach (var lectureSection in lectureSections)
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.Children.Add(new TextBlock
                {
                    Text = lectureSection,
                    FontSize = 32,
                    Margin = new Thickness(12, 0, 12, 12),
                    TextWrapping = TextWrapping.Wrap,
                });
                var scrollViewer = new ScrollViewer
                {
                    Content = new ItemsControl
                    {
                        ItemTemplate = lectureTemplate,
                        ItemsSource = new[] { "D", "E", "F", "X", "Y", "Z" },
                    },
                };
                Grid.SetRow(scrollViewer, 1);
                grid.Children.Add(scrollViewer);
                pivot.Items.Add(new PivotItem
                {
                    Header = "Section " + i++,
                    Content = grid,
                });
            }
        }
    }
}