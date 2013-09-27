using System;
using System.Reflection;

namespace ILMergeSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly abc = AppDomain.CurrentDomain.Load("ABC");

            object a = abc.CreateInstance("A.A");
            a.GetType().InvokeMember("F", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);

            object b = abc.CreateInstance("B.B");
            b.GetType().InvokeMember("F", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);

            object c = abc.CreateInstance("C.C");
            c.GetType().InvokeMember("F", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
        }
    }
}
