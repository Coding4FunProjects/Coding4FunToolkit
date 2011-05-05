using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coding4Fun.Phone.Controls
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
        /// <see cref="P:Microsoft.Phone.Controls.AutoCompleteBox.InputScope"/>
        /// dependency property.
        /// </summary>
        public static readonly DependencyProperty InputScopeProperty =
            DependencyProperty.Register(
                "InputScope",
                typeof(InputScope),
                typeof(InputPrompt),
                null);
        #endregion public InputScope InputScope
    }
}
