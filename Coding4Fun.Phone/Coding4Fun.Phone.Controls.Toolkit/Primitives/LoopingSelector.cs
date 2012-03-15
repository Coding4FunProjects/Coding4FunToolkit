// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using ILoopingSelectorDataSource = Coding4Fun.Phone.Controls.Primitives.ILoopingSelectorDataSource;


namespace Coding4Fun.Phone.Controls.Toolkit.Primitives
{
	/// <summary>
	/// An infinitely scrolling, UI- and data-virtualizing selection control.
	/// </summary>
	/// <QualityBand>Preview</QualityBand>
	[TemplatePart(Name = ItemsPanelName, Type = typeof(Panel))]
	[TemplatePart(Name = CenteringTransformName, Type = typeof(TranslateTransform))]
	[TemplatePart(Name = PanningTransformName, Type = typeof(TranslateTransform))]
	public class LoopingSelector : Control
	{
		// The names of the template parts
		private const string ItemsPanelName = "ItemsPanel";
		private const string CenteringTransformName = "CenteringTransform";
		private const string PanningTransformName = "PanningTransform";

		// Amount of finger movement before the manipulation is considered a dragging manipulation.
		private const double DragSensitivity = 12;
		
		private static readonly Duration _selectDuration = new Duration(TimeSpan.FromMilliseconds(500));
		private readonly IEasingFunction _selectEase = new ExponentialEase() { EasingMode = EasingMode.EaseInOut };

		private static readonly Duration _panDuration = new Duration(TimeSpan.FromMilliseconds(100));
		private readonly IEasingFunction _panEase = new ExponentialEase();

		private DoubleAnimation _panelAnimation;
		private Storyboard _panelStoryboard;

		private Panel _itemsPanel;
		private TranslateTransform _panningTransform;
		private TranslateTransform _centeringTransform;

		private bool _isSelecting;
		private LoopingSelectorItem _selectedItem;

		private Queue<LoopingSelectorItem> _temporaryItemsPool;

		private double _minimumPanelScroll = float.MinValue;
		private double _maximumPanelScroll = float.MaxValue;

		private int _additionalItemsCount = 0;

		private bool _isAnimating;

		private double _dragTarget;

		// Once the user starts dragging horizontally, he is not allowed to drag vertically
		// until he completes his touch gesture and starts again.
		private bool _isAllowedToDragVertically = true;
		private bool _isAllowedToDragHorizontally = true;

		// Specify whether or not the user is dragging with his finger.
		private bool _isDragging;

		private enum State
		{
			Normal,
			Expanded,
			Dragging,
			Snapping,
			Flicking
		}

		private State _state;

		/// <summary>
		/// The data source that the this control is the view for.
		/// </summary>
		public ILoopingSelectorDataSource DataSource
		{
			get { return (ILoopingSelectorDataSource)GetValue(DataSourceProperty); }
			set
			{
				if (DataSource != null)
				{
					DataSource.SelectionChanged -= value_SelectionChanged;
				}

				SetValue(DataSourceProperty, value);

				if (value != null)
				{
					value.SelectionChanged += value_SelectionChanged;
				}
			}
		}

		void value_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!IsReady)
			{
				return;
			}

			if (!_isSelecting && e.AddedItems.Count == 1)
			{
				object selection = e.AddedItems[0];

				foreach (LoopingSelectorItem child in _itemsPanel.Children)
				{
					if (child.DataContext == selection)
					{
						SelectAndSnapTo(child);
						return;
					}
				}
				UpdateData();
			}
		}

		/// <summary>
		/// The DataSource DependencyProperty
		/// </summary>
		public static readonly DependencyProperty DataSourceProperty =
			DependencyProperty.Register("DataSource", typeof(ILoopingSelectorDataSource), typeof(LoopingSelector), new PropertyMetadata(null, OnDataModelChanged));

		private static void OnDataModelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			LoopingSelector picker = (LoopingSelector)obj;
			picker.UpdateData();
		}

		/// <summary>
		/// The ItemTemplate property
		/// </summary>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		/// <summary>
		/// The ItemTemplate DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(LoopingSelector), new PropertyMetadata(null));



		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof(Orientation), typeof(LoopingSelector), new PropertyMetadata(Orientation.Vertical));



		/// <summary>
		/// The size of the items, excluding the ItemMargin.
		/// </summary>
		public Size ItemSize { get; set; }

		/// <summary>
		/// The margin around the items, to be a part of the touchable area.
		/// </summary>
		public Thickness ItemMargin { get; set; }

		/// <summary>
		/// Creates a new LoopingSelector.
		/// </summary>
		public LoopingSelector()
		{
			_isAllowedToDragVertically = Orientation == Orientation.Vertical;
			_isAllowedToDragHorizontally = Orientation == Orientation.Horizontal;

			DefaultStyleKey = typeof(LoopingSelector);
			CreateEventHandlers();
		}

		/// <summary>
		/// The IsExpanded property controls the expanded state of this control.
		/// </summary>
		public bool IsExpanded
		{
			get { return (bool)GetValue(IsExpandedProperty); }
			set { SetValue(IsExpandedProperty, value); }
		}

		/// <summary>
		/// The IsExpanded DependencyProperty.
		/// </summary>
		public static readonly DependencyProperty IsExpandedProperty =
			DependencyProperty.Register("IsExpanded", typeof(bool), typeof(LoopingSelector), new PropertyMetadata(false, OnIsExpandedChanged));

		/// <summary>
		/// The IsExpandedChanged event will be raised whenever the IsExpanded state changes.
		/// </summary>
		public event DependencyPropertyChangedEventHandler IsExpandedChanged;

		private static void OnIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			LoopingSelector picker = (LoopingSelector)sender;

			picker.UpdateItemState();
			if (!picker.IsExpanded)
			{
				picker.SelectAndSnapToClosest();
			}

			if (picker._state == State.Normal || picker._state == State.Expanded)
			{
				picker._state = picker.IsExpanded ? State.Expanded : State.Normal;
			}

			var listeners = picker.IsExpandedChanged;
			if (listeners != null)
			{
				listeners(picker, e);
			}
		}

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:FrameworkElement.ApplyTemplate()"/>.
		/// In simplest terms, this means the method is called just before a UI element displays in an application.
		/// For more information, see Remarks.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Find the template parts. Create dummy objects if parts are missing to avoid
			// null checks throughout the code (although we can't escape them completely.)
			_itemsPanel = GetTemplateChild(ItemsPanelName) as Panel ?? new Canvas();
			_centeringTransform = GetTemplateChild(CenteringTransformName) as TranslateTransform ?? new TranslateTransform();
			_panningTransform = GetTemplateChild(PanningTransformName) as TranslateTransform ?? new TranslateTransform();

			CreateVisuals();
		}

		void LoopingSelector_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (_isAnimating)
			{
				if (Orientation == Orientation.Vertical)
				{
					double y = _panningTransform.Y;
					StopAnimation();
					_panningTransform.Y = y;
				}
				else
				{
					double x = _panningTransform.X;
					StopAnimation();
					_panningTransform.X = x;
				}

				_isAnimating = false;
				_state = State.Dragging;
			}
		}

		void LoopingSelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_selectedItem != sender && _state == State.Dragging && !_isAnimating)
			{
				SelectAndSnapToClosest();
				_state = State.Expanded;
			}
		}

		#region Touch Events
		private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			if (_panningTransform != null)
			{
				SelectAndSnapToClosest();
				e.Handled = true;
			}
		}

		private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			_isAllowedToDragVertically = true;
			_isAllowedToDragHorizontally = true;

			_isDragging = false;
		}

		private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			if (Orientation == Orientation.Vertical)
			{
				if (_isDragging)
				{
					AnimatePanel(_panDuration, _panEase, _dragTarget += e.DeltaManipulation.Translation.Y);
					e.Handled = true;
				}
				else if (Math.Abs(e.CumulativeManipulation.Translation.X) > DragSensitivity)
				{
					_isAllowedToDragVertically = false;
				}
				else if (_isAllowedToDragVertically && Math.Abs(e.CumulativeManipulation.Translation.Y) > DragSensitivity)
				{
					_isDragging = true;
					_state = State.Dragging;
					e.Handled = true;
					_selectedItem = null;

					if (!IsExpanded)
					{
						IsExpanded = true;
					}

					_dragTarget = _panningTransform.Y;
					UpdateItemState();
				}
			}
			else
			{
				if (_isDragging)
				{
					AnimatePanel(_panDuration, _panEase, _dragTarget += e.DeltaManipulation.Translation.X);
					e.Handled = true;
				}
				else if (Math.Abs(e.CumulativeManipulation.Translation.Y) > DragSensitivity)
				{
					_isAllowedToDragHorizontally = false;
				}
				else if (_isAllowedToDragHorizontally && Math.Abs(e.CumulativeManipulation.Translation.X) > DragSensitivity)
				{
					_isDragging = true;
					_state = State.Dragging;
					e.Handled = true;
					_selectedItem = null;

					if (!IsExpanded)
					{
						IsExpanded = true;
					}

					_dragTarget = _panningTransform.X;
					UpdateItemState();
				}
			}
		}

		private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			if (_isDragging)
			{
				// See if it was a flick
				if (e.IsInertial)
				{
					_state = State.Flicking;
					_selectedItem = null;

					if (!IsExpanded)
					{
						IsExpanded = true;
					}

					Point velocity;

					if (Orientation == Orientation.Vertical)
						velocity = new Point(0, e.FinalVelocities.LinearVelocity.Y);
					else
						velocity = new Point(e.FinalVelocities.LinearVelocity.X, 0);

					double flickDuration = PhysicsConstants.GetStopTime(velocity);
					Point flickEndPoint = PhysicsConstants.GetStopPoint(velocity);
					IEasingFunction flickEase = PhysicsConstants.GetEasingFunction(flickDuration);

					double to;
					if (Orientation == Orientation.Vertical)
						to = _panningTransform.Y + flickEndPoint.Y;
					else
						to = _panningTransform.X + flickEndPoint.X;

					AnimatePanel(new Duration(TimeSpan.FromSeconds(flickDuration)), flickEase, to);

					e.Handled = true;

					_selectedItem = null;
					UpdateItemState();
				}

				if (_state == State.Dragging)
				{
					SelectAndSnapToClosest();
				}

				_state = State.Expanded;
			}
		}
		#endregion

		void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (Orientation == Orientation.Vertical)
				_centeringTransform.Y = Math.Round(e.NewSize.Height / 2);
			else
				_centeringTransform.X = Math.Round(e.NewSize.Width / 2);

			Clip = new RectangleGeometry() { Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height) };
			UpdateData();
		}

		void OnWrapperClick(object sender, EventArgs e)
		{
			if (_state == State.Normal)
			{
				_state = State.Expanded;
				IsExpanded = true;
			}
			else if (_state == State.Expanded)
			{
				if (!_isAnimating && sender == _selectedItem)
				{
					_state = State.Normal;
					IsExpanded = false;
				}
				else if (sender != _selectedItem && !_isAnimating)
				{
					SelectAndSnapTo((LoopingSelectorItem)sender);
				}
			}
		}

		private void SelectAndSnapTo(LoopingSelectorItem item)
		{
			if (item == null)
			{
				return;
			}

			if (_selectedItem != null)
			{
				_selectedItem.SetState(IsExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
			}

			if (_selectedItem != item)
			{
				_selectedItem = item;
				// Update DataSource.SelectedItem aynchronously so that animations have a chance to start.
				Dispatcher.BeginInvoke(() =>
				{
					_isSelecting = true;
					DataSource.SelectedItem = item.DataContext;
					_isSelecting = false;
				});
			}

			_selectedItem.SetState(LoopingSelectorItem.State.Selected, true);

			TranslateTransform transform = item.Transform;
			if (transform != null)
			{
				if (Orientation == Orientation.Vertical)
				{
					double newPosition = -transform.Y - Math.Round(item.ActualHeight / 2);
					if (_panningTransform.Y != newPosition)
					{
						AnimatePanel(_selectDuration, _selectEase, newPosition);
					}
				}
				else
				{
					double newPosition = -transform.X - Math.Round(item.ActualWidth / 2);
					if (_panningTransform.X != newPosition)
					{
						AnimatePanel(_selectDuration, _selectEase, newPosition);
					}
				}
			}
		}

		private void UpdateData()
		{
			if (!IsReady)
			{
				return;
			}

			// Save all items
			_temporaryItemsPool = new Queue<LoopingSelectorItem>(_itemsPanel.Children.Count);
			foreach (LoopingSelectorItem item in _itemsPanel.Children)
			{
				if (item.GetState() == LoopingSelectorItem.State.Selected)
				{
					item.SetState(LoopingSelectorItem.State.Normal, false);
				}

				_temporaryItemsPool.Enqueue(item);
				item.Remove();
			}

			_itemsPanel.Children.Clear();
			StopAnimation();

			if (Orientation == Orientation.Vertical)
				_panningTransform.Y = 0;
			else
				_panningTransform.X = 0;

			// Reset the extents
			_minimumPanelScroll = float.MinValue;
			_maximumPanelScroll = float.MaxValue;

			Balance();
		}

		private void AnimatePanel(Duration duration, IEasingFunction ease, double to)
		{
			// Be sure not to run past the first or last items
			double newTo = Math.Max(_minimumPanelScroll, Math.Min(_maximumPanelScroll, to));
			if (to != newTo)
			{
				// Adjust the duration

				double originalDelta;
				double modifiedDelta;

				if (Orientation == Orientation.Vertical)
				{
					originalDelta = Math.Abs(_panningTransform.Y - to);
					modifiedDelta = Math.Abs(_panningTransform.Y - newTo);
				}
				else
				{
					originalDelta = Math.Abs(_panningTransform.X - to);
					modifiedDelta = Math.Abs(_panningTransform.X - newTo);
				}

				double factor = modifiedDelta / originalDelta;

				duration = new Duration(TimeSpan.FromMilliseconds(duration.TimeSpan.Milliseconds * factor));

				to = newTo;
			}

			double from;

			if (Orientation == Orientation.Vertical)
				from = _panningTransform.Y;
			else
				from = _panningTransform.X;

			StopAnimation();
			CompositionTarget.Rendering += AnimationPerFrameCallback;

			_panelAnimation.Duration = duration;
			_panelAnimation.EasingFunction = ease;
			_panelAnimation.From = from;
			_panelAnimation.To = to;

			_panelStoryboard.Begin();
			_panelStoryboard.SeekAlignedToLastTick(TimeSpan.Zero);

			_isAnimating = true;
		}

		private void StopAnimation()
		{
			_panelStoryboard.Stop();
			CompositionTarget.Rendering -= AnimationPerFrameCallback;
		}

		private void Brake(double newStoppingPoint)
		{
			if (_panelAnimation.To != null && _panelAnimation.From != null)
			{
				double originalDelta = _panelAnimation.To.Value - _panelAnimation.From.Value;
				double remainingDelta = newStoppingPoint;

				if (Orientation == Orientation.Vertical)
					remainingDelta -= _panningTransform.Y;
				else
					remainingDelta -= _panningTransform.X;

				double factor = remainingDelta / originalDelta;

				Duration duration = new Duration(TimeSpan.FromMilliseconds(_panelAnimation.Duration.TimeSpan.Milliseconds * factor));

				AnimatePanel(duration, _panelAnimation.EasingFunction, newStoppingPoint);
			}
		}

		private bool IsReady
		{
			get
			{
				bool isValidSizing = (Orientation == Orientation.Vertical) ? ActualHeight > 0 : ActualWidth > 0;

				return isValidSizing && DataSource != null && _itemsPanel != null;
			}
		}

		/// <summary>
		/// Balances the items.
		/// </summary>
		private void Balance()
		{
			if (!IsReady)
			{
				return;
			}

			double actualItemWidth = ActualItemWidth;
			double actualItemHeight = ActualItemHeight;

			if (Orientation == Orientation.Vertical)
				_additionalItemsCount = (int)Math.Round((ActualHeight * 1.5) / actualItemHeight);
			else
				_additionalItemsCount = (int)Math.Round((ActualWidth * 1.5) / actualItemWidth);

			LoopingSelectorItem closestToMiddle = null;
			int closestToMiddleIndex = -1;

			if (_itemsPanel.Children.Count == 0)
			{
				// We need to get the selection and start from there
				closestToMiddleIndex = 0;
				_selectedItem = closestToMiddle = CreateAndAddItem(_itemsPanel, DataSource.SelectedItem);

				if (Orientation == Orientation.Vertical)
				{
					closestToMiddle.Transform.Y = -actualItemHeight / 2;
					closestToMiddle.Transform.X = (ActualWidth - actualItemWidth) / 2;
				}
				else
				{
					closestToMiddle.Transform.X = -actualItemWidth / 2;
					closestToMiddle.Transform.Y = (ActualHeight - actualItemHeight) / 2;
				}

				closestToMiddle.SetState(LoopingSelectorItem.State.Selected, false);
			}
			else
			{
				closestToMiddleIndex = GetClosestItem();
				closestToMiddle = (LoopingSelectorItem)_itemsPanel.Children[closestToMiddleIndex];
			}

			int itemsBeforeCount;
			LoopingSelectorItem firstItem = GetFirstItem(closestToMiddle, out itemsBeforeCount);

			int itemsAfterCount;
			LoopingSelectorItem lastItem = GetLastItem(closestToMiddle, out itemsAfterCount);

			// Does the top need items?
			if (itemsBeforeCount < itemsAfterCount || itemsBeforeCount < _additionalItemsCount)
			{
				while (itemsBeforeCount < _additionalItemsCount)
				{
					object newData = DataSource.GetPrevious(firstItem.DataContext);
					if (newData == null)
					{
						// There may be room to display more items, but there is no more data.
						if (Orientation == Orientation.Vertical)
							_maximumPanelScroll = -firstItem.Transform.Y - actualItemHeight / 2;
						else
							_maximumPanelScroll = -firstItem.Transform.X - actualItemWidth / 2;

						if (_panelAnimation.To != null && (_isAnimating && _panelAnimation.To.Value > _maximumPanelScroll))
						{
							Brake(_maximumPanelScroll);
						}
						break;
					}

					LoopingSelectorItem newItem = null;

					// Can an item from the bottom be re-used?
					if (itemsAfterCount > _additionalItemsCount)
					{
						newItem = lastItem;
						lastItem = lastItem.Previous;
						newItem.Remove();
						newItem.Content = newItem.DataContext = newData;
					}
					else
					{
						// Make a new item
						newItem = CreateAndAddItem(_itemsPanel, newData);

						if (Orientation == Orientation.Vertical)
							newItem.Transform.X = (ActualWidth - actualItemWidth) / 2;
						else
							newItem.Transform.Y = (ActualHeight - actualItemHeight) / 2;
					}

					// Put the new item on the top
					if (Orientation == Orientation.Vertical)
						newItem.Transform.Y = firstItem.Transform.Y - actualItemHeight;
					else
						newItem.Transform.X = firstItem.Transform.X - actualItemWidth;

					newItem.InsertBefore(firstItem);
					firstItem = newItem;

					++itemsBeforeCount;
				}
			}

			// Does the bottom need items?
			if (itemsAfterCount < itemsBeforeCount || itemsAfterCount < _additionalItemsCount)
			{
				while (itemsAfterCount < _additionalItemsCount)
				{
					object newData = DataSource.GetNext(lastItem.DataContext);
					if (newData == null)
					{
						// There may be room to display more items, but there is no more data.
						if (Orientation == Orientation.Vertical)
							_minimumPanelScroll = -lastItem.Transform.Y - actualItemHeight / 2;
						else
							_minimumPanelScroll = -lastItem.Transform.X - actualItemWidth / 2;

						if (_panelAnimation.To != null && (_isAnimating && _panelAnimation.To.Value < _minimumPanelScroll))
						{
							Brake(_minimumPanelScroll);
						}
						break;
					}

					LoopingSelectorItem newItem = null;

					// Can an item from the top be re-used?
					if (itemsBeforeCount > _additionalItemsCount)
					{
						newItem = firstItem;
						firstItem = firstItem.Next;
						newItem.Remove();
						newItem.Content = newItem.DataContext = newData;
					}
					else
					{
						// Make a new item
						newItem = CreateAndAddItem(_itemsPanel, newData);

						if (Orientation == Orientation.Vertical)
							newItem.Transform.X = (ActualWidth - actualItemWidth) / 2;
						else
							newItem.Transform.Y = (ActualHeight - ActualItemHeight) / 2;
					}

					// Put the new item on the bottom
					if (Orientation == Orientation.Vertical)
						newItem.Transform.Y = lastItem.Transform.Y + actualItemHeight;
					else
						newItem.Transform.X = lastItem.Transform.X + actualItemWidth;

					newItem.InsertAfter(lastItem);
					lastItem = newItem;

					++itemsAfterCount;
				}
			}

			_temporaryItemsPool = null;
		}

		private static LoopingSelectorItem GetFirstItem(LoopingSelectorItem item, out int count)
		{
			count = 0;
			while (item.Previous != null)
			{
				++count;
				item = item.Previous;
			}

			return item;
		}

		private static LoopingSelectorItem GetLastItem(LoopingSelectorItem item, out int count)
		{
			count = 0;
			while (item.Next != null)
			{
				++count;
				item = item.Next;
			}

			return item;
		}

		void AnimationPerFrameCallback(object sender, EventArgs e)
		{
			Balance();
		}

		private int GetClosestItem()
		{
			if (!IsReady)
			{
				return -1;
			}

			int count = _itemsPanel.Children.Count;

			double panelY = _panningTransform.Y;
			double panelX = _panningTransform.X;

			double halfHeight = ActualItemHeight / 2;
			double halfWidth = ActualItemWidth / 2;

			int found = -1;
			double closestDistance = double.MaxValue;

			for (int index = 0; index < count; ++index)
			{
				LoopingSelectorItem wrapper = (LoopingSelectorItem)_itemsPanel.Children[index];

				double distance;

				if (Orientation == Orientation.Vertical)
					distance = Math.Abs((wrapper.Transform.Y + halfHeight) + panelY);
				else
					distance = Math.Abs((wrapper.Transform.X + halfWidth) + panelX);

				if (distance <= halfHeight)
				{
					found = index;
					break;
				}

				if (closestDistance > distance)
				{
					closestDistance = distance;
					found = index;
				}
			}

			return found;
		}

		void PanelStoryboardCompleted(object sender, EventArgs e)
		{
			CompositionTarget.Rendering -= AnimationPerFrameCallback;

			_isAnimating = false;

			if (_state != State.Dragging)
			{
				SelectAndSnapToClosest();
			}
		}

		private void SelectAndSnapToClosest()
		{
			if (!IsReady)
			{
				return;
			}

			int index = GetClosestItem();

			if (index == -1)
			{
				return;
			}

			LoopingSelectorItem item = (LoopingSelectorItem)_itemsPanel.Children[index];
			SelectAndSnapTo(item);
		}

		private void UpdateItemState()
		{
			if (!IsReady)
			{
				return;
			}

			bool isExpanded = IsExpanded;

			foreach (LoopingSelectorItem child in _itemsPanel.Children)
			{
				if (child == _selectedItem)
				{
					child.SetState(LoopingSelectorItem.State.Selected, true);
				}
				else
				{
					child.SetState(isExpanded ? LoopingSelectorItem.State.Expanded : LoopingSelectorItem.State.Normal, true);
				}
			}
		}

		private double ActualItemWidth { get { return Padding.Left + Padding.Right + ItemSize.Width; } }
		private double ActualItemHeight { get { return Padding.Top + Padding.Bottom + ItemSize.Height; } }

		private void CreateVisuals()
		{
			_panelAnimation = new DoubleAnimation();

			Storyboard.SetTarget(_panelAnimation, _panningTransform);
			string propName = (Orientation == Orientation.Vertical) ? "Y" : "X";

			Storyboard.SetTargetProperty(_panelAnimation, new PropertyPath(propName));

			_panelStoryboard = new Storyboard();
			_panelStoryboard.Children.Add(_panelAnimation);
			_panelStoryboard.Completed += PanelStoryboardCompleted;
		}

		private void CreateEventHandlers()
		{

			SizeChanged += OnSizeChanged;

			this.ManipulationStarted += OnManipulationStarted;
			this.ManipulationCompleted += OnManipulationCompleted;
			this.ManipulationDelta += OnManipulationDelta;

			this.Tap += OnTap;

			AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(LoopingSelector_MouseLeftButtonDown), true);
			AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(LoopingSelector_MouseLeftButtonUp), true);
		}

		private LoopingSelectorItem CreateAndAddItem(Panel parent, object content)
		{
			bool reuse = _temporaryItemsPool != null && _temporaryItemsPool.Count > 0;

			LoopingSelectorItem wrapper = reuse ? _temporaryItemsPool.Dequeue() : new LoopingSelectorItem();

			if (!reuse)
			{
				wrapper.ContentTemplate = this.ItemTemplate;
				wrapper.Width = ItemSize.Width;
				wrapper.Height = ItemSize.Height;
				wrapper.Padding = ItemMargin;

				wrapper.Click += OnWrapperClick;
			}

			wrapper.DataContext = wrapper.Content = content;

			parent.Children.Add(wrapper); // Need to do this before calling ApplyTemplate
			if (!reuse)
			{
				wrapper.ApplyTemplate();
			}

			return wrapper;
		}
	}
}
