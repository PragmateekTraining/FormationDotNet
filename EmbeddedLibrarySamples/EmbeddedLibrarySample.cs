using SamplesAPI;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EmbeddedLibrarySamples
{
    public class EmbeddedLibrarySample : ISample
    {
        [DllImport("NativeLibrary.dll", EntryPoint = "print", CallingConvention = CallingConvention.Cdecl)]
        extern static void Print(string s);

        public void Run()
        {
            using (Stream outputFile = File.Create("NativeLibrary.dll"))
            {
                Assembly.GetExecutingAssembly().GetManifestResourceStream("NativeLibrary.dll").CopyTo(outputFile);
            }

            Print("a");
            Print("b");
            Print("c");
        }
    }
}
