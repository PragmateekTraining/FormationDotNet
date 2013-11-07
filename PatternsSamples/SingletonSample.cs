using SamplesAPI;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PatternsSamples
{
    public class SingletonSample : ISample
    {
        public sealed class LockedSingleton
        {
            private static LockedSingleton instance = null;

            private static object @lock = new object();

            /// <summary>
            /// Prevent instantiation of the class.
            /// </summary>
            private LockedSingleton()
            {
            }

            public static LockedSingleton Instance
            {
                get
                {
                    lock (@lock)
                    {
                        if (instance == null)
                        {
                            instance = new LockedSingleton();
                        }

                        return instance;
                    }
                }
            }
        }

        public sealed class DoubleCheckSingleton
        {
            private static DoubleCheckSingleton instance = null;

            private static object @lock = new object();

            /// <summary>
            /// Prevent instantiation of the class.
            /// </summary>
            private DoubleCheckSingleton()
            {
            }

            public static DoubleCheckSingleton Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (@lock)
                        {
                            if (instance == null)
                            {
                                instance = new DoubleCheckSingleton();
                            }
                        }
                    }

                    return instance;
                }
            }
        }

        public sealed class ReliableDoubleCheckSingleton
        {
            private static ReliableDoubleCheckSingleton instance = null;

            private static object @lock = new object();

            /// <summary>
            /// Prevent instantiation of the class.
            /// </summary>
            private ReliableDoubleCheckSingleton()
            {
            }

            public static ReliableDoubleCheckSingleton Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (@lock)
                        {
                            if (instance == null)
                            {
                                ReliableDoubleCheckSingleton tmp = new ReliableDoubleCheckSingleton();

                                Interlocked.Exchange(ref instance, tmp);
                            }
                        }
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// Marking the class as "sealed" is not necessary because the private constructor will prevent any sub-classing.
        /// But adding "sealed" add some semantics and is more user-friendly in case of a sub-classing attempt:
        /// - first intellisense will not color "Singleton" in "class MySingletonSubclass : Singleton"
        /// - the CSC compiler displays a clearer message : "cannot derive from Singleton" instead of "Singleton() is inaccessible"
        /// </summary>
        public sealed class Singleton
        {
            /// <summary>
            /// This statement is guaranteed to be executed once per app-domain, the CLR taking care of its thread-safety.
            /// </summary>
            private static readonly Singleton instance = new Singleton();

            /// <summary>
            /// Prevent instantiation of the class.
            /// </summary>
            private Singleton()
            {
            }

            public static Singleton Instance
            {
                get
                {
                    return instance;
                }
            }
        }

        /// <summary>
        /// Same as above but this version is explicitly lazy.
        /// </summary>
        public sealed class LazySingleton
        {
            /// <summary>
            /// Could be "new Lazy&lt;Singleton>()" but for performance reason it's better to specify a factory.
            /// Indeed otherwise "Lazy" will use reflection: Activator.CreateInstance, which is heavier.
            /// </summary>
            private static readonly Lazy<LazySingleton> lazy = new Lazy<LazySingleton>(() => new LazySingleton());

            public static LazySingleton Instance
            {
                get
                {
                    return lazy.Value;
                }
            }

            private LazySingleton()
            {
            }
        }

        public void Run()
        {
        }
    }
}
