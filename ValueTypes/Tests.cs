using System.Collections.Generic;
using NUnit.Framework;

namespace ValueTypesSamples
{
    [TestFixture]
    class Tests
    {
        [Test]
        public void CanCompareValueTypes()
        {
            Assert.AreEqual(new ReportID("John Doe", 42), new ReportID("John Doe", 42));
            Assert.AreNotEqual(new ReportID("John Doe", 42), new ReportID("John doe", 42));
        }

        [Test]
        public void TestArrayCovariance()
        {
            object[] array1 = new string[0]; // OK
            // object[] array2 = new int[0]; // KO
        }
    }
}
