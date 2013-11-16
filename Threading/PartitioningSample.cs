using SamplesAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ThreadingSamples
{
    public class PartitioningSample : ISample
    {
        class StaticExecutor
        {
            void Worker(object o)
            {
                Action[] tasks = (Action[])o;

                for (int i = 0; i < tasks.Length; ++i)
                {
                    tasks[i]();
                }
            }

            public void Run(Action[] tasks, int nWorkers)
            {
                nWorkers = Math.Min(tasks.Length, nWorkers);

                int n = tasks.Length / nWorkers;

                IList<Thread> workers = new List<Thread>();

                for (int i = 0; i < tasks.Length; i += n)
                {
                    Action[] workerTasks = new Action[Math.Min(tasks.Length - i, n)];

                    for (int j = 0; j < workerTasks.Length; ++j)
                    {
                        workerTasks[j] = tasks[i + j];
                    }

                    Thread worker = new Thread(Worker) { IsBackground = true };
                    worker.Start(workerTasks);
                    workers.Add(worker);
                }

                foreach (Thread worker in workers)
                {
                    worker.Join();
                }
            }
        }

        class DynamicExecutor
        {
            BlockingCollection<Action> queue = new BlockingCollection<Action>();

            void Worker()
            {
                Action task;
                while (queue.TryTake(out task))
                {
                    task();
                }
            }

            public void Run(Action[] tasks, int nWorkers)
            {
                nWorkers = Math.Min(tasks.Length, nWorkers);

                foreach (Action task in tasks)
                {
                    queue.Add(task);
                }

                IList<Thread> workers = new List<Thread>();

                for (int i = 1; i <= nWorkers; ++i)
                {
                    Thread worker = new Thread(Worker) { IsBackground = true };
                    worker.Start();
                    workers.Add(worker);
                }

                foreach (Thread worker in workers)
                {
                    worker.Join();
                }
            }
        }

        public void Run()
        {
            const int n = 2;

            Action[] tasks = Enumerable.Range(1, 6).Select(i => (Action)(() => Thread.Sleep(100 * i * i))).ToArray();

            StaticExecutor staticExecutor = new StaticExecutor();
            DynamicExecutor dynamicExecutor = new DynamicExecutor();

            Stopwatch stopwatch = Stopwatch.StartNew();
            staticExecutor.Run(tasks, n);
            stopwatch.Stop();
            TimeSpan staticTime = stopwatch.Elapsed;

            stopwatch.Restart();
            dynamicExecutor.Run(tasks, n);
            stopwatch.Stop();
            TimeSpan dynamicTime = stopwatch.Elapsed;

            Console.WriteLine("Static executor time: {0}.", staticTime);
            Console.WriteLine("Dynamic executor time: {0}.", dynamicTime);
        }
    }
}
