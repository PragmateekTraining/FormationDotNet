using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScreener
{
    public class NewPriceEventArgs : EventArgs
    {
        public string Symbol { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
    }

    public class Market
    {
        readonly DateTime from = new DateTime(2000, 01, 01);

        IList<string> symbols = new List<string>();

        PricesGenerator priceGenerator = new PricesGenerator();

        public event EventHandler<NewPriceEventArgs> NewPrice = delegate { };


        public IDictionary<DateTime, double> RegisterForPrices(string symbol)
        {
            symbols.Add(symbol);

            return priceGenerator.GetPrices(from, DateTime.Now);
        }
    }
}
