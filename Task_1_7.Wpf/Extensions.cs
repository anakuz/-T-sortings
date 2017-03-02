using System;
using System.Windows.Input;

namespace Task_1_7.Wpf
{
    public class Command : ICommand
    {
        private readonly Action _actgion;

        public Command(Action actgion)
        {
            _actgion = actgion;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _actgion();
        }

        public event EventHandler CanExecuteChanged;
    }
}