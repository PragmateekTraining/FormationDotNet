using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace GCSamples
{
    public class GCGenerationsSample : ISample
    {
        public class A
        {
            public int n;
        }


        /*List<A> l = new List<A>();

        void CreateMemoryPressure()
        {
            const int n = 10000000;

            IList<A> list = new List<A>();

            for (int i = 0; i < n; ++i)
            {
                list.Add(new A());
            }
        }*/

        public IList<A> list = new List<A>();

        public unsafe void Run()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            for (int i = 0; i < 10000; ++i)
            {
                list.Add(new A());
            }
            A a = new A();

            Console.WriteLine("Initial generation: {0}", GC.GetGeneration(a));
            fixed (int* p = &a.n) Console.WriteLine("Initial address: {0:X8}", (long)p);

            GC.Collect();

            Console.WriteLine("Next generation: {0}", GC.GetGeneration(a));
            fixed (int* p = &a.n) Console.WriteLine("Next address: {0:X8}", (long)p);

            GC.Collect();

            Console.WriteLine("Next generation: {0}", GC.GetGeneration(a));
            fixed (int* p = &a.n) Console.WriteLine("Next address: {0:X8}", (long)p);

            list = null;

            GC.Collect();

            Console.WriteLine("Final generation: {0}", GC.GetGeneration(a));
            fixed (int* p = &a.n) Console.WriteLine("Final address: {0:X8}", (long)p);
        }
    }
}
