using System;
using SamplesAPI;

namespace DisposePatternSamples
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
                using (new Color(ConsoleColor.Green)) 
                    Console.WriteLine("EventListener received event.");
            }

            public EventListener(EventSource source)
            {
                source.Event += OnEvent;
            }

            ~EventListener()
            {
                using (new Color(ConsoleColor.Yellow))
                    Console.WriteLine("EventListener finalized.");
            }
        }

        public class DisposableEventListener : IDisposable
        {
            private readonly EventSource source;

            private void OnEvent(object source, EventArgs args)
            {
                using (new Color(ConsoleColor.Green))
                    Console.WriteLine("DisposableEventListener received event.");
            }

            public DisposableEventListener(EventSource source)
            {
                this.source = source;
                this.source.Event += OnEvent;
            }

            public void Dispose()
            {
                source.Event -= OnEvent;

                Console.WriteLine("DisposableEventListener disposed.");
            }

            ~DisposableEventListener()
            {
                using (new Color(ConsoleColor.Yellow))
                    Console.WriteLine("DisposableEventListener finalized.");
            }
        }

        public void Run()
        {
            EventSource source = new EventSource();

            EventListener listener = new EventListener(source);

            using (DisposableEventListener disposableListener = new DisposableEventListener(source))
            {
                source.Raise();

                listener = null;
            }

            using (new Color(ConsoleColor.Cyan)) Console.WriteLine("Starting GC.");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            using (new Color(ConsoleColor.Cyan)) Console.WriteLine("GC finished.");

            source.Raise();
        }
    }
}
