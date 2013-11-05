using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class SimpleConsumerProducerSample : ISample
    {
        int count = 0;

        int nProducer;
        int nConsumer;
        int producerMaxLatency;
        int consumerMaxLatency;

        BlockingCollection<int> queue = new BlockingCollection<int>();

        void Producer(object data)
        {
            int offset = (int)data;

            string padding = new string(' ', offset);

            Random rand = new Random();

            while (true)
            {
                int value = Interlocked.Increment(ref count);

                Console.WriteLine("{0}{1}", padding, value);

                queue.Add(value);

                Thread.Sleep(rand.Next(producerMaxLatency));
            }
        }

        void Consumer(object data)
        {
            int offset = (int)data;

            string padding = new string(' ', offset);

            Random rand = new Random();

            while (true)
            {
                int value = queue.Take();

                Console.WriteLine("{0}{1}", padding, value);

                Thread.Sleep(rand.Next(consumerMaxLatency));
            }
        }

        public SimpleConsumerProducerSample(int nProducer, int nConsumer, int producerMaxLatency, int consumerMaxLatency)
        {
            this.nProducer = nProducer;
            this.nConsumer = nConsumer;
            this.producerMaxLatency = producerMaxLatency;
            this.consumerMaxLatency = consumerMaxLatency;
        }

        public void Run()
        {
            StringBuilder header = new StringBuilder();
            for (int i = 1; i <= nProducer; ++i)
            {
                header.AppendFormat("P{0}   ", i);
            }

            for (int i = 1; i <= nConsumer; ++i)
            {
                header.AppendFormat("C{0}   ", i);
            }

            Console.WriteLine(header);

            IList<Thread> threads = new List<Thread>();

            int offset = 0;
            for (int i = 1; i <= nProducer; ++i)
            {
                Thread thread = new Thread(Producer) { IsBackground = true };
                threads.Add(thread);
                thread.Start(offset);
                offset += 5;
            }

            for (int i = 1; i <= nConsumer; ++i)
            {
                Thread thread = new Thread(Consumer) { IsBackground = true };
                threads.Add(thread);
                thread.Start(offset);
                offset += 5;
            }

            Console.ReadLine();
        }
    }
}
