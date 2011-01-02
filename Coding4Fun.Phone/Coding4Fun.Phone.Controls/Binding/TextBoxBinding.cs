using System.Windows;
using System.Windows.Controls;

namespace Coding4Fun.Phone.Controls.Binding
{
    /// <summary>
    /// Supports a PropertyChanged-Trigger for DataBindings
    /// Works for only TextBoxes
    ///  <TextBox 
    ///     Text="{Binding FirstName, Mode=TwoWay}"
    ///     local:TextBoxBinding.UpdateSourceOnChange="True" />
    /// 
    /// Code is just a renamed verison of 
    /// http://www.thomasclaudiushuber.com/blog/2009/07/17/here-it-is-the-updatesourcetrigger-for-propertychanged-in-silverlight/
    /// 
    /// (C) Thomas Claudius Huber 2009
    /// http://www.thomasclaudiushuber.com
    /// </summary>
    public class TextBoxBinding
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
        public static readonly DependencyProperty
          UpdateSourceOnChangeProperty =
            DependencyProperty.RegisterAttached(
            "UpdateSourceOnChange",
            typeof(bool),
            typeof(TextBoxBinding),
            new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged (DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var txt = obj as TextBox;
            if (txt == null)
                return;

            var newValue = (bool)e.NewValue;
            var oldValue = (bool)e.OldValue;

            if (newValue == oldValue)
                return;

            if (newValue)
            {
                txt.TextChanged += OnTextChanged;
            }
            else
            {
                txt.TextChanged -= OnTextChanged;
            }
        }
         
        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;
            
            var bind = textBox.GetBindingExpression(TextBox.TextProperty);
            if (bind != null)
            {
                bind.UpdateSource();
            }
        }
    }
}
