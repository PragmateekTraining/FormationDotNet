using SamplesAPI;
using System;
using System.Collections.Generic;

namespace ValueTypesSamples
{
    public class DictionaryKeySample : ISample
    {
        public void Run()
        {
            IDictionary<ReportID, string> reports = new Dictionary<ReportID, string>();

            string message = "Sir, you've failed.\nPlease die.";

            reports[new ReportID("John Doe", 42)] = message;

            Console.WriteLine(reports[new ReportID("John Doe", 42)]);
        }
    }
}
