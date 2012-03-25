using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls
{
	public class ChatBubbleTextBox : TextBox
	{
		public ChatBubbleTextBox()
		{
			DefaultStyleKey = typeof(ChatBubbleTextBox);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			UpdateChatBubbleDirection();
		}

		public ChatBubbleDirection ChatBubbleDirection
		{
			get { return (ChatBubbleDirection)GetValue(ChatBubbleDirectionProperty); }
			set { SetValue(ChatBubbleDirectionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ChatBubbleDirection.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ChatBubbleDirectionProperty =
			DependencyProperty.Register("ChatBubbleDirection", typeof(ChatBubbleDirection), typeof(ChatBubbleTextBox), new PropertyMetadata(ChatBubbleDirection.UpperRight, OnChatBubbleDirectionChanged));

		private static void OnChatBubbleDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var sender = d as ChatBubbleTextBox;

			if (sender != null)
            {
				sender.UpdateChatBubbleDirection();
            }
		}

		private void UpdateChatBubbleDirection()
		{
			VisualStateManager.GoToState(this, ChatBubbleDirection.ToString(), true);
		}
	}
}
