using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server().Start(int.Parse(args[0]));
        }
    }
}
