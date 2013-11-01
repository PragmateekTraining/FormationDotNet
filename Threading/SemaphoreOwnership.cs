using System;
using System.Threading;
using NUnit.Framework;

namespace ThreadingSamples
{
    [TestFixture]
    class SemaphoreOwnership
    {
        Semaphore semaphore = null;

        bool hasThrown = false;

        void TryReleaseSemaphore()
        {
            try
            {
                semaphore.Release();
            }
            catch (Exception)
            {
                hasThrown = true;
            }
        }

        [Test]
        public void DoNotEnsureOwnership()
        {
            semaphore = new Semaphore(1, 1);
            semaphore.WaitOne();
            new Thread(TryReleaseSemaphore).Start();
            Thread.Sleep(1000);

            Assert.That(!hasThrown);
        }
    }
}
