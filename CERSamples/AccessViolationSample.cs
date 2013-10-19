using SamplesAPI;
using System;
using System.Runtime.CompilerServices;

namespace CERSamples
{
    public class AccessViolationSample : ISample
    {
        bool withoutCERHasRunFinally;
        bool withCERHasRunFinally;

        void WithoutCER()
        {
            try
            {
                AccessViolation.Crash();
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
                AccessViolation.Crash();
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
                Console.WriteLine(e);
            }

            try
            {
                WithCER();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine(withoutCERHasRunFinally);
            Console.WriteLine(withCERHasRunFinally);
        }
    }
}
