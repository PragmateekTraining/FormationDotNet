using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClrSamples
{
    public class ComparisonSample : ISample
    {
        class A
        {
            public override int GetHashCode()
            {
                Console.WriteLine("GetHashCode");
                return 0;
            }

            public override bool Equals(object obj)
            {
                return obj is A;
            }
        }

        public void Run()
        {
            A a1 = new A();
            A a2 = new A();

            Console.WriteLine(a1 == a2);
        }
    }
}
