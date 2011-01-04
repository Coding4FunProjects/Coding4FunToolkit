using System;
using System.Windows.Controls;
using System.Windows.Media;

using Clarity.Phone.Extensions;

namespace Coding4Fun.Phone.Controls
{
    public class PopUpEventArgs<T> : EventArgs
    {
        public PopUpResult PopUpResult { get; set; }
        public Exception Error { get; set; }
        public T Result { get; set; }
    }

    public enum PopUpResult
    {
        OK,
        Cancelled
    }

    public abstract class PopUp<T> : Control
    {
        private DialogService _popUp;
        private bool _alreadyFired;
        public event EventHandler<PopUpEventArgs<T>> Completed;

        protected virtual void OnCompleted(PopUpEventArgs<T> result)
        {
            _alreadyFired = true;
            
            if (Completed != null)
                Completed.Invoke(this, result);
            
            _popUp.Hide();
        }

        public bool IsOpen { get { return _popUp != null && _popUp.IsOpen; } }

        public virtual void Show()
        {
            _popUp = new DialogService
                         {
                             Child = this,
                             BackgroundBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                         };

            _popUp.Show();

            _popUp.Closed += _popUp_Closed;
        }

        void _popUp_Closed(object sender, EventArgs e)
        {
            if(!_alreadyFired)
                OnCompleted(new PopUpEventArgs<T> { PopUpResult = PopUpResult.Cancelled });

            _popUp = null;
        }
    }
}