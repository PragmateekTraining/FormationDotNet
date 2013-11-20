using System;
using SamplesAPI;

namespace ValueTypesSamples
{
    public class DefaultComparisonSample : ISample
    {
        struct A
        {
            public int N { get; set; }

            public override int GetHashCode()
            {
                Console.WriteLine("A.GetHashCode");
                return 0;
            }

            public override bool Equals(object obj)
            {
                Console.WriteLine("A.Equals");
                return true;
            }
        }

        struct Value
        {
            public A A { get; set; }
        }

        public void Run()
        {
            Value value1 = new Value { A = new A { N = 123 } };
            Value value2 = new Value { A = new A { N = 456 } };

            Console.WriteLine(value1.Equals(value2));
        }
    }
}
