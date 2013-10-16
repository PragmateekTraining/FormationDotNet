using SamplesAPI;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime;

namespace StringInterningSamples
{
    public class MemorySavingSample : ISample
    {
        public void Run()
        {
            // Prevent garbage collection to ensure memory consumption measurement accuracy
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            const int n = 5000000;

            string[] strings = new string[n];

            long initialMemory = GC.GetTotalMemory(false);

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < n; ++i)
            {
                strings[i] = new string('a', 100);
                // strings[i] = string.Intern(new string('a', 100));
            }

            stopwatch.Stop();

            TimeSpan withoutInterningTime = stopwatch.Elapsed;

            long memoryWithoutInterning = GC.GetTotalMemory(false);

            strings = new string[n];

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long initialMemoryBeforeInterning = GC.GetTotalMemory(false);

            stopwatch.Restart();

            for (int i = 0; i < n; ++i)
            {
                strings[i] = string.Intern(new string('a', 100));
                // strings[i] = new string('a', 100);
            }

            stopwatch.Stop();

            TimeSpan withInterningTime = stopwatch.Elapsed;

            long memoryWithInterning = GC.GetTotalMemory(false);

            double memoryRatio = 1.0 * (memoryWithoutInterning - initialMemory) / (memoryWithInterning - initialMemoryBeforeInterning);
            double timeRatio = 1.0 * withInterningTime.Ticks / withoutInterningTime.Ticks;

            using (new Culture(CultureInfo.InvariantCulture))
            {
                Console.WriteLine("Without interning: {0} - {1} = {2} (x{4:N1}) (in {3})", memoryWithoutInterning, initialMemory, memoryWithoutInterning - initialMemory, withoutInterningTime, memoryRatio);
                Console.WriteLine("With interning: {0} - {1} = {2} (in {3}) (x{4:N1})", memoryWithInterning, initialMemoryBeforeInterning, memoryWithInterning - initialMemoryBeforeInterning, withInterningTime, timeRatio);
            }
        }
    }
}
