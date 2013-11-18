using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockScreener
{
    class DeleteCommand : ICommand
    {
        Action<StockViewModel> callback;

        public DeleteCommand(Action<StockViewModel> callback)
        {
            this.callback = callback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            callback(parameter as StockViewModel);
        }
    }

    public class StockViewModel
    {
        public string Symbol { get; internal set; }

        public IDictionary<DateTime, double> Prices { get; set; }

        public ICommand Delete { get; internal set; }

        //public 

        public StockViewModel()
        {
        }
    }
}
