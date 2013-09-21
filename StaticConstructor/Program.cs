using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticConstructor
{
    class MyAwesomeLibrary
    {
        static MyAwesomeLibrary()
        {
            Console.WriteLine(string.Format("Hey {0} is using me!", Environment.UserName));
        }

        public static int GetTheAnswer()
        {
            return 42;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The answer is: " + MyAwesomeLibrary.GetTheAnswer());
        }
    }
}
