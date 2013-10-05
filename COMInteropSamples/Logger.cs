using System;
using System.IO;
using System.Runtime.InteropServices;

namespace COMInteropSamples
{
    [ComVisible(true)]
    [Guid("D9560D90-76F7-4666-B871-371EB1FDC3D6")]
    [ProgId("Production.Logger")]
    public class Logger
    {
        public string Path { get; set; }

        public void Log(string message)
        {
            File.AppendAllText(Path, string.Format("[{0}] {1}\n", DateTime.Now, message));
        }
    }
}
