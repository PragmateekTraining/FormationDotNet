﻿using SamplesAPI;
using System;
using System.Diagnostics;

namespace PerformanceCountersSamples
{
    public class SimpleCounterSample : ISample
    {
        public void Run()
        {
            const string categoryName = "Performance counters tests";
            const string counterName = "N";

            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                CounterCreationDataCollection counters = new CounterCreationDataCollection();
                counters.Add(new CounterCreationData(counterName, "Some human provided value.", PerformanceCounterType.NumberOfItems32));

                PerformanceCounterCategory.Create(categoryName, "Some fake counters for testing.", PerformanceCounterCategoryType.SingleInstance, counters);
            }

            PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, readOnly: false);

            while (true)
            {
                Console.Write("N? ");

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                int n = Convert.ToInt32(input);

                counter.RawValue = n;
            }
        }
    }
}
