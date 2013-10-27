using RGiesecke.DllExport;
using System;
using System.Runtime.InteropServices;

namespace UnmanagedExports
{
    public class A
    {
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static int Add(int a, int b)
        {
            return a + b;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
