using System;
using System.Diagnostics;
using SamplesAPI;

namespace ValueTypesSamples
{
    public class EqualsOverloadSample : ISample
    {
        struct NotOverloaded
        {
            public int N { get; set; }
            public double D { get; set; }
        }

        struct Overloaded
        {
            public int N { get; set; }
            public double D { get; set; }

            public override bool Equals(object obj)
            {
                Overloaded other = (Overloaded)obj;
                return other.N == this.N && other.D == this.D;
            }

            public override int GetHashCode()
            {
                return N ^ D.GetHashCode();
            }
        }

        struct OverloadedNoBoxing
        {
            public int N { get; set; }
            public double D { get; set; }

            public override bool Equals(object obj)
            {
                if (!(obj is OverloadedNoBoxing))
                {
                    return false;
                }
                    
                return Equals((OverloadedNoBoxing)obj);
            }

            public bool Equals(OverloadedNoBoxing other)
            {
                return other.N == this.N && other.D == this.D;
            }

            public override int GetHashCode()
            {
                return N ^ D.GetHashCode();
            }
        }

        private readonly int N = 0;

        public EqualsOverloadSample(int n)
        {
            N = n;
        }

        public void Run()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            NotOverloaded notOverloaded = new NotOverloaded { N = 123, D = 456.789 };
            for (int i = 0; i < N; ++i)
            {
                notOverloaded.Equals(notOverloaded);
            }
            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();
            Overloaded overloaded = new Overloaded { N = 123, D = 456.789 };
            for (int i = 0; i < N; ++i)
            {
                overloaded.Equals(overloaded);
            }
            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            stopwatch.Restart();
            OverloadedNoBoxing overloadedNoBoxing = new OverloadedNoBoxing { N = 123, D = 456.789 };
            for (int i = 0; i < N; ++i)
            {
                overloadedNoBoxing.Equals(overloadedNoBoxing);
            }
            stopwatch.Stop();

            TimeSpan t3 = stopwatch.Elapsed;

            double ratioT1T3 = 1.0 * t1.Ticks / t3.Ticks;
            double ratioT2T3 = 1.0 * t2.Ticks / t3.Ticks;

            Console.WriteLine("Overloaded without boxing: {0}", t3);
            Console.WriteLine("Overloaded with boxing: {0} (x{1:N1})", t2, ratioT2T3);
            Console.WriteLine("Not overloaded: {0} (x{1:N1})", t1, ratioT1T3);
        }
    }
}
