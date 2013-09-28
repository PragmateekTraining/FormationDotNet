using System.Collections.Generic;
using SamplesAPI;
using NUnit.Framework;
using System;

namespace SerializationSamples
{
    [TestFixture]
    public class Base64EncodingSample : ISample
    {
        [Test]
        public void Run()
        {
            IEnumerable<OHLC> inData = Common.GetData();

            string base64 = Convert.ToBase64String(inData.ToNetBinary());

            Console.WriteLine("Quotes encoded in base 64:\n{0}", base64);

            IEnumerable<OHLC> outData = Convert.FromBase64String(base64).FromNetBinary<IEnumerable<OHLC>>();

            Common.AssertEquals(inData, outData);
        }
    }
}
