using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            while (true)
            {
                Console.Write((client.Url ?? "") + "> ");

                string commandLine = Console.ReadLine();

                string[] tokens = commandLine.Split();

                string command = tokens[0];

                try
                {
                    if (command == "connect")
                    {
                        string ftp = tokens[1];

                        IList<string> listing = client.Connect(ftp);

                        foreach (string entry in listing)
                        {
                            Console.WriteLine(entry);
                        }
                    }
                    else if (command == "list")
                    {
                        string directory = tokens[1];

                        IList<string> listing = client.List(directory);

                        foreach (string entry in listing)
                        {
                            Console.WriteLine(entry);
                        }
                    }
                    else if (command == "download")
                    {
                        string file = tokens[1];

                        client.Download(file);
                    }
                    else if (command == "quit")
                    {
                        break;
                    }
                    else
                    {
                        Console.Error.WriteLine("Unknown command '{0}'", command);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Unexpected error: {0}", e);
                }
            }
        }
    }
}
