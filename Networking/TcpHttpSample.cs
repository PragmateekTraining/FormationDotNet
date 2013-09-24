using NUnit.Framework;
using SamplesAPI;

namespace Networking
{
    [TestFixture]
    class TcpHttpSample : ISample
    {
        [Test]
        public void CanGetGoogleAndYahooHomePages()
        {
            HttpClient webClient = new HttpClient();
            string page = webClient.Get("http://www.google.com");
            Assert.That(!string.IsNullOrWhiteSpace(page));
            page = webClient.Get("http://www.yahoo.com");
            Assert.That(!string.IsNullOrWhiteSpace(page));
        }

        public void Run()
        {
            CanGetGoogleAndYahooHomePages();
        }
    }
}
