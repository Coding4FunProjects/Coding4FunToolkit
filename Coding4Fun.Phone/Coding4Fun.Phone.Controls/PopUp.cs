using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Clarity.Phone.Extensions;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Coding4Fun.Phone.Controls
{
    public abstract class PopUp<T, TPopUpResult> : Control
    {
        private DialogService _popUp;
		private PhoneApplicationPage _startingPage;
        private bool _alreadyFired;

        public bool IsOpen { get { return _popUp != null && _popUp.IsOpen; } }
        public bool IsAppBarVisible { get; set; }
    	
		// adjust for SIP
    	private bool _isCalculateFrameVerticalOffset;

    	protected bool IsCalculateFrameVerticalOffset
    	{
    		get { return _isCalculateFrameVerticalOffset; }
    		set
    		{
    			_isCalculateFrameVerticalOffset = value;

    			if (_isCalculateFrameVerticalOffset)
    			{
    				var bind = new System.Windows.Data.Binding("Y");
    				var frame = (Application.Current.RootVisual as Frame);

    				if (frame != null)
    				{
    					var transGroup = frame.RenderTransform as TransformGroup;

    					if (transGroup != null)
    					{
    						bind.Source = transGroup.Children.FirstOrDefault(t => t is TranslateTransform);
    						SetBinding(FrameTransformProperty, bind);
    					}
    				}
    			}
    		}
    	}

    	internal IApplicationBar AppBar { get; set; }
        protected internal bool IsBackKeyOverride { get; set; }

        protected DialogService.AnimationTypes AnimationType { get; set; }
        public event EventHandler<PopUpEventArgs<T, TPopUpResult>> Completed;
		public event EventHandler Opened;
		
    	public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_popUp != null)
            {
                _popUp.SetAlignmentsOnOverlay(HorizontalAlignment, VerticalAlignment);
            }
        }

        public virtual void OnCompleted(PopUpEventArgs<T, TPopUpResult> result)
        {
            _alreadyFired = true;

            if (Completed != null)
                Completed(this, result);
            
            if(_popUp != null)
                _popUp.Hide();
            
            if (_popUp != null && _popUp.BackButtonPressed)
                ResetWorldAndDestroyPopUp();
        }
		
		static void OnFrameTransformPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
			var sender = source as PopUp<T, TPopUpResult>;

			if (sender == null || sender._popUp == null)
				return;

			if (!sender.IsCalculateFrameVerticalOffset)
				return;

			sender._popUp.ControlVerticalOffset = -sender.FrameTransform;
			sender._popUp.CalculateVerticalOffset();
		}

    	public virtual void Show()
		{
			_popUp = new DialogService
			         	{
			         		AnimationType = AnimationType,
			         		Child = this,
			         		BackgroundBrush = Overlay,
			         		IsBackKeyOverride = IsBackKeyOverride
			         	};
			if (IsCalculateFrameVerticalOffset)
			{
				_popUp.ControlVerticalOffset = -FrameTransform;
			}

    		_popUp.Closed += PopUpClosed;
			_popUp.Opened += PopUpOpened;

			Dispatcher.BeginInvoke(
				() =>
					{
						if (!IsAppBarVisible)
						{
							AppBar = _popUp.Page.ApplicationBar;
							_popUp.Page.ApplicationBar = null;
						}

						_startingPage = _popUp.Page;

						_popUp.Show();
					});
		}

    	void PopUpOpened(object sender, EventArgs e)
		{
			if (Opened != null)
				Opened(sender, e);
		}
		
    	protected virtual TPopUpResult GetOnClosedValue()
        {
            return default(TPopUpResult);
        }

        public void Hide()
        {
            PopUpClosed(this, null);
        }

        void PopUpClosed(object sender, EventArgs e)
        {
        	if (!_alreadyFired)
			{
				OnCompleted(new PopUpEventArgs<T, TPopUpResult> {PopUpResult = GetOnClosedValue()});
				return;
			}

            ResetWorldAndDestroyPopUp();
        }

    	private void ResetWorldAndDestroyPopUp()
    	{
    		if (_popUp != null)
    		{
				if (!IsAppBarVisible && AppBar != null && _startingPage != null)
    			{
    				_startingPage.ApplicationBar = AppBar;
    			}
				
				_startingPage = null;
    			
                _popUp.Child = null;
    			_popUp = null;
    		}
    	}

		double FrameTransform
		{
			get { return (double)GetValue(FrameTransformProperty); }
			set { SetValue(FrameTransformProperty, value); }
		}

		static readonly DependencyProperty FrameTransformProperty = DependencyProperty.Register(
			  "FrameTransform",
			  typeof(double),
			  typeof(PopUp<T, TPopUpResult>),
			  new PropertyMetadata((double)0, OnFrameTransformPropertyChanged));

    	public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(
				(!DesignerProperties.IsInDesignTool) ? Application.Current.Resources["PhoneSemitransparentBrush"] : null));
	}
}