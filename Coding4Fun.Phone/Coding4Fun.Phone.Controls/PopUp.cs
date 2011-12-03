using System;
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

            ResetWorldAndDestroyPopUp();
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

            if (!IsAppBarVisible)
            {
                AppBar = _popUp.Page.ApplicationBar;
                _popUp.Page.ApplicationBar = null;
            }

		    _popUp.Closed += _popUp_Closed;
			_popUp.Opened += _popUp_Opened;

            _startingPage = _popUp.Page;
			
            Dispatcher.BeginInvoke(() => _popUp.Show());
		}

		void _popUp_Opened(object sender, EventArgs e)
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
            _popUp_Closed(this, null);
        }

        void _popUp_Closed(object sender, EventArgs e)
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

    	public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));
    }
}