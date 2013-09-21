using NUnit.Framework;

namespace Networking
{
    [TestFixture]
    class Tests
    {
        [Test]
        public void CanGetGoogleHomePage()
        {
            HttpClient webClient = new HttpClient();
            string page = webClient.Get("http://www.google.com");
            page = webClient.Get("http://www.yahoo.com");
        }
    }
}
