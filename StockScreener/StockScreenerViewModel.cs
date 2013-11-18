using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockScreener
{
    public class AddCommand : ICommand
    {
        Action<string> callback;

        public AddCommand(Action<string> callback)
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
            callback(parameter as string);
        }
    }

    public class StockScreenerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        Market market;

        string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                if (value != symbol)
                {
                    symbol = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Symbol"));
                }
            }
        }

        public ObservableCollection<StockViewModel> Stocks { get; private set; }

        public ICommand Add { get; private set; }

        StockViewModel selectedStock;
        public StockViewModel SelectedStock
        {
            get { return selectedStock; }
            set
            {
                if (value != selectedStock)
                {
                    selectedStock = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedStock"));
                }
            }
        }

        public StockScreenerViewModel(Market market)
        {
            this.market = market;
            Add = new AddCommand(AddStock);
            Stocks = new ObservableCollection<StockViewModel>();
        }

        void AddStock(string symbol)
        {
            IDictionary<DateTime, double> prices = market.RegisterForPrices(symbol);

            StockViewModel model = new StockViewModel
            {
                Symbol = symbol,
                Prices = prices,
                Delete = new DeleteCommand(DeleteStock)
            };

            Stocks.Add(model);
        }

        void DeleteStock(StockViewModel model)
        {
            Stocks.Remove(model);
        }
    }
}
