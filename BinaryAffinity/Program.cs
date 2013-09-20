using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryAffinity
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The answer is: {0}", Dependency.Dependency.GetAnswer());
        }
    }
}
