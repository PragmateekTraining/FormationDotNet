using System;

namespace BinaryAffinitySamples
{
    public class Application
    {
        static void Main()
        {
            Console.WriteLine("The answer is: {0}", Dependency.Dependency.GetAnswer());
        }
    }
}
