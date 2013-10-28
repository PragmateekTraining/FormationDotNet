using SamplesAPI;
using System;
using System.Runtime.InteropServices;

namespace PInvokeSamples
{
    public class ArraysSample : ISample
    {
        [DllImport("NativeLibrary.dll", EntryPoint = "sum", CallingConvention = CallingConvention.Cdecl)]
        extern static uint Sum(uint size, uint[] data);

        public void Run()
        {
            const uint n = 10;

            uint[] data = new uint[n];

            Random rand = new Random();

            uint thSum = 0;
            for (uint i = 0; i < n; ++i)
            {
                data[i] = (uint)rand.Next(10);
                thSum += data[i];
            }

            uint expSum = Sum(n, data);

            Console.WriteLine("{0} / {1}: {2}", thSum, expSum, expSum == thSum ? "OK" : "X");
        }
    }
}
