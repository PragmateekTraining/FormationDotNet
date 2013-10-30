using System;
using System.ComponentModel.Composition;
using Shell;

namespace Echo
{
    [Export(typeof(ICommand))]
    public class Echo : ICommand
    {
        public string Name
        {
            get
            {
                return "echo";
            }
        }

        public string Documentation
        {
            get
            {
                return "Print some strings of characters.";
            }
        }

        public void Execute(IShell shell, string[] parameters)
        {
            Console.WriteLine(String.Join(" ", parameters));
        }
    }
}