using System.IO;
using Managed;

namespace CppCLIWrapperSamples.CSharp
{
    class Test
    {
        static void Main()
        {
            string logsFile = "logs.log";

            if (File.Exists(logsFile))
            {
                File.Delete(logsFile);
            }

            Logger logger = new Logger(logsFile);
            logger.Log("message 1");
            logger.Log("message 2");
            logger.Log("message 3");
        }
    }
}
