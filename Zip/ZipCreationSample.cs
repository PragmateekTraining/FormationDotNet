using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SamplesAPI;

namespace Zip
{
    [TestFixture]
    public class ZipCreationSample : ISample
    {
        [Test]
        public void CanCreateAZipArchive()
        {
            Encoding encoding = Encoding.UTF8;

            string text = "Some text...";

            encoding.GetBytes(text).SaveAsZipArchive();

            IDictionary<string, byte[]> content = "data.zip".GetContentOfZipArchive();

            Assert.AreEqual(1, content.Count);
            Assert.That(content.ContainsKey("data.bin"));

            byte[] outputData = content["data.bin"];

            Assert.IsNotNull(outputData);

            string outputText = encoding.GetString(outputData);

            Assert.AreEqual(text, outputText);
        }

        public void Run()
        {
            CanCreateAZipArchive();
        }
    }
}
