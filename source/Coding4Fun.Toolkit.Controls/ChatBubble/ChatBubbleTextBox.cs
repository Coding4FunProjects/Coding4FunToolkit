using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Toolkit.Controls
{
	public class ChatBubbleTextBox : TextBox
	{
		protected ContentControl HintContentElement;
		private const string HintContentElementName = "HintContentElement";
		private bool _hasFocus;
		public ChatBubbleTextBox()
		{
			DefaultStyleKey = typeof(ChatBubbleTextBox);
			TextChanged += ChatBubbleTextBoxTextChanged;
		}

		#region dependency properties
		public ChatBubbleDirection ChatBubbleDirection
		{
			get { return (ChatBubbleDirection)GetValue(ChatBubbleDirectionProperty); }
			set { SetValue(ChatBubbleDirectionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ChatBubbleDirection.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ChatBubbleDirectionProperty =
			DependencyProperty.Register("ChatBubbleDirection", typeof(ChatBubbleDirection), typeof(ChatBubbleTextBox), new PropertyMetadata(ChatBubbleDirection.UpperRight, OnChatBubbleDirectionChanged));

		public string Hint
		{
			get { return (string)GetValue(HintProperty); }
			set { SetValue(HintProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HintProperty =
			DependencyProperty.Register("Hint", typeof(string), typeof(ChatBubbleTextBox), new PropertyMetadata(""));

		public Style HintStyle
		{
			get { return (Style)GetValue(HintStyleProperty); }
			set { SetValue(HintStyleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for HintStyle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HintStyleProperty =
			DependencyProperty.Register("HintStyle", typeof(Style), typeof(ChatBubbleTextBox), new PropertyMetadata(null));
		#endregion

		#region overrides
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			HintContentElement = GetTemplateChild(HintContentElementName) as ContentControl;

			UpdateHintVisibility();
			UpdateChatBubbleDirection();
		}
		
		
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			_hasFocus = true;
			SetHintVisibility(Visibility.Collapsed);
			
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			_hasFocus = false;
			UpdateHintVisibility();
			
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Determines if the Hint should be shown or not based on if there is content in the TextBox.
		/// </summary>
		private void UpdateHintVisibility()
		{
			if(!_hasFocus)
				SetHintVisibility(string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Collapsed);
		}

		private void SetHintVisibility(Visibility value)
		{
			if (HintContentElement != null)
			{
				HintContentElement.Visibility = value;
			}
		}

		#endregion
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

		void ChatBubbleTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateHintVisibility();
		}
	}
}
