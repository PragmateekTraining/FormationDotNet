using SamplesAPI;
using System;
using System.Globalization;

namespace CultureSamples
{
    public class NumbersInconsistentCulturesSample : ISample
    {
        public void Run()
        {
            double number = 123.456;

            string str;
            string strInvariant;
            
            using (new Culture("fr-FR"))
            {
                str = number.ToString();
                strInvariant = number.ToString(CultureInfo.InvariantCulture);
            }

            double outputNumber;
            double outputNumberInvariant;

            using (new Culture("en-US"))
            {
                outputNumber = double.Parse(str);
                outputNumberInvariant = double.Parse(strInvariant, CultureInfo.InvariantCulture);
            }

            using (new Culture("fr-FR"))
            {
                string format = "| {0,-10}| {1,-8}| {2,-8}| {3,-8}| {4,-7}|";

                Console.WriteLine(format, "Culture", "Input", "Text", "Output", "Check");
                Console.WriteLine(format, "None", number, str, outputNumber, number == outputNumber ? "OK" : "KO");
                Console.WriteLine(format, "Invariant", number, strInvariant, outputNumberInvariant, number == outputNumberInvariant ? "OK" : "KO");
            }
        }
    }
}