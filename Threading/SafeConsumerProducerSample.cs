using SamplesAPI;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    public class SafeConsumerProducerSample : ISample
    {
        int count = 0;

        int nProducer;
        int nConsumer;
        int producerMaxLatency;
        int consumerMaxLatency;

        ConcurrentQueue<int> consumed = new ConcurrentQueue<int>();
        BlockingCollection<int> queue = new BlockingCollection<int>();

        bool stopRequested = false;
        int runningProducers;

        void Producer(object data)
        {
            int offset = (int)data;
            string padding = new string(' ', offset);

            Random rand = new Random();

            while (!stopRequested)
            {
                // Simulate data creation
                Thread.Sleep(rand.Next(producerMaxLatency));

                // Keep track of the total number of data created
                int value = Interlocked.Increment(ref count);

                Console.WriteLine("{0}{1}", padding, value);

                queue.Add(value);
            }

            // Let the last producer thread mark the production as stopped
            if (Interlocked.Decrement(ref runningProducers) == 0)
            {
                queue.CompleteAdding();
            }
        }

        void Consumer(object data)
        {
            int offset = (int)data;

            string padding = new string(' ', offset);

            Random rand = new Random();

            do
            {
                try
                {
                    int value = queue.Take();

                    Thread.Sleep(rand.Next(consumerMaxLatency));

                    consumed.Enqueue(value);

                    Console.WriteLine("{0}{1}", padding, value);
                }
                catch (InvalidOperationException)
                {
                    // No more elements
                }
            }
            while (!queue.IsCompleted);
        }

        public SafeConsumerProducerSample(int nProducer, int nConsumer, int producerMaxLatency, int consumerMaxLatency)
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

            this.runningProducers = nProducer;

            for (int i = 1; i <= nConsumer; ++i)
            {
                Thread thread = new Thread(Consumer) { IsBackground = true };
                threads.Add(thread);
                thread.Start(offset);
                offset += 5;
            }

            Console.WriteLine("Press enter to stop...");
            Console.ReadLine();

            stopRequested = true;

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            IList<int> consumedList = consumed.ToList();

            if (consumedList.Count != count)
            {
                Console.Error.WriteLine("Consumed only {0} vs {1}!", consumedList.Count, count);
            }

            for (int i = 1; i <= count; ++i)
            {
                if (!consumedList.Contains(i))
                {
                    Console.Error.WriteLine("{0} not consumed!", i);
                    return;
                }
            }

            Console.WriteLine("All items consumed.");
        }
    }
}
