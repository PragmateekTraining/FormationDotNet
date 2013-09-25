using System;
using System.Linq;
using Microsoft.Win32;
using SamplesAPI;

namespace WindowsRegistrySamples
{
    public class DotNetVersionsSample : ISample
    {
        public void Run()
        {
            using (RegistryKey NDP = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (string versionName in NDP.GetSubKeyNames().Where(skn => skn.StartsWith("v")))
                {
                    using (RegistryKey versionKey = NDP.OpenSubKey(versionName))
                    {
                        if (versionKey.GetValue(null) as string == "deprecated") continue;

                        string installPath = versionKey.GetValue("InstallPath") as string;

                        if (installPath == null)
                        {
                            RegistryKey fullKey = versionKey.OpenSubKey("Full");

                            if (fullKey != null)
                            {
                                installPath = fullKey.GetValue("InstallPath") as string;
                            }
                        }

                        Console.Write(versionName);

                        if (installPath != null)
                            Console.Write(" ({0})", installPath);

                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
