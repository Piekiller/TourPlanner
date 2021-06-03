using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TourPlanner.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            _execute = methodToExecute;
            _canExecute = canExecuteEvaluator;
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
            => _canExecute == null || _canExecute.Invoke();

        public void Execute(object parameter) => _execute.Invoke();
    }
}
