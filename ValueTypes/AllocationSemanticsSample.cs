using SamplesAPI;
using System;

namespace ValueTypesSamples
{
    class AllocationSemanticsSample : ISample
    {
        struct Value
        {
            public int N { get; set; }
            public int NPlusOne { get; set; }

            public Value(int n, Action callback = null)
                : this()
            {
                callback = callback ?? delegate { };

                N = n;
                callback();
                NPlusOne = N + 1;
            }
        }

        Value instanceValue;
        Value instanceValue2;

        public void Run()
        {
            instanceValue = new Value();
            instanceValue2 = new Value(1);

            // Action a = () => Console.Write("", instanceValue, closureValue2);

            Value localValue = new Value();
            Value localValue2 = new Value(1);

            Value v = default(Value);
            v = new Value(1, () => Console.WriteLine("{0}/{1}", v.N, v.NPlusOne));

            Console.ReadLine();

            Console.Write("", localValue, localValue2);

            //  a();
        }
    }
}
