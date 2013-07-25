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

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Toolkit.Controls.Primitives
{
    public abstract class ValuePickerBasePage<T> : PhoneApplicationPage, IValuePickerPage<T> where T : struct
    {
        private const string VisibilityGroupName = "VisibilityStates";
        private const string OpenVisibilityStateName = "Open";
        private const string ClosedVisibilityStateName = "Closed";

        private static readonly string StateKeyValue = "ValuePickerPageBase_State_Value" + typeof(T);

        private LoopingSelector _primarySelectorPart;
        private LoopingSelector _secondarySelectorPart;
        private LoopingSelector _tertiarySelectorPart;
        private Storyboard _closedStoryboard;

        protected ValuePickerBasePage()
        {
            DataContext = this; // needed for binding DialogTitle
        }

    	/// <summary>
        /// Initializes the ValuePickerPageBase class; must be called from the subclass's constructor.
        /// </summary>
        /// <param name="primarySelector">Primary selector.</param>
        /// <param name="secondarySelector">Secondary selector.</param>
        /// <param name="tertiarySelector">Tertiary selector.</param>
        protected void InitializeValuePickerPage(LoopingSelector primarySelector, LoopingSelector secondarySelector, LoopingSelector tertiarySelector)
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
                                _closedStoryboard.Completed += OnClosedStoryboardCompleted;
                            }
                        }
                    }
                }
            }

            // Customize the ApplicationBar Buttons by providing the right text
            if (null != ApplicationBar)
            {
                foreach (var obj in ApplicationBar.Buttons)
                {
                    var button = obj as IApplicationBarIconButton;

                    if (null != button)
                    {
                        switch (button.Text)
                        {
                        	case "DONE":
                        		button.Text = Properties.Resources.DoneText;
                        		button.Click += OnDoneButtonClick;
                        		break;
                        	case "CANCEL":
                        		button.Text = Properties.Resources.CancelText;
                        		button.Click += OnCancelButtonClick;
                        		break;
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
            var dataSource = (DataSource<T>)sender;

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

        private void OnDoneButtonClick(object sender, EventArgs e)
        {
            // Commit the value and close
            Debug.Assert((_primarySelectorPart.DataSource.SelectedItem == _secondarySelectorPart.DataSource.SelectedItem) && (_secondarySelectorPart.DataSource.SelectedItem == _tertiarySelectorPart.DataSource.SelectedItem));
            
            Value = ((ValueWrapper<T>)_primarySelectorPart.DataSource.SelectedItem).Value;

            ClosePickerPage();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            // Close without committing a value
            Value = null;
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
                OnClosedStoryboardCompleted(null, null);
            }
        }

        private void OnClosedStoryboardCompleted(object sender, EventArgs e)
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
			for (int i = 0; i < patternCharacters.Length; i++)
			{
				pairs.Add(new Tuple<int, LoopingSelector>(pattern.IndexOf(patternCharacters[i]), selectors[i]));
			}

			// Return the corresponding selectors in order
			return pairs.Where(p => -1 != p.Item1).OrderBy(p => p.Item1).Select(p => p.Item2).Where(s => null != s);
        }

        /// <summary>
        /// Gets or sets the Value to show in the picker page and to set when the user makes a selection.
        /// </summary>
        public virtual T? Value
        {
            get { return _value; }
            set
            {
            	_value = value;

            	SetDataSources();
            }
        }
        private T? _value;

    	private void SetDataSources()
    	{
			var wrapper = GetNewWrapper(Value);

    		if (wrapper == null ||
    		    _primarySelectorPart == null ||
    		    _secondarySelectorPart == null ||
    		    _tertiarySelectorPart == null)
    			return;

    		_primarySelectorPart.DataSource.SelectedItem = wrapper;
    		_secondarySelectorPart.DataSource.SelectedItem = wrapper;
    		_tertiarySelectorPart.DataSource.SelectedItem = wrapper;
    	}

		public string DialogTitle
		{
			get { return (string)GetValue(DialogTitleProperty); }
			set { SetValue(DialogTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DialogTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DialogTitleProperty =
			DependencyProperty.Register("DialogTitle", typeof(string), typeof(ValuePickerBasePage<T>), new PropertyMetadata(""));
		
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
                State[StateKeyValue] = Value;
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

        	var cancelInit = false;

        	// Restore Value if returning to application (to avoid inconsistent state)
        	if (State.ContainsKey(StateKeyValue))
        	{
        		Value = State[StateKeyValue] as T?;

        		// Back out from picker page for consistency with behavior of core pickers in this scenario
        		if (NavigationService.CanGoBack)
        		{
        			NavigationService.GoBack();
        			cancelInit = true;
        		}
        	}

        	if (cancelInit)
				return;

        	InitDataSource();
			SetDataSources();
        }


    	/// <summary>
        /// Hooks Datasources
        /// </summary>
        public abstract void InitDataSource();

		/// <summary>
		/// Sets the selectors and title flow direction.
		/// </summary>
		/// <param name="flowDirection">Flow direction to set.</param>
		public abstract void SetFlowDirection(FlowDirection flowDirection);
    }
}
