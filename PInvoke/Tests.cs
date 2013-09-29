using System;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace PInvokeSamples
{
    [TestFixture]
    class Tests
    {
        [DllImport("msvcrt.dll", EntryPoint = "srand")]
        private static extern int SRand(int seed);
        
        [DllImport("msvcrt.dll", EntryPoint = "rand")]
        private static extern int Rand();

        [Test]
        public void CanGenerateRandomNumbers()
        {
            SRand((int)DateTime.Now.Ticks);
            Console.WriteLine(Rand());
            Console.WriteLine(Rand());
            Console.WriteLine(Rand());
            Console.WriteLine(Rand());
        }

        [DllImport("NativeLibrary.dll", EntryPoint = "super_fast_add")]
        private static extern int Add(int a, int b);

        [Test]
        public void CanUseALocalNativeDLL()
        {
            Assert.AreEqual(3, Add(1, 2));
        }

        /*[DllImport("msvcrt.dll", EntryPoint = "puts", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Puts([MarshalAs(UnmanagedType.LPStr)] string s);

        [DllImport("msvcrt.dll", EntryPoint = "_flushall")]
        internal static extern int FlushAll();

        [Test]
        public void CanDisplaySomeText()
        {
            Puts("Hey! It's me!");
            FlushAll();
        }*/
    }
}
