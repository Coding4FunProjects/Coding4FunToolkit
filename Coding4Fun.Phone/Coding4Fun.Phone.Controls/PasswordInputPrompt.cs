using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls
{
    public class PasswordInputPrompt : UserPrompt
    {
        protected PasswordBox InputBox;
        
        private const string InputBoxName = "inputBox";

        public PasswordInputPrompt()
        {
            DefaultStyleKey = typeof(PasswordInputPrompt);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InputBox = GetTemplateChild(InputBoxName) as PasswordBox;

            if (InputBox != null)
            {
                // manually adding
                // GetBindingExpression doesn't seem to respect TemplateBinding
                // so TextBoxBinding's code doesn't fire

                var binding = new System.Windows.Data.Binding
                {
                    Source = InputBox,
                    Path = new PropertyPath("Password"),
                };

                SetBinding(ValueProperty, binding);

                ThreadPool.QueueUserWorkItem(DelayInputSelect);
            }
        }

        private void DelayInputSelect(object value)
        {
            Thread.Sleep(250);
            Dispatcher.BeginInvoke(() =>
            {
                InputBox.Focus();
                InputBox.SelectAll();
            });
        }
    }
}
