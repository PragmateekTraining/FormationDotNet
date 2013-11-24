using SamplesAPI;
using System;

namespace ValueTypesSamples
{
    public class InterfaceImplementationSample : ISample
    {
        interface IInteger
        {
            int Value { get; set; }
        }

        struct Integer : IInteger
        {
            public int Value { get; set; }
        }

        public void Run()
        {
            Integer n1 = new Integer();
            Integer n2 = n1;

            IInteger i1 = new Integer();
            IInteger i2 = i1;

            n1.Value = 1;
            i1.Value = 1;

            Console.WriteLine("n1: {0}", n1.Value);
            Console.WriteLine("n2: {0}", n2.Value);
            Console.WriteLine("i1: {0}", i1.Value);
            Console.WriteLine("i2: {0}", i2.Value);
        }
    }
}
