using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DecimalsVsDoubles
{
    class Program
    {
        static void Performance()
        {
            const int N = 100000000;

            decimal sumDecimal = 0;
            double sumDouble = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < N; ++i)
            {
                ++sumDecimal;
            }
            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();
            for (int i = 0; i < N; ++i)
            {
                ++sumDouble;
            }
            stopwatch.Stop();
            TimeSpan t2 = stopwatch.Elapsed;

            Console.WriteLine("With double: {0:N2}ms", t2.TotalMilliseconds);
            Console.WriteLine("With decimal: {0:N2}ms (x{1})", t1.TotalMilliseconds, t1.Ticks / t2.Ticks);
        }

        static void Precision()
        {
            const int N = 100000000;

            for (int n = 1; n <= N; n *= 10)
            {
                decimal decimalValue = 1e-10m;
                double doubleValue = 1e-10;

                for (int i = 1; i <= n; ++i)
                {
                    decimalValue += 0.1m;
                }

                for (int i = 1; i <= n; ++i)
                {
                    decimalValue -= 0.1m;
                }

                for (int i = 1; i <= n; ++i)
                {
                    doubleValue += 0.1;
                }

                for (int i = 1; i <= n; ++i)
                {
                    doubleValue -= 0.1;
                }

                double doubleError = Math.Abs((doubleValue - 1e-10) / 1e-10 * 100);
                decimal decimalError = Math.Abs((decimalValue - 1e-10m) / 1e-10m * 100);

                Console.WriteLine("{0}\t{1:N2}%\t{2:N2}%", n, decimalError, doubleError);

                /*Console.WriteLine("With double: {0} ({1:N2}%)", doubleValue, (doubleValue - 1e-10) / 1e-10 * 100);
                Console.WriteLine("With decimal: {0} ({1:N2}%)", decimalValue, (decimalValue - 1e-10m) / 1e-10m * 100);*/
            }
        }

        static void Main(string[] args)
        {
            Precision();
        }
    }
}
