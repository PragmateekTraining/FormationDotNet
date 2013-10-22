using SamplesAPI;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ExceptionsSamples
{
    public class ExceptionDataSample : ISample
    {
        void Log(string logFilePath, string message)
        {
            try
            {
                using (FileStream file = File.Open(logFilePath, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    byte[] bytes = Encoding.Unicode.GetBytes(message);

                    file.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e)
            {
                Exception exception = new Exception("Unexpected exception while logging!", e);

                string prefix = this.GetType().FullName + ".Log";
                exception.Data[prefix + ".LogFilePath"] = logFilePath;
                exception.Data[prefix + ".Message"] = message;

                throw exception;
            }
        }

        string ToString(IDictionary dictionary)
        {
            StringBuilder builder = new StringBuilder("{\n");

            foreach (DictionaryEntry pair in dictionary)
            {
                builder.AppendFormat("\t{0}: {1}\n",  pair.Key, pair.Value ?? "null");
            }

            builder.Append("}");

            return builder.ToString();
        }

        public void Run()
        {
            string logFilePath = "logs.log";

            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            Console.WriteLine("==========");

            try
            {
                Log(logFilePath, "abc");
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception:\n{0}\n{1}", e, ToString(e.Data));
            }

            using (File.Create(logFilePath)) ;

            Console.WriteLine("==========");

            try
            {
                Log(logFilePath, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception:\n{0}\n{1}", e, ToString(e.Data));
            }

            Console.WriteLine("==========");

            try
            {
                Log(logFilePath, "abc");
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception:\n{0}\n{1}", e, ToString(e.Data));
            }
        }
    }
}
