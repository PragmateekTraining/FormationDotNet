using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryAffinitySamples
{
    public class BinaryAffinitySample : ISample
    {
        public void Run()
        {
            Console.WriteLine("===Compiling library===");
            Tools.RunAndWait("csc", "/t:library Dependency.cs");

            Console.WriteLine("===Compiling application===");
            Tools.RunAndWait("csc", "/r:Dependency.dll Application.cs");

            Console.WriteLine("===Dumping library corflags===");
            Tools.RunAndWait("corflags", "Dependency.dll");

            Console.WriteLine("===Dumping application corflags===");
            Tools.RunAndWait("corflags", "Application.exe");

            Console.WriteLine("===Running application===");
            Tools.RunAndWait("Application.exe");

            Console.WriteLine("===Setting 32-bits affinity on library===");
            Tools.RunAndWait("corflags", "/32bitreq+ Dependency.dll");

            Console.WriteLine("===Dumping library corflags===");
            Tools.RunAndWait("corflags", "Dependency.dll");

            Console.WriteLine("===Running application===");
            Tools.RunAndWait("Application.exe");

            Console.WriteLine("===Setting 32-bits affinity on application===");
            Tools.RunAndWait("corflags", "/32bitreq+ Application.exe");

            Console.WriteLine("===Dumping application corflags===");
            Tools.RunAndWait("corflags", "Application.exe");

            Console.WriteLine("===Running application===");
            Tools.RunAndWait("Application.exe");
        }
    }
}
