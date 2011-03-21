using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Coding4Fun.Phone.Controls
{
    public class MessagePrompt : ActionPopUp<object, PopUpResult>
    {
        RoundButton cancelButton;

        public MessagePrompt()
        {
            DefaultStyleKey = typeof(MessagePrompt);

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

        public bool IsCancelVisibile
        {
            get { return (bool)GetValue(IsCancelVisibileProperty); }
            set { SetValue(IsCancelVisibileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCancelVisibile.  This enables animation, styling, binding, etc...
        public readonly DependencyProperty IsCancelVisibileProperty =
            DependencyProperty.Register("IsCancelVisibile", typeof(bool), typeof(MessagePrompt), new PropertyMetadata(false, OnCancelButtonVisibilityChanged));
        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessagePrompt), new PropertyMetadata(""));
        
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessagePrompt), new PropertyMetadata(""));

        private static void OnCancelButtonVisibilityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = ((MessagePrompt)o);

            if (sender != null && e.NewValue != e.OldValue)
                sender.SetCancelButtonVisibility((bool)e.NewValue);
        }

        private void SetCancelButtonVisibility(bool value)
        {
            cancelButton.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<object, PopUpResult> { PopUpResult = PopUpResult.Ok });
        }

        private void cancelled_Click(object sender, RoutedEventArgs e)
        {
            OnCompleted(new PopUpEventArgs<object, PopUpResult> { PopUpResult = PopUpResult.Cancelled });
        }
    }
}
