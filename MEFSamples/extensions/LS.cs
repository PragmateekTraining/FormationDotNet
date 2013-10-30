using System; // Console
using System.IO; // Directory
using System.Linq; // ToList(this IEnumerable&lt;>)
using System.ComponentModel.Composition; // Export

using Shell; // ICommand, IShell


namespace LS
{
    [Export(typeof(ICommand))]
    public class LS : ICommand
    {
        public string Name
        {
            get
            {
                return "ls";
            }
        }

        public string Documentation
        {
            get
            {
                return "List the items of a directory.";
            }
        }

        public void Execute(IShell shell, string[] parameters)
        {
            Directory.GetFileSystemEntries(parameters.Count() == 0 ? "." : parameters[0]).ToList().ForEach(entry => Console.WriteLine(entry));
        }
    }
}