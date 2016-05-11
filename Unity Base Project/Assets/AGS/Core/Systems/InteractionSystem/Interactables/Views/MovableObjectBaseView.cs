using System;
using AGS.Core.Classes.Helpers;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// BaseView for movables. Requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// States need specific implementations due to the different components needed for moving objects in different games
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class MovableObjectBaseView : ActionView
    {
        #region Public properties
        // fields to be set in the editor
        public float SecondsReInteract;
        public MovableObjectWeight MovableObjectWeight;
        #endregion

        public MovableObject MovableObject;
        private Vector3 _pushForceToAdd;
        private Animator _animator;

        #region AGS Setup
        public override void InitializeView()
        {
            MovableObject = new MovableObject(transform, MovableObjectWeight, SecondsReInteract);
            SolveModelDependencies(MovableObject);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            // Subscribes to active state
            MovableObject.MovableObjectCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            // Subscribes to push effect action
            MovableObject.PushEffectAction += HandlePushEffects;
            MovableObject.IsStillFunc += IsStill;
        }
        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region private functions

        /// <summary>
        /// Checks if movable is still.
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsStill();

        /// <summary>
        /// Handles the push effects.
        /// Checks if push effect has additional ticks.
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> [hit from behind].</param>
        private void HandlePushEffects(PushEffect pushEffect, bool hitFromBehind)
        {
            if (pushEffect.Ticks > 0)
            {
                // sets up an interval timer for ticking push effects
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Push effect interval");
                timerComponent.TimerMethod = () => ApplyPushEffect(pushEffect, hitFromBehind);
                timerComponent.SetupIntervalFinite(TimeSpan.FromSeconds(pushEffect.SecondsBetweenTicks), pushEffect.Ticks);
            }
            else
            {
                ApplyPushEffect(pushEffect, hitFromBehind);
            }
        }
        
        /// <summary>
        /// Finilizes the pusheffects with correct values depening on situation, and applies the actual push effect.
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> [hit from behind].</param>
        protected void ApplyPushEffect(PushEffect pushEffect, bool hitFromBehind)
        {
            switch (pushEffect.Direction)
            {
                case VectorDirection.Forward:
                    _pushForceToAdd = hitFromBehind ? -transform.forward * pushEffect.Strength
                                                       : transform.forward * pushEffect.Strength;
                    break;
                case VectorDirection.Back:
                    _pushForceToAdd = hitFromBehind ? transform.forward * pushEffect.Strength
                                                       : -transform.forward * pushEffect.Strength;
                    break;
                case VectorDirection.Up:
                    _pushForceToAdd = Vector3.up * pushEffect.Strength;
                    break;
                case VectorDirection.Down:
                    _pushForceToAdd = -Vector3.up * pushEffect.Strength;
                    break;
                default:
                    _pushForceToAdd = Vector3.zero;
                    break;
            }

            TriggerPushEffect(_pushForceToAdd, ForceConverter.ForceTypeToUnityForceMode(pushEffect.ForceType));
        }
        #endregion
        #region abstract functions
        /// <summary>
        /// Triggers the push effect.
        /// </summary>
        /// <param name="pushForceToAdd">The push force to add.</param>
        /// <param name="value">The value.</param>
        protected abstract void TriggerPushEffect(Vector3 pushForceToAdd, ForceMode value);

        /// <summary>
        /// Determines whether this movable object is connected to a character.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsConnected();        
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(MovableObjectState currentState)
        {
            /* MovableObjectStates 
             * Idle == 0
             * Grabbed == 1
             * PickedUp == 2
             * Carried == 3
             * Thrown == 4 */
            _animator.SetInteger("MovableObjectState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public abstract void OnStateEnterIdle();

        /// <summary>
        /// Called when [state enter grabbed].
        /// </summary>
        public abstract void OnStateEnterGrabbed();

        /// <summary>
        /// Called when [state update grabbed].
        /// </summary>
        public abstract void OnStateUpdateGrabbed();

        /// <summary>
        /// Called when [state enter picked up].
        /// </summary>
        public abstract void OnStateEnterPickedUp();

        /// <summary>
        /// Called when [state update picked up].
        /// </summary>
        public abstract void OnStateUpdatePickedUp();

        /// <summary>
        /// Called when [state enter carried].
        /// </summary>
        public abstract void OnStateEnterCarried();

        /// <summary>
        /// Called when [state update carried].
        /// </summary>
        public abstract void OnStateUpdateCarried();

        /// <summary>
        /// Called when [state enter thrown].
        /// </summary>
        public abstract void OnStateEnterThrown();
        #endregion
    }
}