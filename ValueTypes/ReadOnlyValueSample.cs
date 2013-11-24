using SamplesAPI;
using System;

namespace ValueTypesSamples
{
    public class ReadOnlyValueSample : ISample
    {
        struct ValueInteger
        {
            public int Value { get; private set; }

            public ValueInteger(int value)
                : this()
            {
                this.Value = value;
            }

            public void Increment()
            {
                ++this.Value;
            }
        }

        class ReferenceInteger
        {
            public int Value { get; private set; }

            public ReferenceInteger(int value)
            {
                this.Value = value;
            }

            public void Increment()
            {
                ++this.Value;
            }
        }

        ValueInteger valueInteger = new ValueInteger(0);
        readonly ValueInteger roValueInteger = new ValueInteger(0);
        ReferenceInteger referenceInteger = new ReferenceInteger(0);
        readonly ReferenceInteger roReferenceInteger = new ReferenceInteger(0);

        public void Run()
        {
            valueInteger.Increment();
            roValueInteger.Increment();
            referenceInteger.Increment();
            roReferenceInteger.Increment();

            Console.WriteLine("Value integer: {0}", valueInteger.Value);
            Console.WriteLine("Readonly value integer: {0}", roValueInteger.Value);
            Console.WriteLine("Reference integer: {0}", referenceInteger.Value);
            Console.WriteLine("Readonly reference integer: {0}", roReferenceInteger.Value);
        }
    }
}
