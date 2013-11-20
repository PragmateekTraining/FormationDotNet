using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ValueTypesSamples
{
    class BoxingPerformanceImpactSample : ISample
    {
        public void Run()
        {
            const int n = 1 << 24;

            int[] values = new int[n];
            object[] boxes = new object[n];

            Stopwatch stopwatch = new Stopwatch();

            long initialMemory = GC.GetTotalMemory(true);
            stopwatch.Start();
            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = 1;
            }
            stopwatch.Stop();
            TimeSpan valuesTime = stopwatch.Elapsed;
            long memoryAfterValuesAllocation = GC.GetTotalMemory(true);

            stopwatch.Restart();
            for (int i = 0; i < values.Length; ++i)
            {
                boxes[i] = 1;
            }
            TimeSpan boxesTime = stopwatch.Elapsed;
            long memoryAfterBoxesAllocation = GC.GetTotalMemory(true);

            values[0] = 1;
            boxes[0] = 1;

            double timeRatio = 1.0 * boxesTime.Ticks / valuesTime.Ticks;

            Console.WriteLine("Values: consumed {0} in {1}.", memoryAfterValuesAllocation - initialMemory, valuesTime);
            Console.WriteLine("Boxes: consumed {0} in {1} (x{2:N2}).", memoryAfterBoxesAllocation - initialMemory, boxesTime, timeRatio);
        }
    }
}
