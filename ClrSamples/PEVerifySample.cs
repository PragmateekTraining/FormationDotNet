using SamplesAPI;
using System.Diagnostics;
using System;

namespace ClrSamples
{
    public class PEVerifySample : ISample
    {
        public void Run()
        {
            string peverifyPath = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\peverify.exe";

            Process safePEVerify = new Process
            {
                StartInfo = new ProcessStartInfo(peverifyPath, "safe.exe")
                {
                    UseShellExecute = false
                }
            };

            safePEVerify.Start();
            safePEVerify.WaitForExit();

            Console.WriteLine("==========");

            Process unsafePEVerify = new Process
            {
                StartInfo = new ProcessStartInfo(peverifyPath, "unsafe.exe")
                {
                    UseShellExecute = false
                }
            };

            unsafePEVerify.Start();
            unsafePEVerify.WaitForExit();
        }
    }
}
