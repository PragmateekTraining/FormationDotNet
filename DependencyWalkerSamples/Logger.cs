using System.Runtime.InteropServices;

namespace Logging
{
    public class Logger
    {
        [DllImport("logging.dll", EntryPoint = "log", CallingConvention = CallingConvention.Cdecl)]
        public extern static void Log(string message);
    }
}
