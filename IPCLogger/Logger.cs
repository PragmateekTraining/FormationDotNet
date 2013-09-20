using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;

namespace IPCLogger
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class Logger : ILogger
    {
        private readonly string path = null;

        public Logger(string path)
        {
            this.path = path;
        }

        public void Log(string message)
        {
            Console.WriteLine("Logging '{0}'", message);
            File.AppendAllLines(path, new[] { message });
        }
    }
}
