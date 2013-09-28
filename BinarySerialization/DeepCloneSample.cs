using System;
using System.Collections.Generic;
using SamplesAPI;
using NUnit.Framework;

namespace SerializationSamples
{
    [TestFixture]
    public class DeepCloneSample : ISample
    {
        [Test]
        public void Run()
        {
            IEnumerable<OHLC> data = Common.GetData();
            IEnumerable<OHLC> copy = data.DeepClone();

            Common.AssertEquals(data, copy);
        }
    }
}
