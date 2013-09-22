using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            const int N = 100000;

            string s = "";
            stopwatch.Start();
            for (int i = 1; i <= N; ++i)
            {
                s += "1";
            }
            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            StringBuilder builder = new StringBuilder();
            stopwatch.Restart();
            for (int i = 1; i <= N; ++i)
            {
                builder.Append("1");
            }
            s = builder.ToString();
            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            Console.WriteLine("With StringBuilder: {0:N2}ms", t2.TotalMilliseconds);
            Console.WriteLine("With String: {0:N2}ms (x{1})", t1.TotalMilliseconds, t1.Ticks / t2.Ticks);
        }
    }
}
