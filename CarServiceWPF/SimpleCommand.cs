using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarServiceWPF
{
    public class SimpleCommand : ICommand
    {
        private Action<object> mExecuteParam = null;
        private Func<object, bool> mCanExecute;
        private Action mExecuteSimple = null;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public SimpleCommand(Action<object> aExecute, Func<object, bool> aCanExecute = null)
        {
            mExecuteParam = aExecute;
            mCanExecute = aCanExecute;
        }

        public SimpleCommand(Action aExecute, Func<object, bool> aCanExecute = null)
        {
            mExecuteSimple = aExecute;
            mCanExecute = aCanExecute;
        }

        public bool CanExecute(object aParam)
        {
            return mCanExecute == null || mCanExecute(aParam);
        }

        public void Execute(object aParam)
        {
            if(mExecuteParam != null)
                mExecuteParam(aParam);

            if (mExecuteSimple != null)
                mExecuteSimple();
        }
    }
}

