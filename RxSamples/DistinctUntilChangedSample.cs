using SamplesAPI;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace RxSamples
{
    public class DistinctUntilChangedSample : ISample
    {
        class Stock : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            public string Ticker { get; set; }

            private decimal price;
            public decimal Price
            {
                get
                {
                    return price;
                }
                set
                {
                    if (value != price)
                    {
                        price = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("Price"));
                    }
                }
            }
        }

        public void Run()
        {
            Stock Apple = new Stock { Ticker = "AAPL" };

            using (Observable.FromEventPattern<PropertyChangedEventArgs>(Apple, "PropertyChanged")
                                                    .Select(e => (e.Sender as Stock).Price)
                                                    .DistinctUntilChanged()
                                                    .Subscribe(Console.WriteLine))
            {
                Apple.Price = 450.12m;
                Apple.Price = 451.34m;
                Apple.Price = 451.34m;
                Apple.Price = 452.25m;
                Apple.Price = 452.25m;
                Apple.Price = 452.25m;
                Apple.Price = 455.00m;
                Apple.Price = 455.00m;
            }
        }
    }
}
