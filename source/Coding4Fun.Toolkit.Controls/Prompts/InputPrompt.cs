using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Coding4Fun.Toolkit.Controls.Common;

namespace Coding4Fun.Toolkit.Controls
{
    public class InputPrompt : UserPrompt
    {
        protected TextBox InputBox;
        
        private const string InputBoxName = "inputBox";

        public InputPrompt()
        {
            DefaultStyleKey = typeof (InputPrompt);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InputBox = GetTemplateChild(InputBoxName) as TextBox;

            if (InputBox != null)
            {
                // manually adding
                // GetBindingExpression doesn't seem to respect TemplateBinding
                // so TextBoxBinding's code doesn't fire

                var binding = new System.Windows.Data.Binding
                                  {
                                      Source = InputBox,
                                      Path = new PropertyPath("Text"),
                                  };

                SetBinding(ValueProperty, binding);
                
                HookUpEventForIsSubmitOnEnterKey();

				if (!ApplicationSpace.IsDesignMode)
	                ThreadPool.QueueUserWorkItem(DelayInputSelect);
            }
        }

		#region Control Events
		#endregion

		#region helper methods
		private void DelayInputSelect(object value)
		{
			Thread.Sleep(250);
			Dispatcher.BeginInvoke(() =>
			{
				InputBox.Focus();
				InputBox.SelectAll();
			});
		}

		private void HookUpEventForIsSubmitOnEnterKey()
		{
			InputBox = GetTemplateChild(InputBoxName) as TextBox;

			if (InputBox == null)
				return;

			InputBox.KeyDown -= InputBoxKeyDown;

			if (IsSubmitOnEnterKey)
				InputBox.KeyDown += InputBoxKeyDown;
		}

		void InputBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				OnCompleted(new PopUpEventArgs<string, PopUpResult> { Result = Value, PopUpResult = PopUpResult.Ok });
		}
		#endregion

		#region Dependency Property Callbacks
		private static void OnIsSubmitOnEnterKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var inputPrompt = d as InputPrompt;

			if (inputPrompt != null)
				inputPrompt.HookUpEventForIsSubmitOnEnterKey();
		}
		#endregion

		#region Dependency Properties / Properties
		#region public bool IsSubmitOnEnterKey
		public bool IsSubmitOnEnterKey
		{
			get { return (bool)GetValue(IsSubmitOnEnterKeyProperty); }
			set { SetValue(IsSubmitOnEnterKeyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsSubmitOnEnterKey.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsSubmitOnEnterKeyProperty =
			DependencyProperty.Register("IsSubmitOnEnterKey", typeof(bool), typeof(InputPrompt), new PropertyMetadata(true, OnIsSubmitOnEnterKeyPropertyChanged));
		#endregion
		#region public TextWrapping MessageTextWrapping
		public TextWrapping MessageTextWrapping
		{
			get { return (TextWrapping)GetValue(MessageTextWrappingProperty); }
			set { SetValue(MessageTextWrappingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MessageTextWrapping.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MessageTextWrappingProperty =
			DependencyProperty.Register("MessageTextWrapping", typeof(TextWrapping), typeof(InputPrompt), new PropertyMetadata(TextWrapping.NoWrap));
		#endregion
		#region public InputScope InputScope
		/// <summary>
		/// Gets or sets the
		/// <see cref="T:System.Windows.Input.InputScope"/>
		/// used by the Text template part.
		/// </summary>
		public InputScope InputScope
		{
			get { return (InputScope)GetValue(InputScopeProperty); }
			set { SetValue(InputScopeProperty, value); }
		}

		/// <summary>
		/// Identifies the
		/// <see cref="T:System.Windows.Input.InputScope"/>
		/// dependency property.
		/// </summary>
		public static readonly DependencyProperty InputScopeProperty =
			DependencyProperty.Register("InputScope", typeof(InputScope), typeof(InputPrompt), null);
		#endregion
		#endregion
	}
}
