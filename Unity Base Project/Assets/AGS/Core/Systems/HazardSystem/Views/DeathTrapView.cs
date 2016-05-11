using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// DeathTraps are hazard that instantly kills a Killable if a collision is detected. It also detects collision with ragdolls and movable objects
    /// which enables for example manually destroying it by throwing a crate on it (if it is set to destroy on trigger)
    /// </summary>
    [Serializable]
    public class DeathTrapView : HazardBaseView
    {
        public DeathTrap DeathTrap;

        #region AGS Setup
        public override void InitializeView()
        {
            DeathTrap = new DeathTrap(SecondsActive, SecondsRecharging, DeactivateOnTrigger, DestroyOnTrigger);
            SolveModelDependencies(DeathTrap);
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [state enter active].
        /// </summary>
        public override void OnStateEnterActive()
        {
            // if SecondsActive = 0 we do not time toggle to deactivated
            if (HazardBase.SecondsActive > 0)
            {
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "State count down timer");
                timerComponent.TimerMethod = () => HazardBase.TransitionToStateRecharge();
                timerComponent.Invoke(HazardBase.SecondsActive);
            }
        }

        /// <summary>
        /// Called when [state enter recharging].
        /// </summary>
        public override void OnStateEnterRecharging()
        {
            // Start count down for transitioning back to active
            var timedInvoke = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "State count down timer");
            timedInvoke.TimerMethod = () => HazardBase.TransitionToStateActivate();
            timedInvoke.Invoke(HazardBase.SecondsRecharging);
        }

        /// <summary>
        /// Called when [state enter inactive].
        /// </summary>
        public override void OnStateEnterInactive()
        {
            // Do nothing
        }
        #endregion

        #region collision detection implementations
        /// <summary>
        /// Called when [collision killable notification].
        /// </summary>
        /// <param name="killableView">The killable view.</param>
        protected override void OnCollisionKillableNotification(KillableBaseView killableView)
        {
            if (killableView.Killable == null) return;

            if (DeathTrap.HazardCurrentState.Value == HazardState.Active)
            {
                DeathTrap.TriggerHazard();
                killableView.Killable.InstantDeath();
                if (DeathTrap.DestroyOnTrigger && HazardEffects == null)
                {
                    Destroy(gameObject);
                }
                if (DeathTrap.DeactivateOnTrigger)
                {
                    DeathTrap.TransitionToStateDeactivate();
                }
                else if (DeathTrap.SecondsRecharging > 0f)
                {
                    DeathTrap.TransitionToStateRecharge();
                }
            }

        }

        /// <summary>
        /// Called when [collision movable notification].
        /// </summary>
        /// <param name="movableObjectBaseView">The movable object base view.</param>
        protected override void OnCollisionMovableNotification(MovableObjectBaseView movableObjectBaseView)
        {
            if (movableObjectBaseView.MovableObject == null) return;

            if (DeathTrap.HazardCurrentState.Value == HazardState.Active)
            {
                DeathTrap.TriggerHazard();
                if (DeathTrap.DestroyOnTrigger && HazardEffects == null)
                {
                    Destroy(gameObject);
                }
                else if (DeathTrap.DeactivateOnTrigger)
                {
                    DeathTrap.TransitionToStateDeactivate();
                }
                else if (DeathTrap.SecondsRecharging > 0f)
                {
                    DeathTrap.TransitionToStateRecharge();
                }
            }
        }

        /// <summary>
        /// Called when [collision ragdoll notification].
        /// </summary>
        /// <param name="ragdollBody">The ragdoll body.</param>
        protected override void OnCollisionRagdollNotification(MonoBehaviour ragdollBody)
        {
            if (DeathTrap.HazardCurrentState.Value == HazardState.Active)
            {
                DeathTrap.TriggerHazard();
                if (DeathTrap.DestroyOnTrigger && HazardEffects == null)
                {
                    Destroy(gameObject);
                }
                else if (DeathTrap.DeactivateOnTrigger)
                {
                    DeathTrap.TransitionToStateDeactivate();
                }
                else if (DeathTrap.SecondsRecharging > 0f)
                {
                    DeathTrap.TransitionToStateRecharge();
                }
            }
        }

        #endregion
    }
}