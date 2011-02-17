using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Phone.Controls
{
    public class InputPrompt : ActionPopUp<string, PopUpResult>
    {
        protected TextBox InputBox;
        RoundButton cancelButton;
        private const string InputBoxName = "inputBox";

        public InputPrompt()
        {
            DefaultStyleKey = typeof (InputPrompt);

            var okButton = new RoundButton();
            cancelButton = new RoundButton
                                   {
                                       ImageSource =
                                           new BitmapImage(
                                           new Uri(
                                               "/Coding4Fun.Phone.Controls;component/Media/icons/appbar.cancel.rest.png",
                                               UriKind.RelativeOrAbsolute))
                                   };

            okButton.Click += ok_Click;
            cancelButton.Click += cancelled_Click;

            ActionPopUpButtons.Add(okButton);
            ActionPopUpButtons.Add(cancelButton);

            SetCancelButtonVisibility(IsCancelVisibile);
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

        public bool IsCancelVisibile
        {
            get { return (bool)GetValue(IsCancelVisibileProperty); }
            set { SetValue(IsCancelVisibileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCancelVisibile.  This enables animation, styling, binding, etc...
        public readonly DependencyProperty IsCancelVisibileProperty =
            DependencyProperty.Register("IsCancelVisibile", typeof(bool), typeof(InputPrompt), new PropertyMetadata(false, OnCancelButtonVisibilityChanged));

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

        private static void OnCancelButtonVisibilityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((InputPrompt)o);

            if (sender != null && e.NewValue != e.OldValue)
                sender.SetCancelButtonVisibility((bool)e.NewValue);
        }

        private void SetCancelButtonVisibility(bool value)
        {
            cancelButton.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<string, PopUpResult> { Result = Value, PopUpResult = PopUpResult.Ok });
        }

        private void cancelled_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Cancelled });
        }
    }
}
