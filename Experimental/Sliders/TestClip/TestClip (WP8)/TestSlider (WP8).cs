using System.Windows;

namespace TestClip
{
	public partial class TestSlider
	{
		private void ApplyPlatformFeatures()
		{
		}

		protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);

			
		}
		protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseEnter(e);

			VisualStateManager.GoToState(this, "Pressed", true);
		}
	}
}
