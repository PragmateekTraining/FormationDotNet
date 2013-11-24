using SamplesAPI;
using System;

namespace ConstantValuesSamples
{
    public class FlasgsSample : ISample
    {
        enum E
        {
            None = 0,
            A = 1,
            B = 2,
            C = 4
        }

        [Flags]
        enum Flags
        {
            None = 0,
            A = 1,
            B = 2,
            C = 4
        }

        public void Run()
        {
            E e = (E)3;
            Flags f = (Flags)3;

            Console.WriteLine("e: {0}", e);
            Console.WriteLine("e: {0:f}", e);
            Console.WriteLine("e has A: {0}", e.HasFlag(E.A));
            Console.WriteLine("parse: {0}", Enum.Parse(typeof(E), "A,B,C"));

            Console.WriteLine("f : {0}", f);
            Console.WriteLine("f has A: {0}", f.HasFlag(Flags.A));
            Console.WriteLine("parse: {0}", Enum.Parse(typeof(Flags), "A,B,C"));
        }
    }
}
