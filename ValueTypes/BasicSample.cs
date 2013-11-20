using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueTypesSamples
{
    public class BasicSample : ISample
    {
        struct Value
        {
            byte B;
            int N;
            double D;
        }

        public void Run()
        {
            int a = 1;
            int b = a;
            b = 2;

            Console.WriteLine(a);
            Console.WriteLine(b);

            Object reference1 = new Object();
            Object reference2 = new Object();
            reference2 = reference1;

            /* int input = 1;
            object box = input;
            int output = (int)box;*/

            double input = 1.0;
            object box = input;
            int asIntKO = (int)box;
            int asIntOK = (int)(double)box;
        }
    }
}
