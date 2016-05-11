using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.TimerComponents;

namespace AGS.Core.Systems.BaseSystem
{
    /// <summary>
    /// Base class for AGS ActionModel. It holds lists of any timer components that has been added to the Model, and destroys them when Model is destroyed.
    /// </summary>
    public abstract class ActionModel
    {
        /// <summary>
        /// Subscribe to this Action to get notified of when the ActionModel is destroyed
        /// </summary>
        /// <value>
        /// The model destroyed.
        /// </value>
        public Action ModelDestroyed { get; set; }

        /// <summary>
        /// The ActionModels timer components. Add any created TimerComponents to this ActionList
        /// </summary>
        protected ActionList<TimerComponent> TimerComponents = new ActionList<TimerComponent>();
        /// <summary>
        /// The ActionModels update components. Add any created UpdateComponents to this ActionList
        /// </summary>
        protected ActionList<UpdateComponent> UpdateComponents = new ActionList<UpdateComponent>();

        /// <summary>
        /// Destroys the model and stops any active timers
        /// </summary>
        public virtual void DestroyModel()
        {
            foreach (var timer in TimerComponents)
            {
                if (timer != null)
                {
                    timer.FinishTimer();    
                }
                
            }
            foreach (var update in UpdateComponents)
            {
                if (update != null)
                {
                    update.Stop();
                }
            }
            if (ModelDestroyed != null)
            {
                ModelDestroyed();    
            }
        }
    }
}
