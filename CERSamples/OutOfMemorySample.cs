using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace CERSamples
{
    public class OutOfMemorySample : ISample
    {
        bool withoutCERThreadHasRunFinally;
        bool withCERThreadHasRunFinally;

        void WithoutCER()
        {
            IList<object> list = new List<object>();

            try
            {
                while (true)
                    list.Add(new object());
            }
            finally
            {
                withoutCERThreadHasRunFinally = true;
            }
        }

        void WithCER()
        {
            IList<object> list = new List<object>();

            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                while (true)
                    list.Add(new object());
            }
            finally
            {
                withCERThreadHasRunFinally = true;
            }
        }

        public void Run()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            try
            {
                WithoutCER();
            }
            catch (OutOfMemoryException)
            {
                GC.Collect();
            }

            try
            {
                WithCER();
            }
            catch (OutOfMemoryException)
            {
                GC.Collect();
            }

            Console.WriteLine(withoutCERThreadHasRunFinally);
            Console.WriteLine(withCERThreadHasRunFinally);
        }
    }
}
