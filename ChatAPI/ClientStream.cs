using System.Net.Sockets;
using System.IO;
using System;
using System.Text;

namespace Chat
{
    public class ClientStream
    {
        readonly NetworkStream stream = null;
        readonly byte[] buffer = new byte[1024];
        int i;
        int end;

        public ClientStream(NetworkStream stream)
        {
            this.stream = stream;
        }

        void ReadMore()
        {
            end = stream.Read(buffer, 0, buffer.Length);
            i = 0;
        }

        public string ReadToken()
        {
            string token = "";

            while (true)
            {
                while (i < end && buffer[i] != '|')
                {
                    token += (char)buffer[i];
                    ++i;
                }

                if (i != end)
                {
                    ++i;
                    break;
                }

                ReadMore();
            }

            return token;
        }

        public string ReadCommand()
        {
            return ReadToken();
        }

        public long ReadSize()
        {
            /*string sizeStr = "";

            while (true)
            {
                while (i < end && buffer[i] != '|')
                {
                    sizeStr += (char)buffer[i];
                    ++i;
                }

                if (i != end)
                {
                    ++i;
                    break;
                }

                ReadMore();
            }*/

            string sizeStr = ReadToken();

            return long.Parse(sizeStr);
        }

        public void ReadContent(long size, Stream toStream)
        {
            long total = 0;

            while (true)
            {
                while (i < end && total < size)
                {
                    int toRead = (int)Math.Min(end - i, size - total);
                    toStream.Write(buffer, i, toRead);
                    total += toRead;
                    i += toRead;
                }

                if (total == size)
                {
                    break;
                }

                ReadMore();
            }
        }

        public string ReadMessage(long size)
        {
            MemoryStream memory = new MemoryStream();

            ReadContent(size, memory);

            return Encoding.ASCII.GetString(memory.ToArray());
        }
    }
}
