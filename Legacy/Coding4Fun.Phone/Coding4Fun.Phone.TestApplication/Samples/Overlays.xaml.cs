using System.Windows;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Overlays : PhoneApplicationPage
    {
        public Overlays()
        {
            InitializeComponent();
            DataContext = this;
        }

        bool _databindingBroke;
        const string databindingError = "Databinding was removed on prior call";

        private void Ding_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLICK!", "Testing with Click Event", MessageBoxButton.OKCancel);
        }

        private void ShowOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Show();
            _databindingBroke = true;
        }

        private void HideOverlay(object sender, RoutedEventArgs e)
        {
            progressOverlay.Hide();
            _databindingBroke = true;
        }

        private void DirectVis(object sender, RoutedEventArgs e)
        {
            progressOverlay.Visibility = Visibility.Visible;
            _databindingBroke = true;
        }

        private void DirectCol(object sender, RoutedEventArgs e)
        {
            progressOverlay.Visibility = Visibility.Collapsed;
            _databindingBroke = true;
        }

        private void DataBindVis(object sender, RoutedEventArgs e)
        {
            CheckDataBinding();
            OverlayVis = Visibility.Visible;
        }

        private void DataBindCol(object sender, RoutedEventArgs e)
        {
            CheckDataBinding();
            OverlayVis = Visibility.Collapsed;
        }

        private void CheckDataBinding()
        {
            if (_databindingBroke)
                MessageBox.Show(databindingError);
        }

        public Visibility OverlayVis
        {
            get { return (Visibility)GetValue(OverlayVisProperty); }
            set { SetValue(OverlayVisProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for OverlayVis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayVisProperty =
            DependencyProperty.Register("OverlayVis", typeof(Visibility), typeof(Overlays), new PropertyMetadata(Visibility.Visible));

		private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			MessageBox.Show("TAP!", "Testing with Gesture Tap", MessageBoxButton.OKCancel);
		}
    }
}