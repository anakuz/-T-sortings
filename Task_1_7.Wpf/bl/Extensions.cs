#region Imports (2)

using System;
using System.Windows.Input;

#endregion Imports (2)

namespace Task_1_7.Wpf
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _action;

        public Command(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
