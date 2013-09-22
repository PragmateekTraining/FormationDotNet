using System;

namespace Chat
{
    public class NewParticipantEventArgs : EventArgs
    {
        public string Username { get; private set; }

        public NewParticipantEventArgs(string username)
        {
            Username = username;
        }
    }
}
