using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinqSamples
{
    class Program
    {
        /*static int[] FibonacciArray(int n)
        {
            int[] F = new int[n+1];
            F[0] = 0;
            F[1] = 1;

            for (int i = 2; i <= n; ++i)
            {
                F[i] = F[i - 1] + F[i - 2];
            }

            return F;
        }

        static IList<int> FibonacciList(int n)
        {
            var terms = new List<int> { 0, 1 };
            int i = 2;

            while (i <= n)
            {
                terms.Add(terms[i - 1] + terms[i - 2]);
                i += 1;
            }

            return terms;
        }*/

        static void Main(string[] args)
        {
            // new BrokenPipelineSample().Run();
            new WebClientSample().Run();

            /*const int n = 100;

            Stopwatch stopwatch = Stopwatch.StartNew();
            IList<int> asList = FibonacciList(n);
            stopwatch.Stop();

            TimeSpan withListTime = stopwatch.Elapsed;

            stopwatch.Restart();
            IList<int> asArray = FibonacciArray(n);
            stopwatch.Stop();

            TimeSpan withArrayTime = stopwatch.Elapsed;

            string withList = asList.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i)).ToString();
            string withArray = asArray.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i)).ToString();

            Console.WriteLine(withList);
            Console.WriteLine(withArray);

            Console.WriteLine("With array: {0}", withArrayTime);
            Console.WriteLine("With list: {0} (x{1:N2})", withListTime, 1.0 * withListTime.Ticks / withArrayTime.Ticks);*/
        }
    }
}
