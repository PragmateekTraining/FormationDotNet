using SamplesAPI;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MemoryProfilingSamples
{
    public class MemoryAlignmentSample : ISample
    {
        struct NoPack
        {
            public byte B;
            public int N;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Pack
        {
            public byte B;
            public int N;
        }

        public void Run()
        {
            NoPack noPack = new NoPack();
            Pack pack = new Pack();

            Console.WriteLine(Marshal.SizeOf(noPack));
            Console.WriteLine(Marshal.SizeOf(pack));

            const int N = 10000000;

            NoPack[] noPacks = new NoPack[N];
            Pack[] packs = new Pack[N];

            Console.Write("Press enter to continue...");
            Console.ReadLine();

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            int sum = 0;
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < 100; ++j)
                {
                    noPacks[i].B = 1;
                    sum += noPacks[i].B;
                }
            }
            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;

            Console.WriteLine(sum);

            stopwatch.Restart();
            sum = 0;
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < 100; ++j)
                {
                    packs[i].B = 1;
                    sum += packs[i].B;
                }
            }
            stopwatch.Stop();
            TimeSpan t2 = stopwatch.Elapsed;

            Console.WriteLine(sum);

            double ratio = 1.0 * t2.Ticks / t1.Ticks;

            Console.WriteLine("Without packing: {0}", t1);
            Console.WriteLine("With packing: {0} (x{1:N2})", t2, ratio);
        }
    }
}
