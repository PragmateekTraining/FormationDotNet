using System.Collections.Generic;
using SamplesAPI;
using NUnit.Framework;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SerializationSamples
{
    [TestFixture]
    public class BinarySerializationSample : ISample
    {
        [Test]
        public void Run()
        {
            IFormatter formatter = new BinaryFormatter();

            IEnumerable<OHLC> inData = Common.GetData();

            using (Stream stream = File.OpenWrite("GOOG_prices.dat"))
            {
                formatter.Serialize(stream, inData);
            }

            IEnumerable<OHLC> outData = null;
            using (Stream stream = File.OpenRead("GOOG_prices.dat"))
            {
                outData = formatter.Deserialize(stream) as IEnumerable<OHLC>;
            }

            Common.AssertEquals(inData, outData);
        }
    }
}
