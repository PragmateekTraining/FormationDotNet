using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Chat
{
    public class Client
    {
        readonly Encoding encoding = Encoding.ASCII;

        TcpClient tcpClient = null;

        public event EventHandler<NewParticipantEventArgs> NewParticipant = delegate { };
        public event EventHandler<NewMessageEventArgs> NewMessage = delegate { };

        void Listen()
        {
            NetworkStream stream = tcpClient.GetStream();
            ClientStream clientStream = new ClientStream(stream);

            byte[] buffer = new byte[1024];

            while (true)
            {
                string commandName = clientStream.ReadCommand();
                long size = clientStream.ReadSize();

                if (commandName == "connect")
                {
                    string username = clientStream.ReadMessage(size);

                    NewParticipant(this, new NewParticipantEventArgs(username));
                }
                else if (commandName == "message")
                {
                    string username = clientStream.ReadToken();

                    string message = clientStream.ReadMessage(size);

                    NewMessage(this, new NewMessageEventArgs(username, message));
                }
            }
        }

        public void Connect(string url, int port, string username)
        {
            tcpClient = new TcpClient(url, port);
            string payload = string.Format("connect|{0}|{1}", username.Length, username);
            byte[] rawPayload = encoding.GetBytes(payload);
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(rawPayload, 0, rawPayload.Length);

            new Thread(Listen) { IsBackground = true }.Start();
        }

        public void SendMessage(string message)
        {
            string payload = string.Format("message|{0}|{1}", message.Length, message);
            byte[] rawPayload = encoding.GetBytes(payload);
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(rawPayload, 0, rawPayload.Length);
        }
    }
}
