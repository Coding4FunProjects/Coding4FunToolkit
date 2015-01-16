using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ImageTiles
	{
		public ImageTiles()
		{
			InitializeComponent();

			SetItemSource(15);

            this.data.ValueChanged += DataValueChanged;
		}

		private void SetItemSource(int amount)
		{
			var items = new List<Uri>();

			for (int i = 0; i <= amount; i++)
			{
				items.Add(new Uri(String.Format("ms-appx:/Media/images/{0}.jpg", i)));
			}

			fadeTile.ItemsSource = items;
		}

        private void ButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            fadeTile.CycleImage();
        }

        private void DataValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SetItemSource((int)e.NewValue);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AnimationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AnimationCombo == null)
                return;

            fadeTile.AnimationType = (ImageTileAnimationTypes)Enum.Parse(typeof(ImageTileAnimationTypes), (string)(this.AnimationCombo.SelectedItem as ComboBoxItem).Content);
        }

        private void animationSpeed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.AnimationCombo == null)
                return;

            fadeTile.AnimationDuration = (int)this.animationSpeed.Value;
        }
	}
}
