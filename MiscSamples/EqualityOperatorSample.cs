using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiscSamples
{
    public class EqualityOperatorSample : ISample
    {
        class A
        {
            public override bool Equals(object obj)
            {
                Console.WriteLine("A.Equals");
                return base.Equals(obj);
            }

            public static bool operator ==(A a1, A a2)
            {
                Console.WriteLine("A.==");
                return true;
            }

            public static bool operator !=(A a1, A a2)
            {
                return !(a1 == a2);
            }
        }

        class B : A
        {
            public override bool Equals(object obj)
            {
                Console.WriteLine("B.Equals");
                return base.Equals(obj);
            }

            public static bool operator ==(B b1, B b2)
            {
                Console.WriteLine("B.==");
                return true;
            }

            public static bool operator !=(B b1, B b2)
            {
                return !(b1 == b2);
            }
        }

        public void Run()
        {
            A a1 = new A();
            A a2 = new A();

            A b1 = new B();
            A b2 = new B();

            Console.WriteLine("a1 == a2: {0}", a1 == a2);
            Console.WriteLine("a1.Equals(a2): {0}", a1.Equals(a2));
            Console.WriteLine("==========");
            Console.WriteLine("b1 == b2: {0}", b1 == b2);
            Console.WriteLine("b1.Equals(b2): {0}", b1.Equals(b2));
        }
    }
}
