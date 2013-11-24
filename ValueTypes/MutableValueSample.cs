using SamplesAPI;
using System;
using System.Collections.Generic;

namespace ValueTypesSamples
{
    public class MutableValueSample : ISample
    {
        struct ValueInteger
        {
            public int Value { get; set; }
        }

        class ReferenceInteger
        {
            public int Value { get; set; }
        }

        void One(ValueInteger integer)
        {
            integer.Value = 1;
        }

        void One(ReferenceInteger integer)
        {
            integer.Value = 1;
        }

        public void Run()
        {
            ValueInteger valueInteger = new ValueInteger();
            ReferenceInteger referenceInteger = new ReferenceInteger();

            One(valueInteger);
            One(referenceInteger);

            Console.WriteLine("Value integer: {0}", valueInteger.Value);
            Console.WriteLine("Reference integer: {0}", referenceInteger.Value);

            ValueInteger[] valueIntegers = new ValueInteger[1];
            ReferenceInteger[] referenceIntegers = { new ReferenceInteger() };

            valueIntegers[0].Value = 1;
            referenceIntegers[0].Value = 1;

            Console.WriteLine("valueIntegers[0]: {0}", valueIntegers[0].Value);
            Console.WriteLine("referenceIntegers[0]: {0}", referenceIntegers[0].Value);

            IList<ValueInteger> valueIntegersList = new List<ValueInteger> { new ValueInteger() };
            IList<ReferenceInteger> referenceIntegersList = new List<ReferenceInteger> { new ReferenceInteger() };

            // valueIntegersList[0].Value = 1;
            referenceIntegersList[0].Value = 1;

            Console.WriteLine("referenceIntegersList[0]: {0}", referenceIntegers[0].Value);
        }
    }
}
