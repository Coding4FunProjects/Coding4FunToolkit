using System;
using System.Windows.Input;

namespace Coding4Fun.Phone.Controls
{
    public class PhoneFlipMenuAction
    {
        #region NotificationCommand

        private class NotificationCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Action _finish;
            private readonly Func<bool> _canExecute;

            public NotificationCommand(Action execute, Func<bool> canExecute, Action finishCommand)
            {
                _execute = execute;
                _canExecute = canExecute;
                _finish = finishCommand;
            }

            bool ICommand.CanExecute(object parameter)
            {
                return _canExecute();
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            void ICommand.Execute(object parameter)
            {
                _execute();
                _finish();
            }
        }

        #endregion

        public object Content { get; set; }
        internal ICommand Command { get; private set; }

        internal PhoneFlipMenu Parent { get; set; }

        public PhoneFlipMenuAction(object content, Action execute)
            : this(content, execute, () => true)
        {
        }

        public PhoneFlipMenuAction(object content, Action execute, Func<bool> canExecute)
        {
            Content = content;
            Command = new NotificationCommand(execute, canExecute, () => Parent.Hide());
        }
    }
}
