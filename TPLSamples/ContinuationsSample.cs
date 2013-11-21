using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPLSamples
{
    public class ContinuationsSample : ISample
    {
        struct Pair
        {
            public decimal A;
            public decimal B;
        }

        public void Run()
        {
            Task<Pair> readInput = new Task<Pair>(() =>
                {
                    Pair pair = new Pair();

                    Console.Write("A? ");
                    pair.A = decimal.Parse(Console.ReadLine());

                    Console.Write("B? ");
                    pair.B = decimal.Parse(Console.ReadLine());

                    return pair;
                });

            Task<decimal> computeValue = new Task<decimal>(() =>
            {
                Pair input = readInput.Result;

                return input.A / input.B;
            });

            Task displayResult = new Task(() =>
                {
                    Console.WriteLine("Result: {0}", computeValue.Result);
                });

            Task handleError = new Task(() =>
                {
                    Console.WriteLine("Error:\n{0}", computeValue.Exception);
                });

            Task finallyHandler = new Task(() =>
                {
                    Console.WriteLine("Finally: {0}", !computeValue.IsFaulted ? "OK" : "KO");
                });

            readInput.ContinueWith(_ => computeValue.Start());

            computeValue.ContinueWith(_ => displayResult.Start(), TaskContinuationOptions.OnlyOnRanToCompletion);
            computeValue.ContinueWith(_ => handleError.Start(), TaskContinuationOptions.OnlyOnFaulted);
            computeValue.ContinueWith(_ => finallyHandler.Start());

            readInput.Start();

            Task.WaitAny(displayResult, handleError);
        }
    }
}
