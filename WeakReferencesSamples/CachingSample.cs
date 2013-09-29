using System;
using System.Collections.Generic;
using SamplesAPI;

namespace WeakReferencesSamples
{
    public class CachingSample : ISample
    {
        class StrongCache
        {
            private IDictionary<string, A> cache = new Dictionary<string, A>();

            public A Get(string s)
            {
                if (!cache.ContainsKey(s))
                {
                    cache[s] = new A(s);
                }

                return cache[s];
            }
        }

        class WeakCache
        {
            private IDictionary<string, WeakReference<A>> cache = new Dictionary<string, WeakReference<A>>();

            public A Get(string s)
            {
                if (!cache.ContainsKey(s))
                {
                    cache[s] = new WeakReference<A>(new A(s));
                }

                return cache[s].Target;
            }
        }

        public void Run()
        {
            StrongCache strongCache = new StrongCache();
            WeakCache weakCache = new WeakCache();

            A sa1 = strongCache.Get("strong 1");
            A sa2 = strongCache.Get("strong 2");
            A sa3 = strongCache.Get("strong 3");

            A wa1 = weakCache.Get("weak 1");
            A wa2 = weakCache.Get("weak 2");
            A wa3 = weakCache.Get("weak 3");

            sa1 = null;
            sa2 = null;
            sa3 = null;

            wa1 = null;
            wa2 = null;
            wa3 = null;

            Console.WriteLine("Starting GC.");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("GC finished.");

            // Simulate usage of caches to avoid any optimization
            strongCache.ToString();
            weakCache.ToString();
        }
    }
}
