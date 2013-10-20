using SamplesAPI;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CERSamples
{
    public class NativeAccessViolationSample : ISample
    {
        [DllImport("native.dll")]
        extern static void segfault();

        bool withoutCERHasRunFinally;
        bool withCERHasRunFinally;

        void WithoutCER()
        {
            try
            {
                segfault();
            }
            finally
            {
                withoutCERHasRunFinally = true;
                Console.WriteLine("In");
            }
        }

        void WithCER()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                segfault();
            }
            finally
            {
                withCERHasRunFinally = true;
                Console.WriteLine("In");
            }
        }

        bool withCER;

        public NativeAccessViolationSample(bool withCER)
        {
            this.withCER = withCER;
        }

        public void Run()
        {
            if (!withCER)
            {
                try
                {
                    WithoutCER();
                }
                catch (AccessViolationException e)
                {
                    Console.WriteLine("Got it");
                    Console.WriteLine(e);
                }
            }
            else
            {
                try
                {
                    WithCER();
                }
                catch (AccessViolationException e)
                {
                    Console.WriteLine("Got it");
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine(withoutCERHasRunFinally);
            Console.WriteLine(withCERHasRunFinally);
        }
    }
}
