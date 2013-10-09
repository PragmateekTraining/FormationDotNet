using ProtoBuf;
using SamplesAPI;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Logging
{
    class Server
    {
        public bool IsStarted { get; set; }

        StreamWriter file;

        void Log(LogMessage log)
        {
            string logString = string.Format("[{0}] ({1}) {2}", log.Timestamp, log.Level, log.Message);

            Console.WriteLine(logString);

            lock (file)
            {
                file.WriteLine(logString);
            }
        }

        void HandleClient(object o)
        {
            TcpClient client = o as TcpClient;

            NetworkStream stream = client.GetStream();
            BinaryReader reader = new BinaryReader(stream);

            byte[] buffer = new byte[1024];

            while (true)
            {
                int size;
                byte[] bytes;

                try
                {
                    size = IPAddress.NetworkToHostOrder(reader.ReadInt32());
                    bytes = reader.ReadBytes(size);
                }
                catch (Exception e)
                {
                    using (Color.Red)
                        Console.WriteLine("Caught exception while reading:\n{0}", e);

                    try { client.Close(); }
                    catch { }

                    break;
                }

                LogMessage message = null;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    message = Serializer.Deserialize<LogMessage>(ms);
                }

                Log(message);
            }
        }

        public void Start(int port, CancellationToken? token = null)
        {
            file = new StreamWriter(File.OpenWrite("logs.log"));

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            IsStarted = true;

            Console.WriteLine("Server started.");

            while (true)
            {
                if (token != null && token.Value.IsCancellationRequested)
                {
                    Console.WriteLine("Shutting down logging server.");
                    file.Dispose();
                    break;
                }

                if (!listener.Pending())
                {
                    Thread.Sleep(200);
                    continue;
                }

                TcpClient client = listener.AcceptTcpClient();
                Thread clientThread = new Thread(HandleClient) { IsBackground = true };
                clientThread.Start(client);
            }
        }

        public void StartAsync(int port, CancellationToken? token = null)
        {
            new Thread(() => Start(port, token)).Start();
        }
    }
}
