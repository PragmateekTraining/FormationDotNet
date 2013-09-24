using System;
using NUnit.Framework;
using System.Diagnostics;
using SamplesAPI;

namespace Boxing
{
    [TestFixture]
    public class PerformanceSample : ISample
    {
        [Test]
        public void CanEvaluateBoxingPerformance()
        {
            int[] unboxedArray = new int[10000000];
            object[] boxedArray = new object[10000000];

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < unboxedArray.Length; ++i)
            {
                unboxedArray[i] = i;
            }

            stopwatch.Stop();

            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();

            for (int i = 0; i < boxedArray.Length; ++i)
            {
                boxedArray[i] = i;
            }

            stopwatch.Stop();

            TimeSpan t2 = stopwatch.Elapsed;

            long ratio = t2.Ticks / t1.Ticks;

            Console.WriteLine("Without boxing: {0}", t1);
            Console.WriteLine("With boxing: {0} (x{1})", t2, ratio);

            Assert.GreaterOrEqual(ratio, 10);
        }

        public void Run()
        {
            CanEvaluateBoxingPerformance();
        }

    }
}
