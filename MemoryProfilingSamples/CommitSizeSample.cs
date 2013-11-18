using SamplesAPI;
using System;

namespace MemoryProfilingSamples
{
    public class CommitSizeSample : ISample
    {
        public void Run()
        {
            const int size = 1 << 26;
            const int n = 4;
            const int chunkSize = size / n;

            byte[] buffer = new byte[size];

            for (int i = 1; i <= n; ++i)
            {
                Console.WriteLine("Press enter to access buffer...");
                Console.ReadLine();

                Array.Clear(buffer, (i - 1) * chunkSize, chunkSize);
            }

            Console.WriteLine("Press enter to quit...");
        }
    }
}
