using System;
using System.Messaging;

namespace MSMQ
{
    class Program
    {
        const string queueName = @".\Private$\MessageLogger";

        static void Main(string[] args)
        {
            if (!MessageQueue.Exists(queueName))
            {
                Console.Error.WriteLine("No queue named '{0}'!", queueName);
                return;
            }

            MessageQueue messageQueue = new MessageQueue(queueName);

            string message = null;
            while ((message = Console.ReadLine()) != "")
            {
                messageQueue.Send(message);
            }
        }
    }
}
