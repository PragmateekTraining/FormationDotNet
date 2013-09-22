using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Zip
{
    [TestFixture]
    public class ZipCompressedFilesTest
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
    }
}
