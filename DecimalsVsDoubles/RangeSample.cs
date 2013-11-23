using SamplesAPI;
using System;

namespace DecimalsSamples
{
    public class RangeSample : ISample
    {
        public void Run()
        {
            Console.WriteLine("Double: [{0}, {1}]", double.MinValue, double.MaxValue);
            Console.WriteLine("Decimal: [{0}, {1}]\n         [{0:E}, {1:E}]", decimal.MinValue, decimal.MaxValue);
        }
    }
}
