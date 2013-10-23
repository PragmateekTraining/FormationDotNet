using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace GCSamples
{
    public class MemoryFailPointSample : ISample
    {
        string GetRandomString(int size)
        {
            StringBuilder builder = new StringBuilder(size);

            Random rand = new Random();

            for (int i = 0; i < size; ++i)
            {
                builder.Append((char)('a' + rand.Next(26)));
            }

            return builder.ToString();
        }

        IList<int> referenceSizes = new List<int>();
        IList<long> referenceMemoryConsumptions = new List<long>();

        long EstimateMemory(int size)
        {
            int index = referenceSizes.IndexOf(size);

            if (index != -1) return referenceMemoryConsumptions[index];

            for (int i = 1; i < referenceSizes.Count; ++i)
            {
                if (referenceSizes[i] > size)
                {
                    return referenceMemoryConsumptions[i - 1] + (size - referenceSizes[i - 1]) * (referenceMemoryConsumptions[i] - referenceMemoryConsumptions[i - 1]) / (referenceSizes[i] - referenceSizes[i - 1]);
                }
            }

            int last = referenceMemoryConsumptions.Count - 1;

            return referenceMemoryConsumptions[last] + (size - referenceSizes[last]) * (referenceMemoryConsumptions[last] - referenceMemoryConsumptions[last - 1]) / (referenceSizes[last] - referenceSizes[last - 1]);
        }

        void ComputeReferencePoints()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            for (int size = 1; size < 1 << 27; size <<= 1)
            {
                long memoryBefore = GC.GetTotalMemory(false);

                GetRandomString(size);

                long memoryAfter = GC.GetTotalMemory(false);

                Console.WriteLine("{0}\t{1}", size, memoryAfter - memoryBefore);

                referenceSizes.Add(size);
                referenceMemoryConsumptions.Add(memoryAfter - memoryBefore);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        public void Run()
        {
            ComputeReferencePoints();

            IList<string> strings = new List<string>();

            for (int size = 1; size < 500000000; size *= 3)
            {
                long estimatedMemory = EstimateMemory(size);

                Console.WriteLine("{0} ~ {1}", size, estimatedMemory);

                try
                {
                    using (new MemoryFailPoint((int)Math.Max(estimatedMemory / 1000000, 1)))
                    {
                        strings.Add(GetRandomString(size));
                    }
                }
                catch (InsufficientMemoryException)
                {
                    Console.WriteLine("Not enough memory for {0}!", size);
                    Console.Write("Try? ");
                    bool giveItATry = Convert.ToBoolean(Console.ReadLine());

                    if (giveItATry)
                    {
                        Console.WriteLine("Trying to generate string...");
                        strings.Add(GetRandomString(size));
                    }

                    break;
                }
            }
        }
    }
}
