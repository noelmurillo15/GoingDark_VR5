using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.MovementSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.Swimming
{
    /// <summary>
    /// SwimmingBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class SwimmingBaseView : MovementSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float SwimSpeed;
        public float SurfaceJumpSpeedUpwards;
        public float SurfaceJumpSpeedForward;
        public float SecondsBetweenStrokes;
        #endregion

        public Swimming Swimming;
        private UpdatePersistantGameObject _swimmingIntentionUpdater; // For use when updating the characters intention        
        private Animator _animator;
        private UpdateTemporaryGameObject _restoreAirUpdater;
        private TimerTemporaryGameObject _justSplashedTimer;
        private TimerTemporaryGameObject _strokeCountDownTimer;
        private TimerTemporaryGameObject _surfaceJumpTimer;

        /// <summary>
        /// Convenience property. Gets the character air supply.
        /// </summary>
        /// <value>
        /// The character air supply.
        /// </value>
        public DamageableResource CharacterAirSupply
        {
            get
            {
                if (_characterAirSupply != null) return _characterAirSupply;
                if (Swimming.OwnerMovementSkills.Value != null
                    &&
                    Swimming.OwnerMovementSkills.Value.OwnerCharacter.Value != null
                    &&
                    Swimming.OwnerMovementSkills.Value.OwnerCharacter.Value.Resources != null)
                {
                    _characterAirSupply = Swimming.OwnerMovementSkills.Value.OwnerCharacter.Value.Resources.FirstOrDefault(
                        x => x.ResourceType.Value == DamageableResourceType.Air);
                }
                return _characterAirSupply;
            }
        }
        private DamageableResource _characterAirSupply;

        /// <summary>
        /// Convenience property. Gets a value indicating whether [no movement input].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no movement input]; otherwise, <c>false</c>.
        /// </value>
        protected bool NoMovementInput
        {
            get
            {
                return (Mathf.Abs(Swimming.OwnerMovementSkills.Value.OwnerCharacter.Value.CharacterController.Value.MoveVector.Value.y) < 0.01f
                        && Mathf.Abs(Swimming.OwnerMovementSkills.Value.OwnerCharacter.Value.CharacterController.Value.MoveVector.Value.x) < 0.01f);
            }
        }

        #region AGS Setup

        public override void InitializeView()
        {
            Swimming = new Swimming(SwimSpeed, SurfaceJumpSpeedUpwards, SurfaceJumpSpeedForward, SecondsBetweenStrokes);
            SolveModelDependencies(Swimming);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Swimming.ResourceCost.Value = ResourceEffectCostCombo != null ? ResourceEffectCostCombo.ResourceEffectCombo : null;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Swimming.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);

        }
        #endregion
        #region MonoBehaviours
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion
        #endregion

        #region private functions
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to SwimmingCurrentState and only run the intention update method if skill is enabled.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _swimmingIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _swimmingIntentionUpdater.UpdateMethod = () =>
                {
                    Swimming.OnSurface.Value = CheckOnSurface();
                    Swimming.Intention.Value = SetSwimmingStateIntention();
                };
                Swimming.SwimmingCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                if (_swimmingIntentionUpdater != null)
                {
                    _swimmingIntentionUpdater.Stop();
                }
                Swimming.SwimmingCurrentState.OnValueChanged -= OnSwimmingDisabled;
            }
        }

        /// <summary>
        /// Called when [swimming disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnSwimmingDisabled(object sender, ActionPropertyEventArgs<SwimmingState> state)
        {
            OnCurrentStateChanged(state.Value);
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the swimming state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game.
        /// </summary>
        /// <returns></returns>
        protected abstract SwimmingIntention SetSwimmingStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        protected virtual void OnCurrentStateChanged(SwimmingState currentState)
        {
            /* SwimmingStates 
             * OutOfFluid == 0
             * InWater == 1
             * DoingStroke == 2
             * SurfaceJumping == 3 */
            _animator.SetInteger("SwimmingState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter out of fluid].
        /// </summary>
        public virtual void OnStateEnterOutOfFluid()
        {
            // Reset character
            OwnerCharacter.UseGravity(true);
            if (CharacterAirSupply != null)
            {
                // Set up an updater to restore all air, and stop it when air capacity is full
                _restoreAirUpdater = ComponentExtensions.AddComponentOnEmptyChild<UpdateTemporaryGameObject>(gameObject, "Restore air updater");
                _restoreAirUpdater.UpdateMethod = () =>
                {
                    if (CharacterAirSupply.Current.Value <=
                        CharacterAirSupply.Max.Value)
                    {
                        RestoreAirSupply();
                    }
                    else
                    {
                        _restoreAirUpdater.Stop();
                    }
                };
            }
        }

        /// <summary>
        /// Called when [state enter in fluid].
        /// </summary>
        public virtual void OnStateEnterInFluid()
        {
            if (_restoreAirUpdater != null)
            {
                _restoreAirUpdater.Stop();
            }
            if (_justSplashedTimer != null)
            {
                // If there already was a splashed timer active, finish it before starting a new
                _justSplashedTimer.FinishTimer();
            }
            // Dont use normal gravity while in Fluid.
            OwnerCharacter.UseGravity(false);

            // Set JustSplashed to true and set up a timer for resetting it to false
            Swimming.JustSplashed.Value = true;
            _justSplashedTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Just splashed timer");
            _justSplashedTimer.TimerMethod = () => Swimming.JustSplashed.Value = false;
            _justSplashedTimer.Invoke(0.25f);
        }

        /// <summary>
        /// Called when [state update in fluid].
        /// </summary>
        public virtual void OnStateUpdateInFluid()
        {

            if (Swimming.CurrentFluid == null
                ||
                Swimming.CurrentFluid.Value == null)
            {

                // If there is no CurrentFluid, transition to out of water
                Swimming.TransitionToStateOutOfFluid();
            }

            // Dampen character movement
            ApplyFluidDampening();
        }

        /// <summary>
        /// Called when [state enter doing stroke].
        /// </summary>
        public virtual void OnStateEnterDoingStroke()
        {
            if (Swimming.SecondsBetweenStrokes == 0)
            {
                Debug.LogError("Seconds between strokes for swimming skill cannot be zero. Defaulting to 0.5 seconds");
                Swimming.SecondsBetweenStrokes = 0.5f;
            }
            // Do the swim stroke
            DoStroke();
            // Setup stroke countdown timer based on SecondsBetweenStrokes
            _strokeCountDownTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Stroke countdown timer");
            _strokeCountDownTimer.TimerMethod = () => Swimming.TransitionToStateInFluid();
            _strokeCountDownTimer.Invoke(Swimming.SecondsBetweenStrokes);
        }


        /// <summary>
        /// Called when [state update doing stroke].
        /// </summary>
        public virtual void OnStateUpdateDoingStroke()
        {
        
        }

        /// <summary>
        /// Called when [state exit doing stroke].
        /// </summary>
        public virtual void OnStateExitDoingStroke()
        {
            if (_strokeCountDownTimer != null)
            {
                // Stop the stroke countdown timer if exit state
                _strokeCountDownTimer.FinishTimer();
            }
        }

        /// <summary>
        /// Called when [state enter surface jumping].
        /// </summary>
        public virtual void OnStateEnterSurfaceJumping()
        {
            // Use gravity again, and do the jump
            OwnerCharacter.UseGravity(true);
            DoSurfaceJump();

            // Timer to check if the surface jump was to weak to get character out of the fluid -> get back into swimming
            // Adjustment may be needed if changing surface jumping speed, gravity power, rigidbody mass etc
            _surfaceJumpTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Surface jump timer");
            _surfaceJumpTimer.TimerMethod = () =>
            {
                Swimming.TransitionToStateInFluid();
            };
            _surfaceJumpTimer.Invoke(0.5f);
        }

        /// <summary>
        /// Called when [state update surface jumping].
        /// </summary>
        public virtual void OnStateUpdateSurfaceJumping()
        {
            
        }

        /// <summary>
        /// Called when [state exit surface jumping].
        /// </summary>
        public virtual void OnStateExitSurfaceJumping()
        {
            if (_surfaceJumpTimer != null)
            {
                // Stop the surface jump timer if exit state        
                _surfaceJumpTimer.FinishTimer();
            }
        }

        #endregion

        /// <summary>
        /// Restores the characters air supply.
        /// </summary>
        public void RestoreAirSupply()
        {
            if (OwnerCharacter == null || OwnerCharacter.Resources == null || CharacterAirSupply == null) return;

            if (CharacterAirSupply.Current.Value < CharacterAirSupply.Max.Value)
            {
                CharacterAirSupply.Current.Value += 1;
            }
        }

        #region abstract functions
        /// <summary>
        /// Applies the fluid dampening.
        /// </summary>
        public abstract void ApplyFluidDampening();

        /// <summary>
        /// Does the stroke.
        /// </summary>
        public abstract void DoStroke();

        /// <summary>
        /// Does the surface jump.
        /// </summary>
        public abstract void DoSurfaceJump();

        /// <summary>
        /// Checks if the character is on surface of the fluid.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckOnSurface();
        #endregion
    }
}