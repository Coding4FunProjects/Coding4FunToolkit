using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

using Coding4Fun.Phone.Controls.Toolkit.Primitives;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Phone.Controls.Primitives
{
    public abstract class ValuePickerBasePage<T> : PhoneApplicationPage, IValuePickerPage<T> where T : struct
    {
        private const string VisibilityGroupName = "VisibilityStates";
        private const string OpenVisibilityStateName = "Open";
        private const string ClosedVisibilityStateName = "Closed";
        private const string StateKey_Value = "DateTimePickerPageBase_State_Value";

        private LoopingSelector _primarySelectorPart;
        private LoopingSelector _secondarySelectorPart;
        private LoopingSelector _tertiarySelectorPart;
        private Storyboard _closedStoryboard;

        /// <summary>
        /// Initializes the ValuePickerPageBase class; must be called from the subclass's constructor.
        /// </summary>
        /// <param name="primarySelector">Primary selector.</param>
        /// <param name="secondarySelector">Secondary selector.</param>
        /// <param name="tertiarySelector">Tertiary selector.</param>
        protected void InitializeDateTimePickerPage(LoopingSelector primarySelector, LoopingSelector secondarySelector, LoopingSelector tertiarySelector)
        {
            if (null == primarySelector)
            {
                throw new ArgumentNullException("primarySelector");
            }
            if (null == secondarySelector)
            {
                throw new ArgumentNullException("secondarySelector");
            }
            if (null == tertiarySelector)
            {
                throw new ArgumentNullException("tertiarySelector");
            }

            _primarySelectorPart = primarySelector;
            _secondarySelectorPart = secondarySelector;
            _tertiarySelectorPart = tertiarySelector;

            // Hook up to interesting events
            _primarySelectorPart.DataSource.SelectionChanged += HandleDataSourceSelectionChanged;
            _secondarySelectorPart.DataSource.SelectionChanged += HandleDataSourceSelectionChanged;
            _tertiarySelectorPart.DataSource.SelectionChanged += HandleDataSourceSelectionChanged;
            _primarySelectorPart.IsExpandedChanged += HandleSelectorIsExpandedChanged;
            _secondarySelectorPart.IsExpandedChanged += HandleSelectorIsExpandedChanged;
            _tertiarySelectorPart.IsExpandedChanged += HandleSelectorIsExpandedChanged;

            // Hide all selectors
            _primarySelectorPart.Visibility = Visibility.Collapsed;
            _secondarySelectorPart.Visibility = Visibility.Collapsed;
            _tertiarySelectorPart.Visibility = Visibility.Collapsed;

            // Position and reveal the culture-relevant selectors
            int column = 0;
            foreach (LoopingSelector selector in GetSelectorsOrderedByCulturePattern())
            {
                Grid.SetColumn(selector, column);
                selector.Visibility = Visibility.Visible;
                column++;
            }

            // Hook up to storyboard(s)
            var templateRoot = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            if (null != templateRoot)
            {
                foreach (VisualStateGroup group in VisualStateManager.GetVisualStateGroups(templateRoot))
                {
                    if (VisibilityGroupName == group.Name)
                    {
                        foreach (VisualState state in group.States)
                        {
                            if ((ClosedVisibilityStateName == state.Name) && (null != state.Storyboard))
                            {
                                _closedStoryboard = state.Storyboard;
                                _closedStoryboard.Completed += HandleClosedStoryboardCompleted;
                            }
                        }
                    }
                }
            }

            // Customize the ApplicationBar Buttons by providing the right text
            if (null != ApplicationBar)
            {
                foreach (object obj in ApplicationBar.Buttons)
                {
                    var button = obj as IApplicationBarIconButton;
                    if (null != button)
                    {
                        if ("DONE" == button.Text)
                        {
                            button.Text = Toolkit.Properties.Resources.DateTimePickerDoneText;
                            button.Click += HandleDoneButtonClick;
                        }
                        else if ("CANCEL" == button.Text)
                        {
                            button.Text = Toolkit.Properties.Resources.DateTimePickerCancelText;
                            button.Click += HandleCancelButtonClick;
                        }
                    }
                }
            }

            // Play the Open state
            VisualStateManager.GoToState(this, OpenVisibilityStateName, true);
        }

        private void HandleDataSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Push the selected item to all selectors
             var dataSource = (Toolkit.DataSource<T>)sender;

            _primarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
            _secondarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
            _tertiarySelectorPart.DataSource.SelectedItem = dataSource.SelectedItem;
        }

        private void HandleSelectorIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                // Ensure that only one selector is expanded at a time
                _primarySelectorPart.IsExpanded = (sender == _primarySelectorPart);
                _secondarySelectorPart.IsExpanded = (sender == _secondarySelectorPart);
                _tertiarySelectorPart.IsExpanded = (sender == _tertiarySelectorPart);
            }
        }

        private void HandleDoneButtonClick(object sender, EventArgs e)
        {
            // Commit the value and close
            Debug.Assert((_primarySelectorPart.DataSource.SelectedItem == _secondarySelectorPart.DataSource.SelectedItem) && (_secondarySelectorPart.DataSource.SelectedItem == _tertiarySelectorPart.DataSource.SelectedItem));
            _value = ((ValueWrapper<T>)_primarySelectorPart.DataSource.SelectedItem).Value;
            ClosePickerPage();
        }

        private void HandleCancelButtonClick(object sender, EventArgs e)
        {
            // Close without committing a value
            _value = null;
            ClosePickerPage();
        }

        /// <summary>
        /// Called when the Back key is pressed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            // Cancel back action so we can play the Close state animation (then go back)
            e.Cancel = true;
            ClosePickerPage();
        }

        private void ClosePickerPage()
        {
            // Play the Close state (if available)
            if (null != _closedStoryboard)
            {
                VisualStateManager.GoToState(this, ClosedVisibilityStateName, true);
            }
            else
            {
                HandleClosedStoryboardCompleted(null, null);
            }
        }

        private void HandleClosedStoryboardCompleted(object sender, EventArgs e)
        {
            // Close the picker page
            NavigationService.GoBack();
        }

        /// <summary>
        /// Gets a sequence of LoopingSelector parts ordered according to culture string for date/time formatting.
        /// </summary>
        /// <returns>LoopingSelectors ordered by culture-specific priority.</returns>
        protected abstract IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern();

        /// <summary>
        /// Gets a sequence of LoopingSelector parts ordered according to culture string for date/time formatting.
        /// </summary>
        /// <param name="pattern">Culture-specific date/time format string.</param>
        /// <param name="patternCharacters">Date/time format string characters for the primary/secondary/tertiary LoopingSelectors.</param>
        /// <param name="selectors">Instances for the primary/secondary/tertiary LoopingSelectors.</param>
        /// <returns>LoopingSelectors ordered by culture-specific priority.</returns>
        protected static IEnumerable<LoopingSelector> GetSelectorsOrderedByCulturePattern(string pattern, char[] patternCharacters, LoopingSelector[] selectors)
        {
            if (null == pattern)
            {
                throw new ArgumentNullException("pattern");
            }
            if (null == patternCharacters)
            {
                throw new ArgumentNullException("patternCharacters");
            }
            if (null == selectors)
            {
                throw new ArgumentNullException("selectors");
            }
            if (patternCharacters.Length != selectors.Length)
            {
                throw new ArgumentException("Arrays must contain the same number of elements.");
            }

            // Create a list of index and selector pairs
            var pairs = new List<Tuple<int, LoopingSelector>>(patternCharacters.Length);
            pairs.AddRange(patternCharacters.Select((t, i) => new Tuple<int, LoopingSelector>(pattern.IndexOf(t), selectors[i])));

            // Return the corresponding selectors in order
            return pairs.Where(p => -1 != p.Item1).OrderBy(p => p.Item1).Select(p => p.Item2).Where(s => null != s);
        }

        /// <summary>
        /// Gets or sets the DateTime to show in the picker page and to set when the user makes a selection.
        /// </summary>
        public virtual T? Value
        {
            get { return _value; }
            set
            {
                _value = value;
                var wrapper = GetNewWrapper(_value);

                if (wrapper == null ||
                    _primarySelectorPart == null ||
                    _secondarySelectorPart == null ||
                    _tertiarySelectorPart == null)
                    return;

                _primarySelectorPart.DataSource.SelectedItem = wrapper;
                _secondarySelectorPart.DataSource.SelectedItem = wrapper;
                _tertiarySelectorPart.DataSource.SelectedItem = wrapper;
            }
        }
        /// <summary>
        /// private value
        /// </summary>
        private T? _value;


        /// <summary>
        /// Instanciates a derived wrapper
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract ValueWrapper<T> GetNewWrapper(T? value);

        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            base.OnNavigatedFrom(e);

            // Save Value if navigating away from application
            if ("app://external/" == e.Uri.ToString())
            {
                State[StateKey_Value] = Value;
            }
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (null == e)
            {
                throw new ArgumentNullException("e");
            }

            base.OnNavigatedTo(e);
            InitDataSource();
            InitValue();

            // Restore Value if returning to application (to avoid inconsistent state)
            if (State.ContainsKey(StateKey_Value))
            {
                Value = State[StateKey_Value] as T?;

                // Back out from picker page for consistency with behavior of core pickers in this scenario
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }


        /// <summary>
        /// Hooks Datasources
        /// </summary>
        public abstract void InitDataSource();

        private void InitValue()
        {
            Value = Value.GetValueOrDefault();
        }
    }
}
