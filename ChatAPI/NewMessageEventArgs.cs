using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat
{
    public class NewMessageEventArgs : EventArgs
    {
        public string Username { get; private set; }
        public string Message { get; private set; }

        public NewMessageEventArgs(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }
}
