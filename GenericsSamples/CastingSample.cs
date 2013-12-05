using System;
using System.Collections.Generic;
using SamplesAPI;
using System.Collections;
using System.Diagnostics;

namespace GenericsSamples
{
    /// <summary>
    /// Demonstrate the cost of implicit casts in foreach loops.
    /// </summary>
    public class CastingSample : ISample
    {
        public void Run()
        {
            const int N = 1000000;

            // ArrayList is not type-safe, it stores object references.
            ArrayList arrayList = new ArrayList();
            // List<> is type-safe: here it only stores strings.
            List<string> list = new List<string>();

            // Generate a bunch of random strings.
            for (int i = 0; i < N; ++i)
            {
                string guid = Guid.NewGuid().ToString();

                arrayList.Add(guid);
                list.Add(guid);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // For each element there is an implicit cast ("castclass" instruction in CIL) from object to string.
            foreach (string guid in arrayList)
            {
            }

            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();

            // No need to cast because elements type is known.
            foreach (string guid in list)
            {
            }

            stopwatch.Stop();
            TimeSpan t2 = stopwatch.Elapsed;

            // Compute the overhead of casting.
            double ratio = 1.0 * t1.Ticks / t2.Ticks;

            Console.WriteLine("With generics: {0}", t2);
            Console.WriteLine("Without generics: {0} (x{1:N1})", t1, ratio);
        }
    }
}
