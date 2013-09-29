using System.Diagnostics;

namespace SamplesAPI
{
    public class Tools
    {
        public static void RunAndWait(string exe, string args = null)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo(exe, args)
                {
                    UseShellExecute = false
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}
