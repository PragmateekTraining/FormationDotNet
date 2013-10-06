using SamplesAPI;
using System;
using System.Globalization;

namespace CultureSamples
{
    public class InconsistentCulturesSample : ISample
    {
        public void Run()
        {
            DateTime input20131012 = new DateTime(2013, 10, 12);

            string str20131012;
            string str20131012Invariant;
            
            using (new Culture("fr-FR"))
            {
                str20131012 = input20131012.ToString();
                str20131012Invariant = input20131012.ToString(CultureInfo.InvariantCulture);
            }

            DateTime output20131012;
            DateTime output20131012Invariant;

            using (new Culture("en-US"))
            {
                output20131012 = DateTime.Parse(str20131012);
                output20131012Invariant = DateTime.Parse(str20131012Invariant, CultureInfo.InvariantCulture);
            }

            using (new Culture("fr-FR"))
            {
                string format = "| {0,-10}| {1,-12}| {2,-20}| {3,-12}| {4,-7}|";

                Console.WriteLine(format, "Culture", "Input", "Text", "Output", "Check");
                Console.WriteLine(format, "None", input20131012.ToShortDateString(), str20131012, output20131012.ToShortDateString(), input20131012 == output20131012 ? "OK" : "KO");
                Console.WriteLine(format, "Invariant", input20131012.ToShortDateString(), str20131012Invariant, output20131012Invariant.ToShortDateString(), input20131012 == output20131012Invariant ? "OK" : "KO");
            }
        }
    }
}