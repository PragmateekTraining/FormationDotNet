using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SamplesAPI;

namespace Zip
{
    [TestFixture]
    public class ZipUncompressionSample : ISample
    {
        [Test]
        public void CanUncompressAZipArchive()
        {
            string zipArchivePath = "TestArchiveForSimpleZipUncompression.zip";
            string aFilePAth = "test/a/a.txt";
            string bFilePAth = "test/b/b/b.txt";

            IDictionary<string, byte[]> zipArchiveContent = zipArchivePath.GetContentOfZipArchive();

            string[] expectedPaths = "test/,test/a/,test/a/a.txt,test/b/,test/b/b/,test/b/b/b.txt,test/c/".Split(',');

            Assert.AreEqual(expectedPaths.Length, zipArchiveContent.Count);

            foreach (string path in expectedPaths)
            {
                Assert.That(zipArchiveContent.ContainsKey(path));
            }

            Encoding asciiEncoding = new ASCIIEncoding();

            Assert.AreEqual("a", asciiEncoding.GetString(zipArchiveContent[aFilePAth]));
            Assert.AreEqual("b", asciiEncoding.GetString(zipArchiveContent[bFilePAth]));

            foreach (string path in expectedPaths.Except(new[] { aFilePAth, bFilePAth }))
            {
                Assert.AreEqual(null, zipArchiveContent[path]);
            }
        }

        public void Run()
        {
            CanUncompressAZipArchive();
        }
    }
}
