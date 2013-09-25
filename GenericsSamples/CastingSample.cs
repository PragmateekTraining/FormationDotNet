﻿using System;
using System.Collections.Generic;
using SamplesAPI;
using System.Collections;
using System.Diagnostics;

namespace GenericsSamples
{
    public class CastingSample : ISample
    {
        public void Run()
        {
            const int N = 1000000;

            ArrayList arrayList = new ArrayList();
            List<string> list = new List<string>();

            for (int i = 0; i < N; ++i)
            {
                string guid = Guid.NewGuid().ToString();

                arrayList.Add(guid);
                list.Add(guid);
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (string guid in arrayList)
            {
            }
            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;

            stopwatch.Restart();
            foreach (string guid in list)
            {
            }
            stopwatch.Stop();
            TimeSpan t2 = stopwatch.Elapsed;

            double ratio = 1.0 * t1.Ticks / t2.Ticks;

            Console.WriteLine("With generics: {0}", t2);
            Console.WriteLine("Without generics: {0} (x{1:N1})", t1, ratio);
        }
    }
}
