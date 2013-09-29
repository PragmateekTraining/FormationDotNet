using System;
using SamplesAPI;

namespace GotchasSamples
{
    public class EnumsSample : ISample
    {
        public void Run()
        {
            Console.WriteLine("===Compiling enum v1===");
            Tools.RunAndWait("csc", "/t:library /out:E.dll EV1.cs");

            Console.WriteLine("===Compiling factory===");
            Tools.RunAndWait("csc", "/t:library /r:E.dll EnumFactory.cs");

            Console.WriteLine("===Compiling user===");
            Tools.RunAndWait("csc", "/r:E.dll /r:EnumFactory.dll EnumUser.cs");

            Console.WriteLine("===Running user===");
            Tools.RunAndWait("EnumUser.exe");


            Console.WriteLine("===Compiling enum v2===");
            Tools.RunAndWait("csc", "/t:library /out:E.dll EV2.cs");

            Console.WriteLine("===Running user===");
            Tools.RunAndWait("EnumUser.exe");
        }
    }
}
