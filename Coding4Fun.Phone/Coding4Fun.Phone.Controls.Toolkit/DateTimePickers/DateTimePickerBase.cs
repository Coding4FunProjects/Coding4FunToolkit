using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

using Coding4Fun.Phone.Controls.Primitives;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    [TemplatePart(Name = ButtonPartName, Type = typeof(ButtonBase))]
    public abstract class DateTimePickerBase<T> : Control where T : struct
    {
        private const string ButtonPartName = "DateTimeButton";

        private ButtonBase _dateButtonPart;
        private PhoneApplicationFrame _frame;
        private object _frameContentWhenOpened;
        private NavigationInTransition _savedNavigationInTransition;
        private NavigationOutTransition _savedNavigationOutTransition;
        private IValuePickerPage<T> _dateTimePickerPage;

		public string DialogTitle
		{
			get { return (string)GetValue(DialogTitleProperty); }
			set { SetValue(DialogTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DialogTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DialogTitleProperty =
			DependencyProperty.Register("DialogTitle", typeof(string), typeof(DateTimePickerBase<T>), new PropertyMetadata(""));
		

        /// <summary>
        /// Gets or sets the DateTime value.
        /// </summary>
        [TypeConverter(typeof(TimeTypeConverter))]
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Matching the use of Value as a Picker naming convention.")]
        public T? Value
        {
            get { return (T?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the Value DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(T?), typeof(DateTimePickerBase<T>), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePickerBase<T>)o).OnValueChanged((T?)e.OldValue, (T?)e.NewValue);
        }

        private void OnValueChanged(T? oldValue, T? newValue)
        {
            UpdateValueString();
            OnValueChanged(new ValueChangedEventArgs<T>(oldValue, newValue));
        }

        /// <summary>
        /// Called when the value changes.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected abstract void OnValueChanged(ValueChangedEventArgs<T> e);

        /// <summary>
        /// Gets the string representation of the selected value.
        /// </summary>
        public string ValueString
        {
            get { return (string)GetValue(ValueStringProperty); }
            protected set { SetValue(ValueStringProperty, value); }
        }

        /// <summary>
        /// Identifies the ValueString DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register(
            "ValueString", typeof(string), typeof(DateTimePickerBase<T>), null);

        /// <summary>
        /// Gets or sets the format string to use when converting the Value property to a string.
        /// </summary>
        public string ValueStringFormat
        {
            get { return (string)GetValue(ValueStringFormatProperty); }
            set { SetValue(ValueStringFormatProperty, value); }
        }

        /// <summary>
        /// Identifies the ValueStringFormat DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueStringFormatProperty = DependencyProperty.Register(
            "ValueStringFormat", typeof(string), typeof(DateTimePickerBase<T>), new PropertyMetadata(null, OnValueStringFormatChanged));

        private static void OnValueStringFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePickerBase<T>)o).OnValueStringFormatChanged();
        }

        private void OnValueStringFormatChanged()
        {
            UpdateValueString();
        }

        /// <summary>
        /// Gets or sets the header of the control.
        /// </summary>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the Header DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(DateTimePickerBase<T>), null);

        /// <summary>
        /// Gets or sets the template used to display the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the HeaderTemplate DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", typeof(DataTemplate), typeof(DateTimePickerBase<T>), null);

        /// <summary>
        /// Gets or sets the Uri to use for loading the IDateTimePickerPage instance when the control is clicked.
        /// </summary>
        public Uri PickerPageUri
        {
            get { return (Uri)GetValue(PickerPageUriProperty); }
            set { SetValue(PickerPageUriProperty, value); }
        }

        /// <summary>
        /// Identifies the PickerPageUri DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty PickerPageUriProperty = DependencyProperty.Register(
            "PickerPageUri", typeof(Uri), typeof(DateTimePickerBase<T>), null);

        /// <summary>
        /// Gets the fallback value for the ValueStringFormat property.
        /// </summary>
        protected virtual string ValueStringFormatFallback { get { return "{0}"; } }


        /// <summary>
        /// Called when the control's Template is expanded.
        /// </summary>
        public override void OnApplyTemplate()
        {
            // Unhook from old template
            if (null != _dateButtonPart)
            {
                _dateButtonPart.Click -= HandleDateButtonClick;
            }

            base.OnApplyTemplate();

            // Hook up to new template
            _dateButtonPart = GetTemplateChild(ButtonPartName) as ButtonBase;
            if (null != _dateButtonPart)
            {
                _dateButtonPart.Click += HandleDateButtonClick;
            }
        }

        private void HandleDateButtonClick(object sender, RoutedEventArgs e)
        {
            OpenPicker();
        }

        protected internal virtual void UpdateValueString()
        {
            ValueString = string.Format(CultureInfo.CurrentCulture, ValueStringFormat ?? ValueStringFormatFallback, Value);
        }

        public void OpenPicker()
        {
            if (null == PickerPageUri)
            {
                throw new ArgumentException("PickerPageUri property must not be null.");
            }

            if (null == _frame)
            {
                // Hook up to necessary events and navigate
                _frame = Application.Current.RootVisual as PhoneApplicationFrame;
                if (null != _frame)
                {
                    _frameContentWhenOpened = _frame.Content;

                    // Save and clear host page transitions for the upcoming "popup" navigation
                    var frameContentWhenOpenedAsUiElement = _frameContentWhenOpened as UIElement;
                    if (null != frameContentWhenOpenedAsUiElement)
                    {
                        _savedNavigationInTransition = TransitionService.GetNavigationInTransition(frameContentWhenOpenedAsUiElement);
                        TransitionService.SetNavigationInTransition(frameContentWhenOpenedAsUiElement, null);
                        _savedNavigationOutTransition = TransitionService.GetNavigationOutTransition(frameContentWhenOpenedAsUiElement);
                        TransitionService.SetNavigationOutTransition(frameContentWhenOpenedAsUiElement, null);
                    }

                    _frame.Navigated += HandleFrameNavigated;

                    if (_frame.GetType() == typeof(PhoneApplicationFrame))
                        _frame.NavigationStopped += HandleFrameNavigationStoppedOrFailed;

                    _frame.NavigationFailed += HandleFrameNavigationStoppedOrFailed;

                    _frame.Navigate(PickerPageUri);
                }
            }

        }

        private void ClosePickerPage()
        {
            // Unhook from events
            if (null != _frame)
            {
                _frame.Navigated -= HandleFrameNavigated;
                _frame.NavigationStopped -= HandleFrameNavigationStoppedOrFailed;
                _frame.NavigationFailed -= HandleFrameNavigationStoppedOrFailed;

                // Restore host page transitions for the completed "popup" navigation
                var frameContentWhenOpenedAsUiElement = _frameContentWhenOpened as UIElement;
                if (null != frameContentWhenOpenedAsUiElement)
                {
                    TransitionService.SetNavigationInTransition(frameContentWhenOpenedAsUiElement, _savedNavigationInTransition);
                    _savedNavigationInTransition = null;
                    TransitionService.SetNavigationOutTransition(frameContentWhenOpenedAsUiElement, _savedNavigationOutTransition);
                    _savedNavigationOutTransition = null;
                }

                _frame = null;
                _frameContentWhenOpened = null;
            }
            // Commit the value if available
            if (null != _dateTimePickerPage)
            {
                if (_dateTimePickerPage.Value.HasValue)
                {
                    Value = _dateTimePickerPage.Value.Value;
                }
                _dateTimePickerPage = null;
            }
        }
        
        private void HandleFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == _frameContentWhenOpened)
            {
                // Navigation to original page; close the picker page
                ClosePickerPage();
            }
            else if (null == _dateTimePickerPage)
            {
                // Navigation to a new page; capture it and push the value in
                _dateTimePickerPage = e.Content as IValuePickerPage<T>;
                if (null != _dateTimePickerPage)
                {
                    NavigateToNewPage(e.Content);
                }
            }
        }


        /// <summary>
        /// Navigation to a new page; capture it and push the value in
        /// </summary>
        /// <param name="page">the destination page</param>
        protected virtual void NavigateToNewPage(object page)
        {
            var navPage = page as IValuePickerPage<T>;
            if (navPage != null)
            {
                navPage.Value = Value.GetValueOrDefault();
				navPage.DialogTitle = DialogTitle;
            }
        }

        private void HandleFrameNavigationStoppedOrFailed(object sender, EventArgs e)
        {
            // Abort
            ClosePickerPage();
        }
    }
}
