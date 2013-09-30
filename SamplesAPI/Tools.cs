using System;
using System.Diagnostics;

namespace SamplesAPI
{
    public class Tools
    {
        public static void RunAndWait(string exe, string args = null, bool echo = true)
        {
            if (echo)
            {
                ConsoleColor color = Console.ForegroundColor;

                try
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("> " + exe + " " + args ?? "");
                }
                finally
                {
                    Console.ForegroundColor = color;
                }
            }

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
