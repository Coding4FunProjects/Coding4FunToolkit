using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls.Binding
{
    /// <summary>
    /// Supports a PropertyChanged-Trigger for DataBindings
    /// Works for only TextBoxes and PasswordBoxes
    ///  <TextBox 
    ///     Text="{Binding FirstName, Mode=TwoWay}"
    ///     local:TextBinding.UpdateSourceOnChange="True" />
    /// 
    /// Code is based by Thomas Huber
    /// http://www.thomasclaudiushuber.com/blog/2009/07/17/here-it-is-the-updatesourcetrigger-for-propertychanged-in-silverlight/
    /// 
    /// Idea for allowing PasswordBox as well is from Dele Olowoyo
    /// http://www.pragmaticpattern.wordpress.com
    /// http://www.dcubeapps.com
    /// </summary>
    public class TextBinding
    {
        public static bool GetUpdateSourceOnChange(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpdateSourceOnChangeProperty);
        }

        public static void SetUpdateSourceOnChange(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateSourceOnChangeProperty, value);
        }

        // Using a DependencyProperty as the backing store for …
        public static readonly DependencyProperty UpdateSourceOnChangeProperty =
            DependencyProperty.RegisterAttached(
                "UpdateSourceOnChange",
                typeof(bool),
                typeof(TextBinding),
                new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged (DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            HandleEventAppend(obj, (bool)e.NewValue);
        }

        private static void ItemChanged(object sender, RoutedEventArgs e)
        {
            var dp = GetDependancyPropertyForControl(sender);

            if (dp == null)
                return;

            var bind = ((FrameworkElement)sender).GetBindingExpression(dp);

            if (bind != null)
                bind.UpdateSource();
        }

        private static DependencyProperty GetDependancyPropertyForControl(object sender)
        {
            var type = sender.GetType();
            DependencyProperty returnVal = null;
            
            if (type == typeof(TextBox))
                returnVal = TextBox.TextProperty;
            else if(type == typeof(PasswordBox))
                returnVal = PasswordBox.PasswordProperty;

            return returnVal;
        }

        private static void HandleEventAppend(object sender, bool value)
        {
            var type = sender.GetType();

            if (type == typeof(TextBox))
                HandleEventAppendTextBox(sender, value);
            else if (type == typeof(PasswordBox))
                HandleEventAppendPassword(sender, value);
        }

        private static void HandleEventAppendTextBox(object sender, bool value)
        {
            var item = sender as TextBox;

            if (item == null)
                return;

            if (value)
                item.TextChanged += ItemChanged;
            else
                item.TextChanged -= ItemChanged;
        }

        private static void HandleEventAppendPassword(object sender, bool value)
        {
            var item = sender as PasswordBox;

            if (item == null)
                return;

            if (value)
                item.PasswordChanged += ItemChanged;
            else
                item.PasswordChanged -= ItemChanged;
        }
    }
}
