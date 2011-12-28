// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows.Controls;

using Coding4Fun.Phone.Controls.Primitives;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    public abstract class DataSource<T> : ILoopingSelectorDataSource where T : struct
    {
        private ValueWrapper<T> _selectedItem;

        public object GetNext(object relativeTo)
        {
            T? next = GetRelativeTo(((ValueWrapper<T>)relativeTo).Value, 1);
            return next.HasValue ? _selectedItem.CreateNew(next.Value) : null;
        }

        public object GetPrevious(object relativeTo)
        {
            T? next = GetRelativeTo(((ValueWrapper<T>)relativeTo).Value, -1);
            return next.HasValue ? _selectedItem.CreateNew(next.Value) : null;
        }

        protected abstract T? GetRelativeTo(T relativeDate, int delta);

        /// <summary>
        /// By default, the datasource is not empty -> we always show it
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != _selectedItem)
                {
                    var valueWrapper = value as ValueWrapper<T>;
                    if ((null == valueWrapper) || (null == _selectedItem) || !(valueWrapper.Value.Equals(_selectedItem.Value)))
                    {
                        object previousSelectedItem = _selectedItem;
                        _selectedItem = valueWrapper;

                        var handler = SelectionChanged;
                        if (null != handler)
                        {
                            handler(this, new SelectionChangedEventArgs(new[] { previousSelectedItem }, new object[] { _selectedItem }));
                        }
                    }
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    }
}
