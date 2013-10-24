using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiscSamples
{
    public class ReentrancySample : ISample
    {
        int myNumber;

        int NeitherReentrantNorThreadSafe(int n)
        {
            Random rand = new Random();

            myNumber = rand.Next();

            if (n == 1) return myNumber;

            return myNumber + NeitherReentrantNorThreadSafe(n - 1);
        }

        int n;

        int ReentrantButNotThreadSafe(int n)
        {
            this.n = n;

            return ReentrantButNotThreadSafe();
        }

        int ReentrantButNotThreadSafe()
        {
            Random rand = new Random();

            int myNumber = rand.Next();

            if (n == 1) return myNumber;

            --n;

            return myNumber + ReentrantButNotThreadSafe();
        }

        ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();

        int ThreadSafeButNotReentrant(int n)
        {
            Random rand = new Random();

            rwls.EnterWriteLock();
            try
            {
                myNumber = rand.Next();

                if (n == 1) return myNumber;

                return myNumber + ThreadSafeButNotReentrant(n - 1);
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        int ReentrantAndThreadSafe(int n)
        {
            Random rand = new Random();

            int myNumber = rand.Next();

            if (n == 1) return myNumber;

            return myNumber + ReentrantAndThreadSafe(n - 1);
        }

        public void Run()
        {
        }
    }
}
