using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Toolkit.Controls
{
    public class PasswordInputPrompt : InputPrompt
    {
        readonly StringBuilder _inputText = new StringBuilder();

        public PasswordInputPrompt()
        {
            DefaultStyleKey = typeof(PasswordInputPrompt);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            if (InputBox != null)
            {
                // manually adding
                // GetBindingExpression doesn't seem to respect TemplateBinding
                // so TextBoxBinding's code doesn't fire

                InputBox.TextChanged += InputBoxTextChanged;
				InputBox.SelectionChanged += InputBoxSelectionChanged;
            }
        }

		void InputBoxSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (InputBox.SelectionLength > 0)
				InputBox.SelectionLength = 0;  
		}

        void InputBoxTextChanged(object sender, TextChangedEventArgs e)
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
				var selectionIndex = selectionStart - 1;
				var newChars = InputBox.Text.Substring(selectionIndex, diff);
				_inputText.Insert(selectionIndex, newChars);

                Value = _inputText.ToString();

                if (InputBox.Text.Length >= 2)
                {
                    var replacementString = new StringBuilder();
					replacementString.Insert(0, PasswordChar.ToString(CultureInfo.InvariantCulture), InputBox.Text.Length - 1);
					replacementString.Insert(selectionIndex, newChars);

                    InputBox.Text = replacementString.ToString();
                }

                InputBox.SelectionStart = selectionStart;
            }
        }
        
        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasswordChar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(PasswordInputPrompt), new PropertyMetadata('●'));
    }
}
