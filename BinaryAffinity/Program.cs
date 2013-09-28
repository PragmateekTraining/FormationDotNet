using System;

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
