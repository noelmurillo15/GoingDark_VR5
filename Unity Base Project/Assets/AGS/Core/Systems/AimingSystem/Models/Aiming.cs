using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.MovingEnvironmentSystem;
using UnityEngine;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.AimingSystem
{
    /// <summary>
    /// Aiming can be used in two different ways. Free aiming/strafing (shooter like) and locked on Z-targeting (rpg like).
    /// </summary>
    public class Aiming : ActionModel
    {

        #region Properties
        // Constructor properties


        // Subscribable properties
        public ActionProperty<CombatEntityBase> OwnerCombatEntity { get; protected set; } // Reference to the owner CombatEntity
        public ActionProperty<Vector3> AimTarget { get; private set; } // The current target point
        public ActionProperty<Vector3> AimDirection { get; private set; } // The current aiming direction
        public ActionProperty<bool> OnTarget { get; private set; } // Is the AimTarget point on a valid target?
        public ActionProperty<AimingStateMachineState> AimingCurrentState { get; private set; } // The aiming state. Idle, Aiming (free aiming) and LockedOnTarget
        public ActionProperty<AimingStateIntention> Intention { get; private set; } // The current aiming intention
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Aiming"/> class.
        /// </summary>
        public Aiming()
        {
            OwnerCombatEntity = new ActionProperty<CombatEntityBase>();
            AimTarget = new ActionProperty<Vector3>();
            AimDirection = new ActionProperty<Vector3>();
            OnTarget = new ActionProperty<bool>();
            AimingCurrentState = new ActionProperty<AimingStateMachineState>();
            Intention = new ActionProperty<AimingStateIntention>();
            Intention.OnValueChanged += (sender, aimingState) => SetAimingState(aimingState.Value);
        }

        #region private functions
        /// <summary>
        /// Sets the state of the aiming.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetAimingState(AimingStateIntention intention)
        {
            switch (intention)
            {
                case AimingStateIntention.None:
                    TransitionToStateIdle();
                    break;
                case AimingStateIntention.Aim:
                    TransitionToStateAiming();
                    break;
                case AimingStateIntention.LockOnTarget:
                    TransitionToStateLockedOnTarget();
                    break;
            }
        }

        #endregion

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            AimingCurrentState.Value = AimingStateMachineState.Idle;
        }

        /// <summary>
        /// Transitions to state aiming.
        /// </summary>
        public void TransitionToStateAiming()
        {
            AimingCurrentState.Value = AimingStateMachineState.Aiming;
        }

        /// <summary>
        /// Transitions to state locked on target.
        /// </summary>
        public void TransitionToStateLockedOnTarget()
        {
            AimingCurrentState.Value = AimingStateMachineState.LockedOnTarget;
        }
        #endregion
    }
}
