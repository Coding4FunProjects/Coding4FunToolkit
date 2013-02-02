using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls
{
    public class MessagePrompt : UserPrompt
    {
        public MessagePrompt()
        {
            DefaultStyleKey = typeof(MessagePrompt);
            MessageChanged = SetBodyMessage;
        }

        public object Body
        {
            get { return GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Body.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(object), typeof(MessagePrompt), new PropertyMetadata(null));

        protected internal void SetBodyMessage()
        {
            Body = new TextBlock
                       {
                           Text = Message,
                           TextWrapping = TextWrapping.Wrap
                       };
        }
    }
}
