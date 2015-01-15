using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding4Fun.Toolkit.Controls
{
    public class PropertyChangedEventArgs<T> : EventArgs
	{
        public PropertyChangedEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
		public T OldValue { get; set; }
		public T NewValue { get; set; }
	}
}
