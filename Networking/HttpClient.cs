using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Networking
{
    class HttpClient
    {
        HttpResponse ParseResponse(string responseText)
        {
            HttpResponse response = new HttpResponse();

            StringReader reader = new StringReader(responseText);

            string statusLine = reader.ReadLine();

            string[] statusLineTokens = statusLine.Split();

            response.HttpVersion = statusLineTokens[0];
            response.StatusCode = int.Parse(statusLineTokens[1]);
            response.ReasonPhrase = statusLineTokens[2];

            do
            {
                string headerLine = reader.ReadLine();

                if (headerLine == "") break;

                string[] tokens = headerLine.Split(':');

                response.Headers[tokens[0]] = string.Join(":", tokens.Skip(1).ToArray()).Trim();
            }
            while (true);

            response.Entity = reader.ReadToEnd();

            return response;
        }

        internal string Get(string url)
        {
            string page = null;

            Uri uri = new Uri(url);

            using (TcpClient tcpClient = new TcpClient(uri.Host, 80))
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    string request = string.Format("GET / HTTP/1.1\r\n" +
                                                   "Host: {0}\r\n" +
                                                   "Connection: close\r\n" +
                                                   "\r\n", uri.Host);

                    byte[] bytes = Encoding.ASCII.GetBytes(request);

                    stream.Write(bytes, 0, bytes.Length);

                    using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                    {
                        string responseText = reader.ReadToEnd();

                        HttpResponse response = ParseResponse(responseText);

                        if (response.StatusCode == 302) return Get(response.Headers["Location"]);

                        page = response.Entity;
                    }
                }
            }

            return page;
        }
    }
}
