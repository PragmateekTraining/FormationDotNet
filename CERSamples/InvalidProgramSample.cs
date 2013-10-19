using SamplesAPI;
using System;
using System.Runtime.CompilerServices;

namespace CERSamples
{
    public class InvalidProgramSample : ISample
    {
        bool withoutCERHasRunFinally;
        bool withCERHasRunFinally;

        void WithoutCER()
        {
            try
            {
                A.Crash();
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
                A.Crash();
            }
            finally
            {
                withCERHasRunFinally = true;
            }
        }

        public void Run()
        {
            try
            {
                WithoutCER();
            }
            catch (Exception e)
            {
                Console.WriteLine("aaaaaaa");
            }

            try
            {
                WithCER();
            }
            catch (Exception e)
            {
                Console.WriteLine("aaaaaaa");
            }

            Console.WriteLine(withoutCERHasRunFinally);
            Console.WriteLine(withCERHasRunFinally);
        }
    }
}
