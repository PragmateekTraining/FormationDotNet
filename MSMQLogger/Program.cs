using System;
using System.Messaging;
using System.IO;

namespace MSMQLogger
{
    class Program
    {
        const string queueName = @".\Private$\MessageLogger";

        const string path = "logs.log";

        static void Main(string[] args)
        {
            MessageQueue messageQueue = null;
            if (MessageQueue.Exists(queueName))
            {
                messageQueue = new MessageQueue(queueName);
                // messageQueue.Label = "MessageLogger";
            }
            else
            {
                messageQueue = MessageQueue.Create(queueName);
                // messageQueue.Label = "Newly Created Queue";
            }

            Console.WriteLine("Logger started.");

            while (true)
            {
                Message message = messageQueue.Receive();
                message.Formatter = new XmlMessageFormatter(new []{ typeof(string) });

                Console.WriteLine("Logging '{0}'", message.Body as string);
                File.AppendAllLines(path, new[] { message.Body as string });
            }
        }
    }
}
