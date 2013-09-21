using System;
using System.Threading;
using NUnit.Framework;

namespace Threading
{
    [TestFixture]
    class MutexOwnership
    {
        Mutex mutex = null;

        bool hasThrown = false;

        void TryReleaseMutex()
        {
            try
            {
                mutex.ReleaseMutex();
            }
            catch (Exception)
            {
                hasThrown = true;
            }
        }

        [Test]
        public void CanEnsureOwnership()
        {
            mutex = new Mutex();
            mutex.WaitOne();
            new Thread(TryReleaseMutex).Start();
            Thread.Sleep(1000);

            Assert.That(hasThrown);
        }
    }
}
