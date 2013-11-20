using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace ThreadingSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // LifeCycle.Run();
            // Priorities.Run();
            // UI.Run();
            // new WaitPulse().Run();
            // new MutexOwnership().CanEnsureOwnership();
            // new Scalability().Run();
            // new BackgroundVsForeground().Run(true);
            // new Downloader().Run();
            // new BarrierSample(args.Length == 1 && args[0] == "use-barrier").Run();
            // new ReentrancySample().Run();
            // new ReaderWriterLockSample(1000, 0m, 1m, 0.05m, 4).Run();
            // new ThreadStaticSample().Run();
            // new ThreadLocalSample().Run();
            // new RaceSample().Run();
            // new DeadLockSample().Run();
            // new MonitorRaceSample().Run();
            // new EventWaitHandleSample().Run();
            // new InterlockedExchangeSample().Run();
            // new InterlockedIncrementSample().Run();
            // new SimpleConsumerProducerSample(2, 2, 2000, 4000).Run();
            // new SafeConsumerProducerSample(2, 2, 2000, 4000).Run();
            // new ConcurrentQueueSample().Run();
            // new CountdownEventRaceSample().Run();
            // new ClosureSample().Run();
            // new BeginEndInvokeSample().Run();
            // new LockFreeSample().Run();
            // new LockFreeSample2().Run();
            // new PartitioningSample().Run();
            // new OptimisticConcurrencySample().Run();
            // new ThreadPoolSample().Run();
            new SemaphoreSample().Run();
        }
    }
}
