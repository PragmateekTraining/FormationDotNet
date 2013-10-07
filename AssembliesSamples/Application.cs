using System;
using Dependency;

namespace AssembliesSamples
{
    class Application
    {
        static void Main()
        {
            Console.WriteLine("The answer is: " + DeepThought.GetAnswer());
        }
    }
}
