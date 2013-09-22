using System.Net.Sockets;

namespace Chat
{
    public class Participant
    {
        public string Username { get; set; }
        public string Address { get; set; }
        public TcpClient TcpClient { get; set; }
    }
}
