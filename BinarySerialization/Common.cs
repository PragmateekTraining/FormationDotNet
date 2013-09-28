using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using NUnit.Framework;

namespace SerializationSamples
{
    public class Common
    {
        public static IEnumerable<OHLC> GetData()
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

        public static void AssertEquals(IEnumerable<OHLC> inData, IEnumerable<OHLC> outData)
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
    }
}
