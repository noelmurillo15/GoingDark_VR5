using System;
using UnityEngine;

namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// UpdateComponents should be used for temporary updates. Useful for scenarios where it is not desirable to continue updating something when it is only needed for a short time.
    /// UpdateComponents will continue updating until Stop is called, which then will run the OnFinishedAction
    /// </summary>
    public abstract class UpdateComponentBase : MonoBehaviour
    {
        public Action UpdateMethod;
        public Action OnFinishedAction { get; set; }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (OnFinishedAction != null)
            {
                OnFinishedAction();
            }
        }
    }
}
