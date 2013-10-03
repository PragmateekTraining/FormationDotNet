using SamplesAPI;
using System;
using System.Runtime.InteropServices;

namespace PInvokeSamples
{
    public class UnionSample : ISample
    {
        [DllImport("NativeLibrary.dll", EntryPoint = "dump", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Dump(MyShort myShort);

        [StructLayout(LayoutKind.Explicit)]
        struct MyShort
        {
            [FieldOffset(0)]
            public byte FirstByte;
            [FieldOffset(1)]
            public byte SecondByte;
            [FieldOffset(0)]
            public short Value;
        }

        public void Run()
        {
            MyShort myShort = new MyShort { Value = 1 };
            Dump(myShort);
        }
    }
}
