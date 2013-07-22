using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using Coding4Fun.Toolkit.Controls.Common;

namespace Coding4Fun.Toolkit.Controls
{
	public class MetroFlow : ItemsControl
	{
		public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
		public event EventHandler<SelectionTapEventArgs> SelectionTap;

		private GridLength _minimizedGridLength = new GridLength(48); //(GridLength)Application.Current.Resources["GridMinimized"];
		private readonly GridLength _maximizedGridLength = new GridLength(1, GridUnitType.Star);

		private Storyboard _animationBoard;
		private Grid _layoutGrid;

		private int _minimizingColumnIndex;
		private const string LayoutRootName = "LayoutRoot";

		public MetroFlow()
		{
			DefaultStyleKey = typeof(MetroFlow);
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			if (_layoutGrid == null) // have not loaded yet
				return;

			if (SelectedColumnIndex >= Items.Count)
			{
				SelectedColumnIndex = Items.Count - 1;
			}
			else if(Items.Count > 0 && SelectedColumnIndex < 0)
			{
				SelectedColumnIndex = 0;
			}

			ControlLoaded();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)   
		{
			return ((item is MetroFlowData));// || (item is MetroFlowItem));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_layoutGrid = GetTemplateChild(LayoutRootName) as Grid;

			if (_layoutGrid != null)
			{
				if (!ApplicationSpace.IsDesignMode || Items.Count > 0)
					ControlLoaded();
			}
		}

		#region AnimationDuration
		public TimeSpan AnimationDuration
		{
			get { return (TimeSpan)GetValue(AnimationDurationProperty); }
			set { SetValue(AnimationDurationProperty, value); }
		}

		// Using a DependencyProperty as the backing store for AnimationDuration.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AnimationDurationProperty =
			DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(MetroFlow), new PropertyMetadata(TimeSpan.FromMilliseconds(100)));
		#endregion

		#region SelectedColumnIndex
		public int SelectedColumnIndex
		{
			get { return (int)GetValue(SelectedColumnIndexProperty); }
			set { SetValue(SelectedColumnIndexProperty, value); }
		}

		public static readonly DependencyProperty SelectedColumnIndexProperty =
			DependencyProperty.Register("SelectedColumnIndex", typeof(int), typeof(MetroFlow), new PropertyMetadata(0, SelectedColumnIndexChanged));

		private static void SelectedColumnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var item = d as MetroFlow;

			if (item == null)
				return;

			// same value
			if (e.NewValue == e.OldValue)
				return;

			item.SelectionIndexChanged((int)e.OldValue);
		}

		private void SelectionIndexChanged(int oldIndex)
		{
			_minimizingColumnIndex = oldIndex;

			VerifyMinimizingColumnIndex();

			// fire SelectionChanged
			if (SelectionChanged != null)
			{
				var itemAdded = (Items.Count > 0 && SelectedColumnIndex >= 0) ? (MetroFlowData)Items[SelectedColumnIndex] : null;
				var itemRemoved = (Items.Count > 0 && _minimizingColumnIndex >= 0) ? (MetroFlowData)Items[_minimizingColumnIndex] : null;

				var eventArgs = new SelectionChangedEventArgs(
					new List<MetroFlowData> { itemRemoved }, // removed
					new List<MetroFlowData> { itemAdded }); // added

				SelectionChanged(this, eventArgs);
			}

			CreateSb(_layoutGrid, oldIndex);
		}

		#endregion

		#region column grow / shrink widths
		public double ExpandingWidth
		{
			get { return (double)GetValue(ExpandingWidthProperty); }
			set { SetValue(ExpandingWidthProperty, value); }
		}

		public static readonly DependencyProperty ExpandingWidthProperty =
			DependencyProperty.Register("ExpandingWidth", typeof(double), typeof(MetroFlow),
			new PropertyMetadata(ColumnGrowWidthChanged));

		public double CollapsingWidth
		{
			get { return (double)GetValue(CollapsingWidthProperty); }
			set { SetValue(CollapsingWidthProperty, value); }
		}

		public static readonly DependencyProperty CollapsingWidthProperty =
			DependencyProperty.Register("CollapsingWidth", typeof(double), typeof(MetroFlow),
			new PropertyMetadata(ColumnShrinkWidthChanged));

		private static void ColumnGrowWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var item = d as MetroFlow;

			if (item == null)
				return;

			var gridPanel = item._layoutGrid;

			if (gridPanel.ColumnDefinitions.Count > 1)
				ChangeColumnWidth(gridPanel.ColumnDefinitions[item.SelectedColumnIndex], (double)e.NewValue);
		}

		private static void ColumnShrinkWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var item = d as MetroFlow;

			if (item == null)
				return;

			item.VerifyMinimizingColumnIndex();

			var gridPanel = item._layoutGrid;

			if (gridPanel.ColumnDefinitions.Count > 1)
				ChangeColumnWidth(gridPanel.ColumnDefinitions[item._minimizingColumnIndex], (double)e.NewValue);
		}

		private void VerifyMinimizingColumnIndex()
		{
			if (_minimizingColumnIndex >= Items.Count)
			{
				_minimizingColumnIndex = Items.Count - 1;

				if (SelectedColumnIndex == _minimizingColumnIndex)
					_minimizingColumnIndex--;

				if (_minimizingColumnIndex < 0)
					_minimizingColumnIndex = 0;
			}
		}

		#endregion

		private static void ChangeColumnWidth(ColumnDefinition target, double value)
		{
			if (target == null)
				return;

			target.Width = new GridLength(value);
		}

		private void ControlLoaded()
		{
			var parentGrid = _layoutGrid;

			if (_layoutGrid == null || Items == null)
				return;

			Debug.WriteLine("Creating Control");

			parentGrid.ColumnDefinitions.Clear();
			parentGrid.Children.Clear();

			var index = 0;

			foreach (MetroFlowData item in Items)
			{
				var selectedCol = (index == SelectedColumnIndex);
				var colDef = new ColumnDefinition { Width = !selectedCol ? _minimizedGridLength : new GridLength(1, GridUnitType.Star) };
				parentGrid.ColumnDefinitions.Add(colDef);

				var control = new MetroFlowItem
				{
					ItemIndex = index + 1,
					ItemIndexOpacity = (!selectedCol) ? 1 : 0,
					ItemIndexVisibility = (!selectedCol) ? Visibility.Visible : Visibility.Collapsed,

					ImageSource = new BitmapImage(item.ImageUri),
					ImageOpacity = (selectedCol) ? 1 : 0,
					ImageVisibility = (selectedCol) ? Visibility.Visible : Visibility.Collapsed,

					Title = item.Title,
					TitleOpacity = (selectedCol) ? 1 : 0,
					TitleVisibility = (selectedCol) ? Visibility.Visible : Visibility.Collapsed,
				};

				control.SetValue(Grid.ColumnProperty, index);
				control.Tap += ItemTap;

				parentGrid.Children.Add(control);

				index++;
			}
		}

		private void ItemTap(object sender, GestureEventArgs e)
		{
			var item = sender as MetroFlowItem;

			if (item == null)
				return;

			var currentIndex = SelectedColumnIndex;

			// logic for selection change is in here
			SelectedColumnIndex = GetColumnIndex(item);

			// if not equal, user swipped, logic is in SelectedColumnIndex
			if (currentIndex != SelectedColumnIndex)
				return;

			// user tapped
			Debug.WriteLine(SelectedColumnIndex + "SelectionTap");

			// fire SelectionTapped
			if (SelectionTap != null)
				SelectionTap(this, new SelectionTapEventArgs { Index = SelectedColumnIndex, 
					Data = (MetroFlowData) Items[SelectedColumnIndex]
				});
		}

		private void HandleStoppingAnimation(int targetIndex)
		{
			if (_animationBoard == null || _animationBoard.GetCurrentState() != ClockState.Active)
				return;

			_animationBoard.Stop();
			AnimationCompleted(targetIndex);
		}

		private void CreateSb(Grid target, int oldIndex)
		{
			if (target == null || target.ColumnDefinitions.Count < SelectedColumnIndex)
				return;

			HandleStoppingAnimation(oldIndex);

			var sb = new Storyboard();

			var growingItem = GetMetroFlowItem(target, SelectedColumnIndex);
			var collapsingItem = GetMetroFlowItem(target, oldIndex);

			if (growingItem != null)
			{
				growingItem.ImageVisibility = Visibility.Visible;
				growingItem.TitleVisibility = Visibility.Visible;

				CreateDoubleAnimations(sb, growingItem, "ImageOpacity", 1, growingItem.ImageOpacity);
				CreateDoubleAnimations(sb, growingItem, "TitleOpacity", 1, growingItem.TitleOpacity);
				CreateDoubleAnimations(sb, growingItem, "ItemIndexOpacity", 0, growingItem.ItemIndexOpacity);
			}

			if (collapsingItem != null)
			{
				collapsingItem.ItemIndexVisibility = Visibility.Visible;

				CreateDoubleAnimations(sb, collapsingItem, "ImageOpacity", 0, collapsingItem.ImageOpacity);
				CreateDoubleAnimations(sb, collapsingItem, "TitleOpacity", 0, collapsingItem.TitleOpacity);
				CreateDoubleAnimations(sb, collapsingItem, "ItemIndexOpacity", 1, collapsingItem.ItemIndexOpacity);
			}

			var doubleShrink = CreateDoubleAnimations(sb, this,
													  toValue: _minimizedGridLength.Value,
													  propertyPath: "CollapsingWidth");

			var doubleGrow = CreateDoubleAnimations(sb, this,
													fromValue: _minimizedGridLength.Value,
													propertyPath: "ExpandingWidth");

			sb.Completed += (sbSender, sbEventArgs) => AnimationCompleted();


			if (collapsingItem != null)
			{
				var targetGrowWidth = collapsingItem.ActualWidth;
				doubleGrow.To = targetGrowWidth;
				doubleShrink.From = targetGrowWidth;
			}

			UpdateLayout(); // since i changed visibility
			_animationBoard = sb;
			_animationBoard.Begin();
		}

		private DoubleAnimation CreateDoubleAnimations(Storyboard sb, DependencyObject target, string propertyPath, double toValue = 0, double fromValue = 0)
		{
			var doubleAni = new DoubleAnimation
			{
				To = toValue,
				From = fromValue,
				Duration = AnimationDuration
			};

			Storyboard.SetTarget(doubleAni, target);
			Storyboard.SetTargetProperty(doubleAni, new PropertyPath(propertyPath));

			sb.Children.Add(doubleAni);
			return doubleAni;
		}

		private static MetroFlowItem GetMetroFlowItem(Panel target, int index)
		{
			return target.Children.Where(item => GetColumnIndex(item) == index).SingleOrDefault() as MetroFlowItem;
		}

		private static int GetColumnIndex(DependencyObject element)
		{
			return (int)element.GetValue(Grid.ColumnProperty);
		}

		private void AnimationCompleted()
		{
			AnimationCompleted(SelectedColumnIndex);
		}

		private void AnimationCompleted(int column)
		{
			for (var i = 0; i < _layoutGrid.ColumnDefinitions.Count; i++)
			{
				_layoutGrid.ColumnDefinitions[i].Width = (i != column) ? _minimizedGridLength : _maximizedGridLength;
			}

			foreach (var item in _layoutGrid.Children.Select(t => t as MetroFlowItem))
			{
				SetMetroFlowControlItemProperties(item, GetColumnIndex(item) == column);
			}

			UpdateLayout(); // since i'm altering visibility
		}

		private static void SetMetroFlowControlItemProperties(MetroFlowItem item, bool isLarge)
		{
			if (item == null)
				return;

			item.ImageVisibility =
				item.TitleVisibility = isLarge ? Visibility.Visible : Visibility.Collapsed;

			item.TitleOpacity =
				item.ImageOpacity = isLarge ? 1 : 0;

			item.ItemIndexVisibility = isLarge ? Visibility.Collapsed : Visibility.Visible;
			item.ItemIndexOpacity = isLarge ? 0 : 1;
		}
	}
}