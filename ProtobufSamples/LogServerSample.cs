using Logging;
using SamplesAPI;
using System;

namespace ProtobufSamples
{
    public class LogServerSample : ISample
    {
        string host;
        int port;

        public LogServerSample(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Run()
        {
            using (Client client = new Client(host, port))
            {
                string message = null;
                while (!string.IsNullOrWhiteSpace(message = Console.ReadLine()))
                {
                    client.Log(Level.INFO, message);
                }
            }
        }
    }
}
