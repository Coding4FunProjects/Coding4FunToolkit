using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Toolkit.Controls
{
	public class ChatBubble : ContentControl
	{
		public ChatBubble()
		{
			DefaultStyleKey = typeof(ChatBubble);

			IsEnabledChanged += ChatBubbleIsEnabledChanged;
		}

		void ChatBubbleIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			UpdateIsEnabledVisualState();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			UpdateChatBubbleDirection();
			UpdateIsEnabledVisualState();
		}

		public ChatBubbleDirection ChatBubbleDirection
		{
			get { return (ChatBubbleDirection)GetValue(ChatBubbleDirectionProperty); }
			set { SetValue(ChatBubbleDirectionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ChatBubbleDirection.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ChatBubbleDirectionProperty =
			DependencyProperty.Register("ChatBubbleDirection", typeof(ChatBubbleDirection), typeof(ChatBubble), new PropertyMetadata(ChatBubbleDirection.UpperRight, OnChatBubbleDirectionChanged));

		private static void OnChatBubbleDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var sender = d as ChatBubble;

			if (sender != null)
			{
				sender.UpdateChatBubbleDirection();
			}
		}

		private void UpdateChatBubbleDirection()
		{
			VisualStateManager.GoToState(this, ChatBubbleDirection.ToString(), true);
		}

		private void UpdateIsEnabledVisualState()
		{
			VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
		}
	}
}
