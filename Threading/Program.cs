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
            // new BackgroundVsForeground().Run(true);
            // new BackgroundWorkerSample().Run();
            // new BarrierSample(args.Length == 1 && args[0] == "use-barrier").Run();
            // new BeginEndInvokeSample().Run();
            // new Downloader().Run();
            // new LockFreeSample().Run();
            // new LockFreeSample2().Run();
            new MemoryVisibilitySample().Run();
            // new MutexOwnership().CanEnsureOwnership();
            // new OptimisticConcurrencySample().Run();
            // new PartitioningSample().Run();
            // new ReaderWriterLockSample(1000, 0m, 1m, 0.05m, 4).Run();
            // new ReentrancySample().Run();            
            // new Scalability().Run();
            // new SemaphoreSample().Run();
            // new ThreadPoolSample().Run();
            // new WaitPulse().Run();            
            
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
        }
    }
}
