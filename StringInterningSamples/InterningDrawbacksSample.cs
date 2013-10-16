using SamplesAPI;
using System;
using System.Globalization;
using System.Runtime;

namespace StringInterningSamples
{
    public class InterningDrawbacksSample : ISample
    {
        public void Run()
        {
            // Prevent garbage collection to ensure memory consumption measurement accuracy
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            const int n = 1000000;

            string[] strings = new string[n];

            long initialMemory = GC.GetTotalMemory(true);

            for (int i = 0; i < n; ++i)
            {
                strings[i] = Guid.NewGuid().ToString();
            }

            long memoryWithoutInterning = GC.GetTotalMemory(false);

            strings = new string[n];

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryAfterCollectionWithoutInterning = GC.GetTotalMemory(false);

            long initialMemoryBeforeInterning = GC.GetTotalMemory(false);

            for (int i = 0; i < n; ++i)
            {
                strings[i] = string.Intern(Guid.NewGuid().ToString());
            }

            long memoryWithInterning = GC.GetTotalMemory(false);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memoryAfterCollectionWithInterning = GC.GetTotalMemory(false);

            using (new Culture(CultureInfo.InvariantCulture))
            {
                string format = "|{0,10}|{1,15}|{2,15}|";
                Console.WriteLine(format, "Interning?", "Before GC", "After GC");
                Console.WriteLine(format, "No", memoryWithoutInterning - initialMemory, memoryAfterCollectionWithoutInterning - initialMemory);
                Console.WriteLine(format, "Yes", memoryWithInterning - initialMemoryBeforeInterning, memoryAfterCollectionWithInterning - initialMemoryBeforeInterning);
            }
        }
    }
}
