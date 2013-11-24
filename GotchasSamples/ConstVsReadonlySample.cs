using System;
using SamplesAPI;
using System.IO;

namespace ConstantValuesSamples
{
    public class ConstVsReadonlySample : ISample
    {
        public void Run()
        {
            string template = File.ReadAllText("ConfigurationDataTemplate.cs");

            string v1 = template.Replace("{{n}}", "1");
            string v2 = template.Replace("{{n}}", "2");

            File.WriteAllText("ConfigurationData.cs", v1);

            Console.WriteLine("===Compiling configuration v1===");

            Tools.RunAndWait("csc", "/t:library ConfigurationData.cs");

            Console.WriteLine("===Compiling user===");

            Tools.RunAndWait("csc", "/r:ConfigurationData.dll ConfigurationUser.cs");

            Console.WriteLine("===Running user===");

            Tools.RunAndWait("ConfigurationUser.exe");

            File.WriteAllText("ConfigurationData.cs", v2);

            Console.WriteLine("===Compiling configuration v2===");

            Tools.RunAndWait("csc", "/t:library ConfigurationData.cs");

            Console.WriteLine("===Running user===");

            Tools.RunAndWait("ConfigurationUser.exe");
        }
    }
}
