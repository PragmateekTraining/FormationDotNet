using System;
using SamplesAPI;
using System.Windows;

namespace WeakReferencesSamples
{
    public class EventHandlerSample : ISample
    {
        public class EventSource
        {
            public event EventHandler<EventArgs> Event = delegate { };

            public void Raise()
            {
                Event(this, EventArgs.Empty);
            }
        }

        public class EventListener
        {
            private void OnEvent(object source, EventArgs args)
            {
                Console.WriteLine("EventListener received event.");
            }

            public EventListener(EventSource source)
            {
                source.Event += OnEvent;
            }

            ~EventListener()
            {
                Console.WriteLine("EventListener finalized.");
            }
        }

        public class WeakEventListener
        {
            private void OnEvent(object source, EventArgs args)
            {
                Console.WriteLine("WeakEventListener received event.");
            }

            public WeakEventListener(EventSource source)
            {
                WeakEventManager<EventSource, EventArgs>.AddHandler(source, "Event", OnEvent);
            }

            ~WeakEventListener()
            {
                Console.WriteLine("WeakEventListener finalized.");
            }
        }

        public void Run()
        {
            EventSource source = new EventSource();

            EventListener listener = new EventListener(source);

            WeakEventListener weakListener = new WeakEventListener(source);

            source.Raise();

            listener = null;
            weakListener = null;

            Console.WriteLine("Starting GC.");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("GC finished.");

            source.Raise();
        }
    }
}
