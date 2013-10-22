using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;

namespace PInvokeSamples
{
    public class MemoryPinningSample : ISample
    {
        [DllImport("NativeLibrary.dll", EntryPoint = "set_data", CallingConvention = CallingConvention.Cdecl)]
        extern unsafe static void SetData(int* p);

        [DllImport("NativeLibrary.dll", EntryPoint = "get_data", CallingConvention = CallingConvention.Cdecl)]
        extern static int GetData();

        class A
        {
            public int n;
        }

        IList<A> list = new List<A>();

        public unsafe void Run()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            for (int i = 0; i < 10000; ++i)
            {
                list.Add(new A());
            }

            A a = new A { n = 123 };

            // "Push" the objects to gen#2
            GC.Collect();
            GC.Collect();

            // Make the list ready for collection
            list = null;

            Console.WriteLine("===Initial situation===");

            fixed (int* p = &a.n)
            {
                SetData(p);

                Console.WriteLine("Address: {0}", (long)p);
                Console.WriteLine("GetData: {0}", GetData());

                GC.Collect();
            }

            Console.WriteLine("===After first collection===");

            fixed (int* p = &a.n)
                Console.WriteLine("Address: {0}", (long)p);
            Console.WriteLine("GetData: {0}", GetData());

            GC.Collect();

            Console.WriteLine("===After second collection===");

            fixed (int* p = &a.n)
                Console.WriteLine("Address: {0}", (long)p);
            Console.WriteLine("GetData: {0}", GetData());

            Random rand = new Random();
            list = new List<A>();
            for (int i = 0; i < 10000; ++i)
            {
                list.Add(new A { n = rand.Next() });
            }

            GC.Collect();
            GC.Collect();

            Console.WriteLine("===After memory allocations and 2 more collections===");

            fixed (int* p = &a.n)
                Console.WriteLine("Address: {0}", (long)p);
            Console.WriteLine("GetData: {0}", GetData());
        }
    }
}
