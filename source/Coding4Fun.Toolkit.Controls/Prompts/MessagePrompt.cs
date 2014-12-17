using System;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
#endif


namespace Coding4Fun.Toolkit.Controls
{
    public class MessagePrompt : UserPrompt
    {
        public MessagePrompt()
        {
            DefaultStyleKey = typeof(MessagePrompt);

            MessageChanged = SetBodyMessage;
        }

		#region Control Events
		#endregion

		#region helper methods
		protected internal void SetBodyMessage()
		{
			Body = new TextBlock
			{
				Text = Message,
				TextWrapping = TextWrapping.Wrap
			};
		}
		#endregion

		#region Dependency Property Callbacks
		#endregion

		#region Dependency Properties / Properties
		public object Body
		{
			get { return GetValue(BodyProperty); }
			set { SetValue(BodyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Body.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BodyProperty =
			DependencyProperty.Register("Body", typeof(object), typeof(MessagePrompt), new PropertyMetadata(null));
		#endregion
	}
}
