using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadingSamples
{
    /// <summary>
    /// Illustrate the reentrant behavior of some synchronization primitives.
    /// </summary>
    public class ReentrancySample : ISample
    {
        public void Run()
        {
            // Mutex supports reentrancy.
            using (Mutex mutex = new Mutex())
            {
                Console.WriteLine("First wait");
                mutex.WaitOne();
                Console.WriteLine("Second wait");
                mutex.WaitOne();
            }

            Console.WriteLine("Done with mutex");

            // ReaderWriterLockSlim does not support reentrancy by default...
            using (ReaderWriterLockSlim rwls = new ReaderWriterLockSlim())
            {
                try
                {
                    Console.WriteLine("First wait");
                    rwls.EnterWriteLock();
                    Console.WriteLine("Second wait");
                    try
                    {
                        // Will throw
                        rwls.EnterWriteLock();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caugh exception:\n{0}", e);
                    }
                }
                finally // Exit only the first lock acquisition
                {
                    rwls.ExitWriteLock();
                }
            }

            Console.WriteLine("Done with reader-writer lock");

            // ... but it can be changed.
            using (ReaderWriterLockSlim rwls = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                try
                {
                    Console.WriteLine("First wait");
                    rwls.EnterWriteLock();
                    Console.WriteLine("Second wait");
                    try
                    {
                        // OK because the lock is reentrant
                        rwls.EnterWriteLock();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caugh exception:\n{0}", e);
                    }
                    finally // Exit the first lock acquisition
                    {
                        rwls.ExitWriteLock();
                    }
                }
                finally // And exit the second lock acquisition
                {
                    rwls.ExitWriteLock();
                }
            }

            Console.WriteLine("Done with reentrant reader-writer lock");
        }
    }
}
