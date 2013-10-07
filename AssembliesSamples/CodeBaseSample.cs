using SamplesAPI;
using System.IO;

namespace AssembliesSamples
{
    public class CodeBaseSample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait("build.bat");

            const string configFile = "Application.exe.config";

            if (File.Exists(configFile))
            {
                File.Delete(configFile);
            }

            Tools.RunAndWait("Application.exe");

            File.Copy(configFile + ".in", configFile);

            Tools.RunAndWait("Application.exe");
        }
    }
}
