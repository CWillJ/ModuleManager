namespace LoadDLLs.Classes
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// MyICommand implements the ICommand.
    /// </summary>
    public class MyICommand : ICommand
    {
        private Action targetExecuteMethod;
        private Func<bool> targetCanExecuteMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyICommand"/> class.
        /// </summary>
        /// <param name="executeMethod">Method to execute.</param>
        public MyICommand(Action executeMethod)
        {
            this.targetExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyICommand"/> class.
        /// </summary>
        /// <param name="executeMethod">Method to execute.</param>
        /// <param name="canExecuteMethod">Returns boolean value on if a method can be executed.</param>
        public MyICommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            this.targetExecuteMethod = executeMethod;
            this.targetCanExecuteMethod = canExecuteMethod;
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
            this.CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Returns boolean value stating whether the object can be executed on.
        /// </summary>
        /// <param name="parameter">Object.</param>
        /// <returns>Boolean.</returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (this.targetCanExecuteMethod != null)
            {
                return this.targetCanExecuteMethod();
            }

            if (this.targetExecuteMethod != null)
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
            if (this.targetExecuteMethod != null)
            {
                this.targetExecuteMethod();
            }
        }
    }
}