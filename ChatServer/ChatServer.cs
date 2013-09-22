using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Chat
{
    public class Server
    {
        readonly Encoding encoding = Encoding.ASCII;

        IList<Participant> clients = new List<Participant>();

        public bool IsStarted { get; set; }

        public event EventHandler<NewParticipantEventArgs> NewParticipant = delegate { };
        public event EventHandler<NewMessageEventArgs> NewMessage = delegate { };

        void BroadCast(byte[] message, Participant from)
        {
            foreach (Participant client in clients)
            {
                if (client == from) continue;

                NetworkStream s = client.TcpClient.GetStream();
                s.Write(message, 0, message.Length);
                s.Flush();
            }
        }

        void HandleClient(object o)
        {
            Participant client = o as Participant;

            NetworkStream stream = client.TcpClient.GetStream();
            ClientStream clientStream = new ClientStream(stream);

            byte[] buffer = new byte[1024];

            while (true)
            {
                string commandName = clientStream.ReadCommand();
                long size = clientStream.ReadSize();

                if (commandName == "connect")
                {
                    MemoryStream memory = new MemoryStream();
                    clientStream.ReadContent(size, memory);
                    byte[] rawUsername = memory.ToArray();

                    string username = encoding.GetString(rawUsername);
                    client.Username = username;

                    Console.WriteLine("User '{0}' joined chat.", username);

                    NewParticipant(this, new NewParticipantEventArgs(username));

                    MemoryStream output = new MemoryStream();
                    byte[] header = encoding.GetBytes("connect|" + rawUsername.Length + "|");
                    output.Write(header, 0, header.Length);
                    output.Write(rawUsername, 0, rawUsername.Length);
                    BroadCast(output.ToArray(), client);
                }
                else if (commandName == "message")
                {
                    MemoryStream memory = new MemoryStream();
                    clientStream.ReadContent(size, memory);
                    byte[] rawMessage = memory.ToArray();

                    string message = encoding.GetString(rawMessage);

                    Console.WriteLine("User '{0}' says '{1}'.", client.Username, message);

                    NewMessage(this, new NewMessageEventArgs(client.Username, message));

                    MemoryStream output = new MemoryStream();
                    byte[] header = encoding.GetBytes("message|" + rawMessage.Length + "|" + client.Username + "|");
                    output.Write(header, 0, header.Length);
                    output.Write(rawMessage, 0, rawMessage.Length);                    
                    BroadCast(output.ToArray(), client);
                }
            }
        }

        public void Start(int port, CancellationToken? token = null)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            IsStarted = true;

            while (true)
            {
                if (token != null && token.Value.IsCancellationRequested)
                {
                    Console.WriteLine("Shutting down server.");
                    break;
                }

                if (!listener.Pending())
                {
                    Thread.Sleep(200);
                    continue;
                }

                TcpClient tcpClient = listener.AcceptTcpClient();
                Participant client = new Participant { TcpClient = tcpClient };
                clients.Add(client);
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
