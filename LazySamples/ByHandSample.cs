using System;

namespace LazySamples
{
    class ByHandSample
    {
        object notThreadSafeLazy;
        public object NotThreadSafeLazy
        {
            get
            {
                // return (value = value ?? new Object());

                if (notThreadSafeLazy == null)
                {
                    notThreadSafeLazy = new Object();
                }

                return notThreadSafeLazy;
            }
        }

        object threadSafeLazy;
        public object ThreadSafeLazy
        {
            get
            {
                lock (threadSafeLazy)
                {
                    if (threadSafeLazy == null)
                    {
                        threadSafeLazy = new Object();
                    }
                }

                return threadSafeLazy;
            }
        }

        object optimizedThreadSafeLazy;
        public object OptimizedThreadSafeLazy
        {
            get
            {
                if (optimizedThreadSafeLazy == null)
                {
                    lock (optimizedThreadSafeLazy)
                    {
                        if (optimizedThreadSafeLazy == null)
                        {
                            optimizedThreadSafeLazy = new Object();
                        }
                    }
                }

                return optimizedThreadSafeLazy;
            }
        }

        volatile object optimizedAndSafeThreadSafeLazy;
        public object OptimizedAndSafeThreadSafeLazy
        {
            get
            {
                if (optimizedAndSafeThreadSafeLazy == null)
                {
                    lock (optimizedAndSafeThreadSafeLazy)
                    {
                        if (optimizedAndSafeThreadSafeLazy == null)
                        {
                            optimizedAndSafeThreadSafeLazy = new Object();
                        }
                    }
                }

                return optimizedAndSafeThreadSafeLazy;
            }
        }
    }
}
