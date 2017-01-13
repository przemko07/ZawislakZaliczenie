using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TSP.Extensions
{
    public class ActionCommand
    : ICommand
    {
        #region private readonly members

        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        #endregion


        #region public ctors

        public ActionCommand(Action execute)
        {
            this.execute = (o) => execute();
        }

        public ActionCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public ActionCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = (o) => execute();
            this.canExecute = (o) => canExecute();
        }

        public ActionCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion


        #region implement ICommand

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }

        #endregion
    }
}
