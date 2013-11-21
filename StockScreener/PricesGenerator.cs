using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockScreener
{
    public class PricesGenerator
    {
        const int nPerDay = 1;

        Random rand = new Random();

        public IDictionary<DateTime, double> GetPrices(DateTime from, DateTime to)
        {
            int nDays = (to - from).Days + 1;

            IDictionary<DateTime, double> prices = new Dictionary<DateTime, double>(nDays * nPerDay);

            double v = 0.2 + 0.5 * rand.NextDouble();

            double[] W = BrownianMotion(nDays * nPerDay + 1);

            TimeSpan tick = TimeSpan.FromMilliseconds(TimeSpan.FromDays(1).TotalMilliseconds / nPerDay);

            int i = 0;
            double price = 100 * rand.NextDouble();
            for (DateTime date = from; date <= to; date += tick)
            {
                prices.Add(date, price);
                // prices.Add(date, W[i]);
                price += price * v / (365 * nPerDay) * (W[i + 1] - W[i]);
                ++i;
            }

            // Simulate some hard work
            Thread.Sleep(2000);

            return prices;
        }

        double[] BrownianMotion(int n)
        {
            double[] motion = new double[n];

            motion[0] = 0;
            for (int i = 1; i < n; ++i)
            {
                double r = Math.Sqrt(2 * (-1 * Math.Log(1 - rand.NextDouble())));

                double t = 2 * Math.PI * rand.NextDouble();

                motion[i] = motion[i - 1] + r * Math.Cos(t);

                ++i;

                if (i < n)
                {
                    motion[i] = motion[i - 1] + r * Math.Sin(t);
                }
            }

            return motion;
        }
    }
}
