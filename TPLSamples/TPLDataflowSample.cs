using SamplesAPI;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace TPLSamples
{
    public class TPLDataflowSample : ISample
    {
        public void Run()
        {
            // IPropagatorBlock<string, string> filter = new TransformManyBlock<string, string>(s => !string.IsNullOrWhiteSpace(s) ? new[] { s } : Enumerable.Empty<string>());
            IPropagatorBlock<string, string> filter = new TransformBlock<string, string>(s => s);
            ITargetBlock<string> sink = DataflowBlock.NullTarget<string>();
            IPropagatorBlock<string, Tuple<string, byte[]>> encode = new TransformBlock<string, Tuple<string, byte[]>>(s => Tuple.Create(s, Encoding.ASCII.GetBytes(s)));
            IPropagatorBlock<Tuple<string, byte[]>, Tuple<string, long>> sum = new TransformBlock<Tuple<string, byte[]>, Tuple<string, long>>(pair => Tuple.Create(pair.Item1, pair.Item2.Sum(b => (long)b)));
            ITargetBlock<Tuple<string, long>> print = new ActionBlock<Tuple<string, long>>(pair => Console.WriteLine("{0}: {1}", pair.Item1, pair.Item2));

            DataflowLinkOptions options = new DataflowLinkOptions { PropagateCompletion = true };

            filter.LinkTo(encode, options, s => !string.IsNullOrWhiteSpace(s));
            filter.LinkTo(sink, options, s => string.IsNullOrWhiteSpace(s));
            encode.LinkTo(sum, options);
            sum.LinkTo(print, options);

            filter.Completion.ContinueWith(t => Console.WriteLine("Filter completed."));
            encode.Completion.ContinueWith(t => Console.WriteLine("Encode completed."));
            sum.Completion.ContinueWith(t => Console.WriteLine("Sum completed."));
            print.Completion.ContinueWith(t => Console.WriteLine("Done!"));

            while (true)
            {
                Console.Write("? ");

                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) break;

                filter.Post(input);
            }

            filter.Complete();
            print.Completion.Wait();
        }
    }
}
