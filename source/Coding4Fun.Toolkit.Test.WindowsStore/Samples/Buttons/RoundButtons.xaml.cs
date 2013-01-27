using System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Coding4Fun.Toolkit.Test.WindowsStore.Samples.Buttons
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class RoundButtons
	{
		public RoundButtons()
		{
			InitializeComponent();
		}

		public object TogglingContent
		{
			get { return (object)GetValue(TogglingContentProperty); }
			set { SetValue(TogglingContentProperty, value); }
		}

		// Using a DependencyProperty as the backing store for RoundButtonImageSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TogglingContentProperty =
			DependencyProperty.Register("TogglingContent", typeof(object), typeof(RoundToggleButtons), new PropertyMetadata(null));

		private async void BasicClick(object sender, RoutedEventArgs e)
		{
			var msg = new MessageDialog("You clicked the tile!");
			await msg.ShowAsync();
		}


		bool _isAdd;
		private void TogglingContentClick(object sender, RoutedEventArgs e)
		{
			_isAdd = !_isAdd;

			TogglingContent = (!_isAdd) ?
				XamlReader.Load(@"<Path 
					xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
					Height=""22""
					Stretch=""Uniform"" 
					Data=""M8.75502,0 L12.521,0 L12.521,8.75499 L21.279,8.75499 L21.279,12.52 L12.521,12.52 L12.521,21.279 L8.75502,21.279 L8.75502,12.52 L0,12.52 L0,8.75499 L8.75502,8.75499 z"" />"
			) as Path
			:
				XamlReader.Load(@"<Path 
					xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
					Height=""25"" 
					Stretch=""Uniform"" 
					Data=""F1M428.6484,639.8711C428.6484,631.0761,424.7974,623.1801,418.6894,617.7731C418.5234,617.7281,418.3414,617.6941,418.1574,617.6861C417.9364,617.6771,417.6974,617.6991,417.4464,617.7481C417.4054,617.7561,417.3644,617.7651,417.3224,617.7741C417.2694,617.7861,417.2164,617.7981,417.1624,617.8121C414.8104,618.6141,410.4984,622.0021,406.3184,626.4521C406.3184,626.4521,411.7074,632.7011,416.3314,639.9001C417.5294,641.7671,418.5414,643.7461,419.5214,645.6581C422.0014,650.5031,423.5784,656.3371,421.6624,658.9321C426.0194,653.7891,428.6484,647.1371,428.6484,639.8711 M420.9204,659.2881C420.9174,659.1251,420.9044,658.9581,420.8844,658.7891C420.3994,654.6421,415.3734,648.8161,410.2704,643.2981C405.6554,638.3061,401.6274,635.1021,399.1804,633.0301L399.1804,633.0301C399.1654,633.0411,399.1504,633.0531,399.1364,633.0631C397.6924,634.1141,392.7904,638.2451,388.1624,643.2511C382.8864,648.9571,377.6924,655.0211,377.5154,659.1951C377.5154,659.2281,377.5164,659.2621,377.5174,659.2951C377.5194,659.4181,377.5274,659.5371,377.5404,659.6541C377.5444,659.6971,377.5494,659.7421,377.5554,659.7851C377.5634,659.8551,377.6374,659.9901,377.6674,660.0211C383.0754,665.7621,390.7244,669.3791,399.1194,669.3791C407.7474,669.3791,415.5104,665.6811,420.9104,659.7851C420.9134,659.7401,420.9154,659.6951,420.9174,659.6521C420.9224,659.5331,420.9234,659.4111,420.9204,659.2881 M382.5104,618.3821C381.9584,618.1061,381.4414,617.9041,380.9764,617.8001C380.9494,617.7941,380.9214,617.7871,380.8944,617.7811C380.6304,617.7281,380.3854,617.7071,380.1624,617.7241C379.9064,617.7431,379.6524,617.8051,379.4394,617.8721C373.3954,623.2751,369.5904,631.1291,369.5904,639.8711C369.5904,647.0741,372.2594,653.7281,376.6514,658.8871C374.8694,656.5651,376.1964,651.2821,378.8474,645.7731C379.7794,643.8361,380.9444,641.9261,382.1084,640.0371C386.5864,632.7711,392.1204,626.5891,392.1204,626.5891C388.2324,622.3051,384.4694,619.4551,382.5104,618.3821 M415.7474,615.4821C415.4074,615.2851,415.0034,615.1251,414.5234,615.0311C412.3844,614.6101,409.5054,615.3091,407.2734,616.2111C403.7734,617.6271,400.4634,619.5181,399.1734,620.3311L399.1734,620.3311C397.9404,619.5401,394.5434,617.5031,391.0034,616.1501C388.4144,615.1611,385.4154,614.6191,383.7994,614.9871C383.2114,615.1211,382.7434,615.3271,382.3684,615.5691C387.1274,612.2871,392.8984,610.3621,399.1194,610.3621C405.2864,610.3621,411.0114,612.2521,415.7474,615.4821"" />"
			) as Path;
		}

		bool _isRed;
		private void ToggleBackgroundClick(object sender, RoutedEventArgs e)
		{
			_isRed = !_isRed;

			Background = new SolidColorBrush(_isRed ? Colors.Red : Colors.Transparent);
		}
	}
}
