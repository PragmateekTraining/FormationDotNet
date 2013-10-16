using System;
using System.IO;
using System.Runtime.InteropServices;

namespace COMInteropSamples
{
    [ComVisible(true)]
    [Guid("FA4F18E7-2BB8-4020-8313-EACD5B9026F8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ILogger
    {
        string Path { get; set; }
        void Log(string message);
        void F();
    }

    [ComVisible(true)]
    [Guid("D9560D90-76F7-4666-B871-371EB1FDC3D6")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Production.Logger")]
    public class Logger : ILogger
    {
        public Logger()
        {
            Console.WriteLine("In!");
        }

        public string Path { get; set; }

        public void Log(string message)
        {
            File.AppendAllText(Path, string.Format("[{0}] {1}\n", DateTime.Now, message));
        }

        public void F()
        {
            Console.WriteLine("FFFFFFFFFFFFFFFF");
            File.WriteAllText("ffffffffffffffffff.txt", "ffffffffffffffffffffff");
        }
    }
}
