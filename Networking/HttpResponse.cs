using System.Collections.Generic;

namespace Networking
{
    class HttpResponse
    {
        public string HttpVersion { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Entity { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public HttpResponse()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}
