using SamplesAPI;
using System;
using System.Runtime.InteropServices;

namespace ReversePInvokeSamples
{
    public class ReversePInvokeSample : ISample
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void Log(string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void Progress(double progress);

        [DllImport("calculator", CallingConvention = CallingConvention.Cdecl)]
        extern static int compute(int n, Progress progress, Log log);

        public void Run()
        {
            Progress progress = p => Console.Write("\r[{0,-60}] {1:N2}%{2}", new string('*', (int)Math.Round(p * 60)), p * 100, p == 1.0 ? "\n" : "");
            Log log = m => Console.WriteLine(m);

            int fn = compute(45, progress, log);

            Console.WriteLine(fn);
        }
    }
}
