using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;

namespace Threading
{
    [TestFixture]
    class Stop
    {
        bool keepRunning = true;

        void DoWork(object o)
        {
            CancellationToken cancellationToken = (CancellationToken)o;

            try
            {
                while (keepRunning && !cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        [Test]
        public void CanAbortAThread()
        {
            CancellationTokenSource source = new CancellationTokenSource();

            Thread thread = new Thread(DoWork);
            thread.Start(source.Token);

            Thread.Sleep(1000);

            thread.Abort();

            Thread.Sleep(1000);

            Assert.AreEqual(ThreadState.Aborted, thread.ThreadState);

            thread = new Thread(DoWork);
            thread.Start(source.Token);

            keepRunning = false;

            Thread.Sleep(1000);

            Assert.AreEqual(ThreadState.Stopped, thread.ThreadState);

            thread = new Thread(DoWork);
            thread.Start(source.Token);

            source.Cancel();

            Thread.Sleep(1000);

            Assert.AreEqual(ThreadState.Stopped, thread.ThreadState);
        }
    }
}
