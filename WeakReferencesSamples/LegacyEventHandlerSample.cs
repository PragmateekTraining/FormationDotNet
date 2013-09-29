using System;
using SamplesAPI;
using System.Windows;

namespace WeakReferencesSamples
{
    public class LegacyEventHandlerSample : ISample
    {
        public class EventManager : WeakEventManager
        {
            private EventManager()
            {
            }

            private static EventManager manager = null;

            static EventManager()
            {
                manager = new EventManager();

                WeakEventManager.SetCurrentManager(typeof(EventManager), manager);
            }

            public static EventManager CurrentManager
            {
                get
                {
                    return WeakEventManager.GetCurrentManager(typeof(EventManager)) as EventManager;
                }
            }

            public static void AddListener(EventSource source, IWeakEventListener listener)
            {
                CurrentManager.ProtectedAddListener(source, listener);
            }

            public static void RemoveListener(EventSource source, IWeakEventListener listener)
            {
                CurrentManager.ProtectedRemoveListener(source, listener);
            }

            protected override void StartListening(object source)
            {
                ((EventSource)source).Event += DeliverEvent;
            }

            protected override void StopListening(object source)
            {
                ((EventSource)source).Event -= DeliverEvent;
            }
        }

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

        public class WeakEventListener : IWeakEventListener
        {
            private void OnEvent(object source, EventArgs args)
            {
                Console.WriteLine("WeakEventListener received event.");
            }

            public WeakEventListener(EventSource source)
            {
                EventManager.AddListener(source, this);
            }

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
            {
                OnEvent(sender, e);

                return true;
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
