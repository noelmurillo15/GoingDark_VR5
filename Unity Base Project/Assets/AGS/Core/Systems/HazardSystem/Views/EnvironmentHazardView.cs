using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using AGS.Core.Systems.RagdollSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// This view should be placed on single targets hazard in the scene.
    /// </summary>
    [Serializable]
    public class EnvironmentHazardView : HazardBaseView
    {
        #region Public properties
        // references to be set in the editor
        public StatusEffectComboView StatusEffectComboView;
        public ExplosionEffectBaseView ExplosionEffectView;
        #endregion

        public EnvironmentHazard EnvironmentHazard;

        #region AGS Setup
        public override void InitializeView()
        {
            EnvironmentHazard = new EnvironmentHazard(SecondsActive, SecondsRecharging, DeactivateOnTrigger, DestroyOnTrigger,  transform);
            SolveModelDependencies(EnvironmentHazard);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (StatusEffectComboView != null)
            {
                EnvironmentHazard.EffectsCombo.Value = StatusEffectComboView.StatusEffectCombo;
            }
            if (ExplosionEffectView != null)
            {
                EnvironmentHazard.ExplosionEffect.Value = ExplosionEffectView.ExplosionEffect;
            }
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

            if (EnvironmentHazard.HazardCurrentState.Value == HazardState.Active)
            {
                
                EnvironmentHazard.HitKillable(killableView.Killable);
                if (EnvironmentHazard.SecondsRecharging > 0f)
                {
                    EnvironmentHazard.TransitionToStateRecharge();
                }
                else if (EnvironmentHazard.DestroyOnTrigger && HazardEffects == null)
                {
                    Destroy(gameObject);
                }
                else if (EnvironmentHazard.DeactivateOnTrigger)
                {
                    EnvironmentHazard.TransitionToStateDeactivate();
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

            if (EnvironmentHazard.HazardCurrentState.Value == HazardState.Active)
            {

                EnvironmentHazard.HitMovable(movableObjectBaseView.MovableObject);
                if (EnvironmentHazard.SecondsRecharging > 0f)
                {
                    EnvironmentHazard.TransitionToStateRecharge();
                }
                else if (EnvironmentHazard.DestroyOnTrigger && HazardEffects == null)
                {
                    Destroy(gameObject);
                }
                else if (EnvironmentHazard.DeactivateOnTrigger)
                {
                    EnvironmentHazard.TransitionToStateDeactivate();
                }
            } 
        }

        /// <summary>
        /// Called when [collision ragdoll notification].
        /// </summary>
        /// <param name="ragdollBody">The ragdoll body.</param>
        protected override void OnCollisionRagdollNotification(MonoBehaviour ragdollBody)
        {
            var ragdollView = ragdollBody.GetComponentInParent<RagdollView>();
            if (ragdollView.Ragdoll == null) return;

            if (EnvironmentHazard.HazardCurrentState.Value == HazardState.Active)
            {
                EnvironmentHazard.HitMovable(ragdollView.Ragdoll);
                if (EnvironmentHazard.SecondsRecharging > 0f)
                {
                    EnvironmentHazard.TransitionToStateRecharge();
                }
                else if (EnvironmentHazard.DestroyOnTrigger && HazardEffects == null)
                {
                    Destroy(gameObject);
                }
                else if (EnvironmentHazard.DeactivateOnTrigger)
                {
                    EnvironmentHazard.TransitionToStateDeactivate();
                }
            }
        }
        #endregion
    }
}