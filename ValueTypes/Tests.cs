using System.Collections.Generic;
using NUnit.Framework;

namespace ValueTypes
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
        public void CanUseAValueTypeAsADictionaryKey()
        {
            IDictionary<ReportID, string> reports = new Dictionary<ReportID, string>();

            string message = "Sir, you've failed.\nPlease die.";

            reports[new ReportID("John Doe", 42)] = message;

            Assert.AreEqual(message, reports[new ReportID("John Doe", 42)]);
        }

        [Test]
        public void TestArrayCovariance()
        {
            object[] array1 = new string[0]; // OK
            // object[] array2 = new int[0]; // KO
        }
    }
}
