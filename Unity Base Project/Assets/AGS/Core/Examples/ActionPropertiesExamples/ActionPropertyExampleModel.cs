using System;
using System.Timers;
using AGS.Core.Classes.ActionProperties;

namespace AGS.Core.Examples.ActionPropertiesExamples
{
    public class ActionPropertyExampleModel
    {
        public ActionProperty<int> MySubscribableInt { get; private set; }
        private static Timer _timer;

        public ActionPropertyExampleModel()
        {
            MySubscribableInt = new ActionProperty<int>();
            
            // Set up a time interval for calling OnTimedEvent every second
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += OnTimedEvent;
        }


        public void StartTimer()
        {
            _timer.Start();
        }
        public void StopTimer()
        {

            _timer.Stop();
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // Increment MySubscribableInt
            MySubscribableInt.Value++;
        }
    }
}
