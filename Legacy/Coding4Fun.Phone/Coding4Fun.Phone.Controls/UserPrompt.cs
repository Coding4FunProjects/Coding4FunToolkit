using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Phone.Controls
{
    public abstract class UserPrompt : ActionPopUp<string, PopUpResult>
    {
        readonly RoundButton _cancelButton;
        protected internal Action MessageChanged;

        protected UserPrompt()
        {
            var okButton = new RoundButton();
            _cancelButton = new RoundButton
            {
                ImageSource =
                    new BitmapImage(
                    new Uri(
                        "/Coding4Fun.Phone.Controls;component/Media/icons/appbar.cancel.rest.png",
                        UriKind.RelativeOrAbsolute))
            };

            okButton.Click += ok_Click;
            _cancelButton.Click += cancelled_Click;

            ActionPopUpButtons.Add(okButton);
            ActionPopUpButtons.Add(_cancelButton);

            SetCancelButtonVisibility(IsCancelVisible);
        }

        public bool IsCancelVisible
        {
            get { return (bool)GetValue(IsCancelVisibileProperty); }
            set { SetValue(IsCancelVisibileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCancelVisible.  This enables animation, styling, binding, etc...
        public readonly DependencyProperty IsCancelVisibileProperty =
            DependencyProperty.Register("IsCancelVisible", typeof(bool), typeof(UserPrompt), new PropertyMetadata(false, OnCancelButtonVisibilityChanged));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(UserPrompt), new PropertyMetadata(""));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(UserPrompt), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(UserPrompt), new PropertyMetadata("", OnMesageContentChanged));

        private static void OnMesageContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((UserPrompt)o);

            if (sender != null && e.NewValue != e.OldValue && sender.MessageChanged != null)
                sender.MessageChanged();
        }

        private static void OnCancelButtonVisibilityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
			var sender = ((UserPrompt)o);

            if (sender != null && e.NewValue != e.OldValue)
                sender.SetCancelButtonVisibility((bool)e.NewValue);
        }

        private void SetCancelButtonVisibility(bool value)
        {
            _cancelButton.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
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
