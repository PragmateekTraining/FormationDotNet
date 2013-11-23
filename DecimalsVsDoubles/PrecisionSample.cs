using System;
using SamplesAPI;
using System.Diagnostics;

namespace DecimalsSamples
{
    public class PrecisionSample : ISample
    {
        int N = 0;

        public PrecisionSample(int N)
        {
            this.N = N;
        }

        public void Run()
        {
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
    }
}
