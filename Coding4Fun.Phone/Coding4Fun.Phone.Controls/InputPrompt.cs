using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

namespace Coding4Fun.Phone.Controls
{
    public class InputPrompt : PopUp<string, PopUpResult>
    {
        protected Button OkButton;
        protected TextBox InputBox;
        private const string OkButtonName = "okButton";
        private const string InputBoxName = "inputBox";

        public InputPrompt()
        {
            DefaultStyleKey = typeof (InputPrompt);
            DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (OkButton != null)
                OkButton.Click -= ok_Click;

            OkButton = GetTemplateChild(OkButtonName) as Button;
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

                if (!DesignerProperties.IsInDesignTool)
                {
                    InputBox.Focus();
                    InputBox.SelectAll();
                }
            }

            if (OkButton != null)
                OkButton.Click += ok_Click;
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


        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
       
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(InputPrompt), new PropertyMetadata(""));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(InputPrompt), new PropertyMetadata(""));
        
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(InputPrompt), new PropertyMetadata(""));

        
        

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<string, PopUpResult> { Result = Value, PopUpResult = PopUpResult.OK });
        }
    }
}
