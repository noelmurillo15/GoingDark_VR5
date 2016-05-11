using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// Base class for all types of environment hazards, death traps, area hazard and fluids.
    /// </summary>
    public class HazardBase : ActionModel
    {
        #region Properties
        // Constructor properties
        public float SecondsActive { get; private set; }
        public float SecondsRecharging { get; private set; }
        public bool DeactivateOnTrigger { get; private set; } // Should the hazard fall into inactive mode after its triggered once?
        public bool DestroyOnTrigger { get; private set; } // Should the hazard destroy itself after its triggered once?

        // Subscribable properties
        public ActionProperty<HazardState> HazardCurrentState { get; private set; } // The state of the hazard.

        // For notifying subscribers that the hazard was triggered
        public Action HitTriggerAction { get; set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HazardBase"/> class.
        /// </summary>
        /// <param name="secondsActive">The seconds active duration of this hazard.</param>
        /// <param name="secondsRecharging">The seconds rechargingd uration of this hazard.</param>
        /// <param name="deactivateOnTrigger">if set to <c>true</c> [deactivate on trigger].</param>
        /// <param name="destroyOnTrigger">if set to <c>true</c> [destroy on trigger].</param>
        public HazardBase(float secondsActive, float secondsRecharging, bool deactivateOnTrigger, bool destroyOnTrigger)
        {
            SecondsActive = secondsActive;
            SecondsRecharging = secondsRecharging;
            DeactivateOnTrigger = deactivateOnTrigger;
            DestroyOnTrigger = destroyOnTrigger;
            HazardCurrentState = new ActionProperty<HazardState>();
        }

        #region public functions
        /// <summary>
        /// Triggers the hazard.
        /// </summary>
        public virtual void TriggerHazard()
        {
            if (HitTriggerAction != null)
            {
                HitTriggerAction();    
            }
            
        }
        #endregion

        #region TransitionToStates
        public virtual void TransitionToStateActivate()
        {
            HazardCurrentState.Value = HazardState.Active;
        }

        public virtual void TransitionToStateDeactivate()
        {
            HazardCurrentState.Value = HazardState.Inactive;
        }

        public virtual void TransitionToStateRecharge()
        {
            if (SecondsRecharging <= 0f)
            {
                UnityEngine.Debug.LogError("Set recharge timer before using hazard recharge");
                return;
            }
            HazardCurrentState.Value = HazardState.Recharging;
        }
        #endregion
    }
}
