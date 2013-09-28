using System;

namespace ReflectionSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[0] == "performance")
                {
                    long n = Convert.ToInt64(args[1]);

                    new PerformanceSample(n).Run();
                }
            }
        }
    }
}
