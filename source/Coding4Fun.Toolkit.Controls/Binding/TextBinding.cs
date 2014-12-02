using System.Windows;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#elif WINDOWS_PHONE
using System.Windows.Controls;
#endif

namespace Coding4Fun.Toolkit.Controls.Binding
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
        #region UpdateSourceOnChange
        #region DependencyProperty
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
                new PropertyMetadata(false, OnUpdateSourceOnChangePropertyChanged));
        #endregion

        private static void OnUpdateSourceOnChangePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            HandleUpdateSourceOnChangeEventAppend(obj, (bool)e.NewValue);
        }

        private static void HandleUpdateSourceOnChangeEventAppend(object sender, bool value)
        {
			if (sender is TextBox)
                HandleUpdateSourceOnChangeEventAppendTextBox(sender, value);
            else if (sender is PasswordBox)
                HandleUpdateSourceOnChangeEventAppendPassword(sender, value);
        }

        private static void HandleUpdateSourceOnChangeEventAppendTextBox(object sender, bool value)
        {
            var item = sender as TextBox;

            if (item == null)
                return;

            if (value)
                item.TextChanged += UpdateSourceOnChangePropertyChanged;
            else
                item.TextChanged -= UpdateSourceOnChangePropertyChanged;
        }

        private static void HandleUpdateSourceOnChangeEventAppendPassword(object sender, bool value)
        {
            var item = sender as PasswordBox;

            if (item == null)
                return;

            if (value)
                item.PasswordChanged += UpdateSourceOnChangePropertyChanged;
            else
                item.PasswordChanged -= UpdateSourceOnChangePropertyChanged;
        }

        private static void UpdateSourceOnChangePropertyChanged(object sender, RoutedEventArgs e)
        {
            var dp = GetDependancyPropertyForText(sender);

            if (dp == null)
                return;

            var bind = ((FrameworkElement)sender).GetBindingExpression(dp);

            if (bind != null)
                bind.UpdateSource();
        }

        private static DependencyProperty GetDependancyPropertyForText(object sender)
        {
            DependencyProperty returnVal = null;

            if (sender is TextBox)
                returnVal = TextBox.TextProperty;
            else if (sender is PasswordBox)
                returnVal = PasswordBox.PasswordProperty;

            return returnVal;
        }
        #endregion
    }
}
