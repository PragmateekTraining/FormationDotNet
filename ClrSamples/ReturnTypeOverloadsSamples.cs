using SamplesAPI;
using System.Diagnostics;

namespace ClrSamples
{
    public class ReturnTypeOverloadsSamples : ISample
    {
        public void Run()
        {
            Process test = new Process
            {
                StartInfo = new ProcessStartInfo(@"test.exe")
                {
                    UseShellExecute = false
                }
            };

            test.Start();
            test.WaitForExit();
        }
    }
}
