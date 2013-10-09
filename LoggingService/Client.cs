using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Logging
{
    public class Client : IDisposable
    {
        TcpClient client;
        BinaryWriter writer;

        public Client(string url, int port)
        {
            client = new TcpClient(url, port);
            writer = new BinaryWriter(client.GetStream());
        }

        public void Log(Level level, string message)
        {
            LogMessage log = new LogMessage
            {
                Timestamp = DateTime.UtcNow.ToString("o"),
                Level = level,
                Message = message
            };

            byte[] bytes;

            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, log);
                bytes = ms.ToArray();
            }

            int size = IPAddress.HostToNetworkOrder(bytes.Length);

            writer.Write(size);
            writer.Write(bytes);
        }

        public void Dispose()
        {
            writer.Dispose();
            client.Close();
        }
    }
}
