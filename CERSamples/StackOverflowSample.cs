using SamplesAPI;
using System;
using System.Runtime.CompilerServices;

namespace CERSamples
{
    public class StackOverflowSample : ISample
    {
        bool withoutCERHasRunFinally;
        bool withCERHasRunFinally;

        // [PrePrepareMethod]
        void BlackHole()
        {
            BlackHole();
        }

        void WithoutCER()
        {
            try
            {
                BlackHole();
            }
            finally
            {
                withoutCERHasRunFinally = true;
            }
        }

        void WithCER()
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                BlackHole();
            }
            finally
            {
                withCERHasRunFinally = true;
            }
        }

        bool useCER;

        public StackOverflowSample(bool useCER)
        {
            this.useCER = useCER;
        }

        public void Run()
        {
            if (!useCER)
            {
                try
                {
                    WithoutCER();
                }
                catch (StackOverflowException)
                {
                }
            }
            else
            {
                Console.WriteLine("Using CER");

                try
                {
                    WithCER();
                }
                catch (StackOverflowException)
                {
                }
            }

            Console.WriteLine(withoutCERHasRunFinally);
            Console.WriteLine(withCERHasRunFinally);
        }
    }
}
