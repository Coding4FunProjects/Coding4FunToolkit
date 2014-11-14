#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;

#endif

using Coding4Fun.Toolkit.Controls.Common;

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

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
		{
			base.OnApplyTemplate();

			UpdateChatBubbleDirection();
			UpdateIsEnabledVisualState();

            UpdateIsEquallySpaced();
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

        public bool IsEquallySpaced
        {
            get { return (bool)GetValue(IsEquallySpacedProperty); }
            set { SetValue(IsEquallySpacedProperty, value); }
        }

        public static readonly DependencyProperty IsEquallySpacedProperty =
          DependencyProperty.Register("IsEquallySpaced", typeof(bool),
          typeof(ChatBubble),
          new PropertyMetadata(true, OnIsEquallySpacedChanged));

        private static bool _triggered = false;

        private static void OnIsEquallySpacedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as ChatBubble;

            if (sender != null)
            {
                _triggered = true;
                sender.UpdateIsEquallySpaced();
            }
        }

        private void UpdateIsEquallySpaced()
        {
            int delta = IsEquallySpaced ? ControlHelper.MagicSpacingNumber : (_triggered ? -1 * ControlHelper.MagicSpacingNumber : 0);

            var margin = Margin;

            switch (ChatBubbleDirection)
            {
                case ChatBubbleDirection.LowerLeft:
                case ChatBubbleDirection.LowerRight:
                    margin.Top += delta;
                    break;
                case ChatBubbleDirection.UpperLeft:
                case ChatBubbleDirection.UpperRight:
                    margin.Bottom += delta;
                    break;
            }

            Margin = margin;
        }
	}
}
