using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Coding4Fun.Toolkit.Controls.Common;
using Coding4Fun.Toolkit.Controls.Binding;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using Windows.System;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
#endif

namespace Coding4Fun.Toolkit.Controls
{
    public class PasswordInputPrompt : InputPrompt
    {
        readonly StringBuilder _inputText = new StringBuilder();
	    private DateTime _lastUpdated = DateTime.Now;

        public PasswordInputPrompt()
        {
            DefaultStyleKey = typeof(PasswordInputPrompt);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();
            
            if (InputBox != null)
			{
#if WINDOWS_STORE || WINDOWS_PHONE_APP
                var binding = new Windows.UI.Xaml.Data.Binding();
#elif WINDOWS_PHONE
			    var binding = new System.Windows.Data.Binding();
#endif

                binding.Source = InputBox;
                binding.Path = new PropertyPath("Text");

                SetBinding(ValueProperty, binding);

                TextBinding.SetUpdateSourceOnChange(InputBox, true);
				InputBox.TextChanged -= InputBoxTextChanged;
				InputBox.SelectionChanged -= InputBoxSelectionChanged;

                // manually adding
                // GetBindingExpression doesn't seem to respect TemplateBinding
                // so TextBoxBinding's code doesn't fire

                InputBox.TextChanged += InputBoxTextChanged;
				InputBox.SelectionChanged += InputBoxSelectionChanged;
            }
        }

		#region Control Events

		private void InputBoxSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (InputBox.SelectionLength > 0)
				InputBox.SelectionLength = 0;
		}

		private async void InputBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			var diff = InputBox.Text.Length - _inputText.Length;

			// handle text removes
			if (diff < 0)
			{
				diff *= -1;

				// adding one since the selection has moved
				var startIndex = InputBox.SelectionStart + 1 - diff;

				if (startIndex < 0)
					startIndex = 0;

				_inputText.Remove(startIndex, diff);

				Value = _inputText.ToString();
			}
			else if (diff > 0)
			{
				// get new chars
				// append onto SB
				// set value
				// update InputBox with *
				var selectionStart = InputBox.SelectionStart;
				var selectionIndex = selectionStart - diff;
				var newChars = InputBox.Text.Substring(selectionIndex, diff);

				_inputText.Insert(selectionIndex, newChars);

				Value = _inputText.ToString();

				// Paste operation
				if (diff > 1)
				{
					var replacementString = new StringBuilder();

					replacementString.Insert(0, PasswordChar.ToString(), InputBox.Text.Length);
					InputBox.Text = replacementString.ToString();
				}
				else
				{
					if (InputBox.Text.Length >= 2)
					{
						var replacementString = new StringBuilder();
						replacementString.Insert(0, PasswordChar.ToString(), InputBox.Text.Length - diff);
						replacementString.Insert(selectionIndex, newChars);

						InputBox.Text = replacementString.ToString();
					}

					await ExecuteDelayedOverwrite();
					_lastUpdated = DateTime.Now;
				}

				InputBox.SelectionStart = selectionStart;
			}
		}
		#endregion

		#region helper methods
		private async Task ExecuteDelayedOverwrite()
		{
			await Task.Run(async () =>
                {
					var delay = TimeSpan.FromMilliseconds(500);
					await Task.Delay(delay);

					if (DateTime.Now - _lastUpdated < TimeSpan.FromMilliseconds(500))
						return;

#if WINDOWS_STORE || WINDOWS_PHONE_APP
                    await ApplicationSpace.CurrentDispatcher.RunAsync(CoreDispatcherPriority.Normal,
#elif WINDOWS_PHONE
                    ApplicationSpace.CurrentDispatcher.BeginInvoke(
#endif
						() =>
						{
							var selectionStart = InputBox.SelectionStart;

							InputBox.Text = Regex.Replace(InputBox.Text, ".", PasswordChar.ToString());

							InputBox.SelectionStart = selectionStart;
						});
				});
		}
		#endregion

		#region Dependency Property Callbacks
		#endregion

		#region Dependency Properties / Properties

		public char PasswordChar
		{
			get { return (char)GetValue(PasswordCharProperty); }
			set { SetValue(PasswordCharProperty, value); }
		}

		// Using a DependencyProperty as the backing store for PasswordChar.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PasswordCharProperty =
			DependencyProperty.Register("PasswordChar", typeof(char), typeof(PasswordInputPrompt), new PropertyMetadata('●'));
		#endregion
	}
}
