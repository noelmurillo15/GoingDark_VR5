using System;
using System.Timers;
using AGS.Core.Classes.ActionProperties;

namespace AGS.Core.Examples.ActionPropertiesExamples
{
    public class ActionListExampleModel
    {
        public ActionList<int> MySubscribableList;
        private static Timer _timer;

        public ActionListExampleModel()
        {
            MySubscribableList = new ActionList<int>();

            // Set up a time interval for calling OnTimedEvent every second
            _timer = new Timer(1000) { AutoReset = true };
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
            // Add DateTime.Now.Second to MySubscribableList
            MySubscribableList.Add(DateTime.Now.Second);
        }
    }
}
