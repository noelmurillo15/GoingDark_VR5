using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.LedgeClimbing
{
    /// <summary>
    /// LedgeClimbingBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    public abstract class LedgeClimbingBaseView : InteractionSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float ClimbOffsetHorizontal;
        public float ClimbOffsetVertical;
        public float ExitMargin;
        public float LedgeClimbSpeed;
        public float LedgeJumpSpeedVertical;
        public float LedgeJumpSpeedHorizontal;
        #endregion

        public LedgeClimbing LedgeClimbing;

        private UpdatePersistantGameObject _ledgeClimbingIntentionUpdater; // For use when updating the characters intention
        private Animator _animator;
        private Vector3 _currentClimbTarget;
        private float _smoothness;
        protected Vector3 TargetPosition;

        #region AGS Setup
        public override void InitializeView()
        {
            LedgeClimbing = new LedgeClimbing(ApproachMargin, OffsetVertical, OffsetHorizontal, ClimbOffsetHorizontal, ClimbOffsetVertical, ExitMargin, LedgeClimbSpeed, LedgeJumpSpeedVertical, LedgeJumpSpeedHorizontal);
            SolveModelDependencies(LedgeClimbing);
        }
        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            LedgeClimbing.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
        }
        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the ledge climbing state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game.
        /// </summary>
        /// <returns></returns>
        protected abstract LedgeClimbingIntention SetLedgeClimbingStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(LedgeClimbingState currentState)
        {
            /* LedgeClimbingStates 
             * Idle == 0
             * Approaching == 1
             * Climbing == 2
             * Releasing == 3
             * Grabbing == 4
             * Jumping == 5 */
            _animator.SetInteger("LedgeClimbingState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            OwnerCharacter.UsePhysics(true);
            if (OwnerInteractionSkills == null) return;
            CurrentInteractableTarget = null;  
        }

        /// <summary>
        /// Called when [state enter approaching].
        /// </summary>
        public override void OnStateEnterApproaching()
        {
            OwnerCharacter.UsePhysics(false);
            _smoothness = 3f * Time.deltaTime;
            TargetPosition = GetTargetPosition();  
        }

        /// <summary>
        /// Called when [state update approaching].
        /// </summary>
        public override void OnStateUpdateApproaching()
        {
            if (CharTransform == null) return;

            // Move towards target until approach margin is reached
            MoveTowardsInteractableTarget(TargetPosition, _smoothness);
            if (ReachedTargetPosition(TargetPosition, LedgeClimbing.ApproachMargin))
            {
                LedgeClimbing.TransitionToStateInteract();
            }  
        }

        /// <summary>
        /// Called when [state enter releasing].
        /// </summary>
        public override void OnStateEnterReleasing()
        {
            OwnerCharacter.UsePhysics(true);

            // let characters drop out of ledge trigger before exit
            var releaseTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Release timer");
            releaseTimer.TimerMethod = () => LedgeClimbing.TransitionToStateIdle();
            releaseTimer.Invoke(0.5f);
        }

        /// <summary>
        /// Called when [state enter grabbing].
        /// </summary>
        public virtual void OnStateEnterGrabbing()
        {
            // Wait for ledgeclimbing action
        }

        /// <summary>
        /// Called when [state enter climbing].
        /// </summary>
        public virtual void OnStateEnterClimbing()
        {
            OwnerCharacter.UsePhysics(false);

            // Get the offset position
            TargetPosition = GetClimbExitTarget();
            _smoothness = LedgeClimbing.LedgeClimbSpeed * Time.deltaTime;

            // First, climb vertically until target height is reached
            _currentClimbTarget = new Vector3(CharTransform.position.x, TargetPosition.y, CharTransform.position.z);
        }

        /// <summary>
        /// Called when [state exit climbing].
        /// </summary>
        public virtual void OnStateExitClimbing()
        {

        }

        /// <summary>
        /// Called when [state update climbing].
        /// </summary>
        public virtual void OnStateUpdateClimbing()
        {
            if (ClimbLedge(_currentClimbTarget, _smoothness, LedgeClimbing.ExitMargin))
            {
                if (_currentClimbTarget == TargetPosition)
                {
                    // We have reached final target destination. Transition to idle
                    LedgeClimbing.TransitionToStateIdle();
                }
                // Reached target height. Continue to final destination
                _currentClimbTarget = TargetPosition;
            }
        }

        /// <summary>
        /// Called when [state enter jumping].
        /// </summary>
        public virtual void OnStateEnterJumping()
        {
            OwnerCharacter.UsePhysics(true);

            // Unlock direction change, turn around, then lock it again.
            OwnerCharacter.DirectionLocked.Value = false;
            OwnerCharacter.TurnAround();
            OwnerCharacter.DirectionLocked.Value = true;

            // Jump and set up a release timer
            DoJump();
            var releaseTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Release timer");
            releaseTimer.TimerMethod = () =>
            {
                LedgeClimbing.TransitionToStateIdle();
                LedgeClimbing.OwnerInteractionSkills.Value.ForceClear();
            };
            releaseTimer.Invoke(0.5f);
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to LedgeClimbingCurrentState and only run the intention update method if skill is enabled  
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _ledgeClimbingIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _ledgeClimbingIntentionUpdater.UpdateMethod = () =>
                {
                    LedgeClimbing.Intention.Value = SetLedgeClimbingStateIntention();
                };
                LedgeClimbing.LedgeClimbingCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                if (_ledgeClimbingIntentionUpdater != null)
                {
                    _ledgeClimbingIntentionUpdater.Stop();
                }
                LedgeClimbing.LedgeClimbingCurrentState.OnValueChanged -= OnLedgeClimbingDisabled;
            }
        }

        /// <summary>
        /// Called when [ledge climbing disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnLedgeClimbingDisabled(object sender, ActionPropertyEventArgs<LedgeClimbingState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        #endregion

        #region abstract functions
        /// <summary>
        /// Does the jump.
        /// </summary>
        public abstract void DoJump();

        /// <summary>
        /// Gets the climb exit target.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 GetClimbExitTarget();

        /// <summary>
        /// Makes character climb the ledge.
        /// </summary>
        /// <param name="offsetVector">The offset vector.</param>
        /// <param name="smoothness">The smoothness.</param>
        /// <param name="exitMargin">The exit margin.</param>
        /// <returns></returns>
        public abstract bool ClimbLedge(Vector3 offsetVector, float smoothness, float exitMargin);
        #endregion
    }
}