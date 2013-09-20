using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace BinarySerialization
{
    [TestFixture]
    class Tests
    {
        static IEnumerable<OHLC> GetData()
        {
            return File.ReadAllLines("GOOG_prices.csv")
                       .Skip(1)
                       .Select(line => line.Split(','))
                       .Select(tokens => new OHLC
                       {
                           Date = DateTime.ParseExact(tokens[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                           Open = decimal.Parse(tokens[1], CultureInfo.InvariantCulture),
                           High = decimal.Parse(tokens[2], CultureInfo.InvariantCulture),
                           Low = decimal.Parse(tokens[3], CultureInfo.InvariantCulture),
                           Close = decimal.Parse(tokens[4], CultureInfo.InvariantCulture),
                           Volume = decimal.Parse(tokens[5], CultureInfo.InvariantCulture)
                       })
                       .ToList();
        }

        static void AssertEquals(IEnumerable<OHLC> inData, IEnumerable<OHLC> outData)
        {
            foreach (OHLC inOHLC in inData)
            {
                OHLC outOHLC = outData.SingleOrDefault(ohlc => ohlc.Date == inOHLC.Date);

                Assert.IsNotNull(outOHLC);
                Assert.AreEqual(inOHLC.Open, outOHLC.Open);
                Assert.AreEqual(inOHLC.High, outOHLC.High);
                Assert.AreEqual(inOHLC.Low, outOHLC.Low);
                Assert.AreEqual(inOHLC.Close, outOHLC.Close);
                Assert.AreEqual(inOHLC.Volume, outOHLC.Volume);
            }
        }

        [Test]
        public void CanBinarySerializeQuotes()
        {
            IFormatter formatter = new BinaryFormatter();

            IEnumerable<OHLC> inData = GetData();

            using (Stream stream = File.OpenWrite("GOOG_prices.dat"))
            {
                formatter.Serialize(stream, inData);
            }

            IEnumerable<OHLC> outData = null;
            using (Stream stream = File.OpenRead("GOOG_prices.dat"))
            {
                outData = formatter.Deserialize(stream) as IEnumerable<OHLC>;
            }

            AssertEquals(inData, outData);
        }

        [Test]
        public void CanDeepClone()
        {
            IEnumerable<OHLC> data = GetData();
            IEnumerable<OHLC> copy = data.DeepClone();

            AssertEquals(data, copy);
        }
    }
}
