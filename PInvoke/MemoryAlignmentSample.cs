using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PInvokeSamples
{
    public class MemoryAlignmentSample : ISample
    {
        struct NoPack
        {
            public byte B;
            public int N;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
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

            /*const int N = 60000000;

            NoPack[] noPacks = new NoPack[N];
            Pack[] packs = new Pack[N];

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            int sum = 0;
            for (int i = 0; i < N; ++i)
            {
                sum += noPacks[i].B;
            }
            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();
            sum = 0;
            for (int i = 0; i < N; ++i)
            {
                sum += packs[i].B;
            }
            stopwatch.Stop();
            TimeSpan t2 = stopwatch.Elapsed;

            double ratio = 1.0 * t2.Ticks / t1.Ticks;

            Console.WriteLine("Without packing: {0}", t1);
            Console.WriteLine("With packing: {0} (x{1:N2})", t2, ratio);*/
        }
    }
}
