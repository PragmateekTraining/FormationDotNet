using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessSamples
{
    public class FailFastSample : ISample
    {
        class A
        {
            ~A()
            {
                Console.WriteLine("~A");
            }
        }

        private bool failFast = false;

        public FailFastSample(bool failFast)
        {
            this.failFast = failFast;
        }

        public void Run()
        {
            new A();

            try
            {
                if (failFast)
                {
                    Environment.FailFast("Bye!");
                }
                else
                {
                    Environment.Exit(1);
                }
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }
    }
}
