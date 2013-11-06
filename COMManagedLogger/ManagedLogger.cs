using System;
using System.IO;
using System.Runtime.InteropServices;

namespace COMManagedLogger
{
    [ComVisible(true)]
    [Guid("32E63B20-70F0-42C5-B242-90A3BB2C075F")]
    public interface IManagedLogger
    {
        string Path { get; set; }
        void Log(string message);
    }

    [ComVisible(true)]
    [Guid("58595EDE-2AC8-4462-8E58-C5AEC4FC7A82")]
    public class ManagedLogger : IManagedLogger
    {
        public string Path { get; set; }

        public void Log(string message)
        {
            File.AppendAllText(Path, message);
        }
    }
}
