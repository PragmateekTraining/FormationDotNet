using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Threading
{
    public class ReentrancySample : ISample
    {
        public void Run()
        {
            using (Mutex mutex = new Mutex())
            {
                Console.WriteLine("First wait");
                mutex.WaitOne();
                Console.WriteLine("Second wait");
                mutex.WaitOne();
            }

            Console.WriteLine("Done with mutex");

            using (ReaderWriterLockSlim rwls = new ReaderWriterLockSlim())
            {
                try
                {
                    Console.WriteLine("First wait");
                    rwls.EnterWriteLock();
                    Console.WriteLine("Second wait");
                    try
                    {
                        rwls.EnterWriteLock();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caugh exception:\n{0}", e);
                    }
                }
                finally
                {
                    rwls.ExitWriteLock();
                }
            }

            Console.WriteLine("Done with reader-writer lock");

            using (ReaderWriterLockSlim rwls = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion))
            {
                try
                {
                    Console.WriteLine("First wait");
                    rwls.EnterWriteLock();
                    Console.WriteLine("Second wait");
                    try
                    {
                        rwls.EnterWriteLock();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caugh exception:\n{0}", e);
                    }
                    finally
                    {
                        rwls.ExitWriteLock();
                    }
                }
                finally
                {
                    rwls.ExitWriteLock();
                }
            }

            Console.WriteLine("Done with reentrant reader-writer lock");
        }
    }
}
