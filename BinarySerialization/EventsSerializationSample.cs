using SamplesAPI;
using System;

namespace SerializationSamples
{
    public class EventsSerializationSample : ISample
    {
        [Serializable]
        class EventSource1
        {
            public event EventHandler Event = delegate { };
        }

        [Serializable]
        class EventSource2
        {
            [field: NonSerialized]
            public event EventHandler Event = delegate { };
        }

        class Listener
        {
            public void OnEvent(object sender, EventArgs args)
            {
            }
        }

        public void Run()
        {
            Listener listener = new Listener();

            EventSource1 eventSource1 = new EventSource1();
            eventSource1.Event += listener.OnEvent;

            EventSource2 eventSource2 = new EventSource2();
            eventSource2.Event += listener.OnEvent;

            try
            {
                Console.WriteLine("Trying to serialize event source 1.");

                // Will crash because the Listener class is not serializable
                eventSource1.ToNetBinary();

                using (Color.Green) Console.WriteLine("Event source 1 serialized.");
            }
            catch (Exception e)
            {
                using (Color.Red) Console.WriteLine("Caught exception:\n{0}.", e);
            }

            Console.WriteLine("\n==========\n");

            try
            {
                Console.WriteLine("Trying to serialize event source 2.");

                eventSource2.ToNetBinary();

                using (Color.Green) Console.WriteLine("Event source 2 serialized.");
            }
            catch (Exception e)
            {
                using (Color.Red) Console.WriteLine("Caught exception:\n{0}.", e);
            }
        }
    }
}
