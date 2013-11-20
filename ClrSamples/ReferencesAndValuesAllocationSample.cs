using SamplesAPI;

namespace ClrSamples
{
    public class ReferencesAndValuesAllocationSample : ISample
    {
        class Reference
        {
            public Value value = new Value();
        }

        struct Value
        {
            public Reference reference;
        }

        public void Run()
        {
            Reference reference = new Reference();
            Value value = new Value { reference = new Reference() };
        }
    }
}
