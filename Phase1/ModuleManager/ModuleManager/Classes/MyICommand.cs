namespace ModuleManager.Classes
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// MyICommand implements the ICommand.
    /// </summary>
    public class MyICommand : ICommand
    {
        private Action _targetExecuteMethod;
        private Func<bool> _targetCanExecuteMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyICommand"/> class.
        /// </summary>
        /// <param name="executeMethod">Method to execute.</param>
        public MyICommand(Action executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyICommand"/> class.
        /// </summary>
        /// <param name="executeMethod">Method to execute.</param>
        /// <param name="canExecuteMethod">Returns boolean value on if a method can be executed.</param>
        public MyICommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Delegate event handler.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Raise an event where the can execute a method has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Returns boolean value stating whether the object can be executed on.
        /// </summary>
        /// <param name="parameter">Object.</param>
        /// <returns>Boolean.</returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
            {
                return _targetCanExecuteMethod();
            }

            if (_targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes on the passed in object.
        /// </summary>
        /// <param name="parameter">Object.</param>
        void ICommand.Execute(object parameter)
        {
            if (_targetExecuteMethod != null)
            {
                _targetExecuteMethod();
            }
        }
    }
}