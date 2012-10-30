using System;

namespace Coding4Fun.Toolkit.Controls
{
    public class PopUpEventArgs<T, TPopUpResult> : EventArgs
    {
        public TPopUpResult PopUpResult { get; set; }
        public Exception Error { get; set; }
        public T Result { get; set; }
    }
}
