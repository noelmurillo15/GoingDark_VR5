using System;
using System.Collections;
using UnityEngine;

namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// TimerComponents are MonoBehaviours that simplifies Coroutine and provides an easy to use API to use timers and timed intervals.
    /// </summary>
    public abstract class TimerComponent : MonoBehaviour
    {
        public Action TimerMethod { get; set; } // Setup the TimerMethod, which will be called each interval for updates or when timer runs out for timers.
        public Action OnFinishedAction { get; set; } // OnFinishedAction will be called when TimedInvoke has run out.
        private IEnumerator _coroutine;

        /// <summary>
        /// Starts coroutine with a TimedInvoke of the specified timer.
        /// </summary>
        /// <param name="timer">The timer.</param>
        public void Invoke(float timer)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(TimedInvoke(timer));    
            }
            
        }

        /// <summary>
        /// Waits for specified time then invokes finished actions
        /// </summary>
        /// <param name="waitTime">The wait time.</param>
        /// <returns></returns>
        private IEnumerator TimedInvoke(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (TimerMethod != null)
            {
                TimerMethod();    
            }
            if (OnFinishedAction != null)
            {
                OnFinishedAction();
            }

        }

        /// <summary>
        /// Setups an infinite interval.
        /// </summary>
        /// <param name="secondsInterval">The interval in seconds.</param>
        public void SetupIntervalInfinite(float secondsInterval)
        {
            _coroutine = Interval(secondsInterval);
            StartInterval();
        }

        /// <summary>
        /// Setups an infinite interval.
        /// </summary>
        /// <param name="interval">The interval in seconds.</param>
        public void SetupIntervalInfinite(TimeSpan interval)
        {
            _coroutine = Interval(interval.TotalSeconds);
            StartInterval();
        }

        /// <summary>
        /// Setups a finite interval.
        /// </summary>
        /// <param name="interval">The interval in seconds.</param>
        /// <param name="numberOfIntervals">The number of intervals.</param>
        public void SetupIntervalFinite(TimeSpan interval, int numberOfIntervals)
        {
            _coroutine = Interval(interval.TotalSeconds, numberOfIntervals);
            StartInterval();
        }

        /// <summary>
        /// Starts the interval.
        /// </summary>
        public void StartInterval()
        {
            StartCoroutine(_coroutine);
        }

        /// <summary>
        /// Stops the interval.
        /// </summary>
        public void StopInterval()
        {
            StopCoroutine(_coroutine);
        }

        /// <summary>
        /// Intervals the specified interval.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <returns></returns>
        private IEnumerator Interval(double interval)
        {
            while (true)
            {
                TimerMethod();
                yield return new WaitForSeconds((float)interval);
            }            
        }

        /// <summary>
        /// Intervals the specified interval count times.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="count">The number of intervals.</param>
        /// <returns></returns>
        private IEnumerator Interval(double interval, int count)
        {
            while (count >= 0)
            {
                TimerMethod();
                count--;
                yield return new WaitForSeconds((float)interval);
            }
            if (OnFinishedAction != null)
            {
                OnFinishedAction();
            }
        }

        public void FinishTimer()
        {
            if (OnFinishedAction != null)
            {
                OnFinishedAction();
            }
        }
    }
}
