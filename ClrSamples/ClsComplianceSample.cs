using System;
using SamplesAPI;

namespace ClrSamples
{
    public class A
    {
        public void F() { }
        public UInt32 f() { return 42; }
    }

    [CLSCompliant(false)]
    public class B
    {
        public void F() { }
        public UInt32 f() { return 42; }
    }

    internal class C
    {
        public void F() { }
        public UInt32 f() { return 42; }
    }

    public class ClsComplianceSample : ISample
    {
        public void Run()
        {
        }
    }
}
