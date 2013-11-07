using SamplesAPI;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace RxSamples
{
    public class FromEventSample : ISample
    {
        class Model : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            private string name;
            public string Name
            {
                get { return name; }
                set
                {
                    if (value != name)
                    {
                        name = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }

        public void Run()
        {
            Model model = new Model();

            IObservable<PropertyChangedEventArgs> propertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(h => model.PropertyChanged += h, h => model.PropertyChanged -= h)
                                                                              .Select(ep => ep.EventArgs);

            using (IDisposable subscription = propertyChanged.Subscribe(args => Console.WriteLine("Name changed to '{0}'.", model.Name)))
            {
                model.Name = "John";
                model.Name = "Bob";
            }

            model.Name = "Jack";
        }
    }
}
