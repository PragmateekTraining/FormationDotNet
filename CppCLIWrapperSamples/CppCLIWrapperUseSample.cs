using System;
using SamplesAPI;
using System.IO;

namespace CppCLIWrapperSamples
{
    public class CppCLIWrapperUseSample : ISample
    {
        private void Build()
        {
            Tools.RunAndWait("build.bat");
        }

        public void Run()
        {
            Build();
            Tools.RunAndWait("CSharp/Test.exe");
            Console.WriteLine("Generated logs:\n{0}", File.ReadAllText("logs.log"));
        }
    }
}
