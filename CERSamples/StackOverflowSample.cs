using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                Console.WriteLine("IN!");
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
                Console.WriteLine("IN!");
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
