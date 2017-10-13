using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarService1.ViewModel
{
    class Command : ICommand
    {
        public Command(Action action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public Command(Action<object> parameterizedAction, bool canExecute = true)
        {
            this.parameterizedAction = parameterizedAction;
            this.canExecute = canExecute;
        }

        protected Action action = null;
        protected Action<object> parameterizedAction = null;
        private bool canExecute = false;

        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    EventHandler canExecuteChanged = CanExecuteChanged;
                    if (canExecuteChanged != null)
                        canExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            this.DoExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;
        public event CancelCommandEventHandler Executing;
        public event CommandEventHandler Executed;

        protected void InvokeAction(object param)
        {
            Action theAction = action;
            Action<object> theParameterizedAction = parameterizedAction;
            if (theAction != null)
                theAction();
            else if (theParameterizedAction != null)
                theParameterizedAction(param);
        }

        protected void InvokeExecuted(CommandEventArgs args)
        {
            CommandEventHandler executed = Executed;

            if (executed != null)
                executed(this, args);
        }

        protected void InvokeExecuting(CancelCommandEventArgs args)
        {
            CancelCommandEventHandler executing = Executing;

            if (executing != null)
                executing(this, args);
        }

        public virtual void DoExecute(object param)
        {
            CancelCommandEventArgs args =
               new CancelCommandEventArgs() { Parameter = param, Cancel = false };
            InvokeExecuting(args);

            if (args.Cancel)
                return;

            InvokeAction(param);
            InvokeExecuted(new CommandEventArgs() { Parameter = param });
        }
    }

   
    public delegate void CommandEventHandler(object sender, CommandEventArgs args);
    public delegate void CancelCommandEventHandler(object sender, CancelCommandEventArgs args);

    public class CommandEventArgs : EventArgs
    {
        public object Parameter { get; set; }
    }

    public class CancelCommandEventArgs : CommandEventArgs
    {
        public bool Cancel { get; set; }
    }
}
