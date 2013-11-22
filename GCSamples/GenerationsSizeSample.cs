using SamplesAPI;
using System;
using System.Collections.Generic;

namespace GCSamples
{
    public class GenerationsSizeSample : ISample
    {
        IList<object> list = new List<object>();

        public void Run()
        {
            while (true)
            {
                Console.Write("How many objects to add? ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                if (input == "clear")
                {
                    Console.WriteLine("Clearing...");
                    list.Clear();
                    continue;
                }

                if (input == "collect")
                {
                    Console.WriteLine("Collecting...");
                    GC.Collect();
                    continue;
                }

                int n;
                if (!int.TryParse(input, out n))
                {
                    Console.WriteLine("Bad input!");
                    continue;
                }

                for (int i = 1; i <= n; ++i)
                {
                    list.Add(new object());
                }
            }
        }
    }
}
