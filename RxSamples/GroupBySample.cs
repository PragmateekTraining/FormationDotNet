using SamplesAPI;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxSamples
{
    public class GroupBySamples : ISample
    {
        class HREvent
        {
            public string Department { get; set; }
            public string Employee { get; set; }
            public bool IsHired { get; set; }
        }

        public void Run()
        {
            ReplaySubject<HREvent> HREvents = new ReplaySubject<HREvent>();

            HREvents.Subscribe(e => Console.WriteLine("New event: {0} department has {1} {2}", e.Department, e.IsHired ? "hired" : "fired", e.Employee));

            HREvents.GroupBy(e => e.Department)
                    .Subscribe(g =>
                    {
                        using (Color.Blue)
                        Console.WriteLine("New department: {0}.", g.Key);

                        if (g.Key.Split('/')[0] == "IT")
                        {
                            g.Subscribe(e =>
                                {
                                    using (e.IsHired ? Color.Green : Color.Red)
                                        Console.WriteLine("{0} {1}!", e.IsHired ? "Welcome" : "Goodbye", e.Employee);
                                });
                        }
                    });

            HREvents.OnNext(new HREvent { Department = "HR", IsHired = true, Employee = "Melany" });
            HREvents.OnNext(new HREvent { Department = "IT/System", IsHired = true, Employee = "Sponge Bob" });
            HREvents.OnNext(new HREvent { Department = "IT/Dev", IsHired = true, Employee = "John Doe" });
            HREvents.OnNext(new HREvent { Department = "HR", IsHired = true, Employee = "Roger" });
            HREvents.OnNext(new HREvent { Department = "IT/Dev", IsHired = true, Employee = "Paul" });
            HREvents.OnNext(new HREvent { Department = "IT/Dev", IsHired = false, Employee = "Sponge Bob" });
        }
    }
}
