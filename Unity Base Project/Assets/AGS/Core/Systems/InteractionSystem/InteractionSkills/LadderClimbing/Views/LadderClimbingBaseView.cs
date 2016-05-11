using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.LadderClimbing
{
    /// <summary>
    /// LadderClimbingBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class LadderClimbingBaseView : InteractionSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float ClimbOffsetVertical;
        public float ClimbOffsetHorizontal;
        public float ClimbSpeed;
        public float LadderJumpSpeedHorizontal;
        public float LadderJumpSpeedVertical;
        #endregion

        public LadderClimbing LadderClimbing;
        private Animator _animator;
        private float _smoothness;
        private Vector3 _targetPosition;
        #region AGS Setup
        public override void InitializeView()
        {
            LadderClimbing = new LadderClimbing(ApproachMargin, OffsetVertical, OffsetHorizontal, ClimbOffsetVertical, ClimbOffsetHorizontal, ClimbSpeed, LadderJumpSpeedHorizontal, LadderJumpSpeedVertical);
            SolveModelDependencies(LadderClimbing);
        }
        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            LadderClimbing.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
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
        /// Called when [skill state changed].
        /// Only subscribe to LadderClimbingCurrentState if skill is enabled 
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                LadderClimbing.LadderClimbingCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                LadderClimbing.LadderClimbingCurrentState.OnValueChanged -= OnLadderClimbingDisabled;
            }
        }
        private void OnLadderClimbingDisabled(object sender, ActionPropertyEventArgs<LadderClimbingState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(LadderClimbingState currentState)
        {
            /* LadderClimbingStates 
             * Idle == 0
             * Approaching == 1
             * Climbing == 2
             * Releasing == 3
             * ExitingTop == 4
             * Jumping == 5 */
            _animator.SetInteger("LadderClimbingState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public override void OnStateEnterIdle()
        {
            OwnerCharacter.UsePhysics(true);
            _targetPosition = Vector3.zero;
        }

        /// <summary>
        /// Called when [state enter approaching].
        /// </summary>
        public override void OnStateEnterApproaching()
        {
            OwnerCharacter.UsePhysics(false);
            FaceLadder();
            _smoothness = 3f * Time.deltaTime;
            _targetPosition = GetTargetPosition();    
        }

        /// <summary>
        /// Called when [state update approaching].
        /// </summary>
        public override void OnStateUpdateApproaching()
        {
            // Move towards target until approach margin is reached
            MoveTowardsInteractableTarget(_targetPosition, _smoothness);
            if (ReachedTargetPosition(_targetPosition, LadderClimbing.ApproachMargin))
            {
                LadderClimbing.TransitionToStateInteract();
            } 
        }

        /// <summary>
        /// Called when [state enter releasing].
        /// </summary>
        public override void OnStateEnterReleasing()
        {
            // Also make sure that interactionvolumes and targets are reset
            OwnerInteractionSkills.ForceClear();
            LadderClimbing.TransitionToStateIdle();  
        }

        /// <summary>
        /// Called when [state enter jumping].
        /// </summary>
        public virtual void OnStateEnterJumping()
        {
            OwnerCharacter.UsePhysics(true);
            DoJump();

            // Set up a timer for when to release
            var releaseTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Release timer");
            releaseTimer.TimerMethod = () => LadderClimbing.TransitionToStateRelease();
            releaseTimer.Invoke(0.5f);  
        }

        /// <summary>
        /// Called when [state enter climbing].
        /// </summary>
        public virtual void OnStateEnterClimbing()
        {
            // Do nothing   
        }

        /// <summary>
        /// Called when [state update climbing].
        /// </summary>
        public virtual void OnStateUpdateClimbing()
        {
            if (CharTransform == null) return;

            // Listen for jump input while climbing a ladder
            if (OwnerCharacter.CharacterController.Value.Jump.Value)
            {
                LadderClimbing.TransitionToStateJump();
            }

            // Calculate climbing speed based on any added effects
            var speedEffected = ((LadderClimbing.ClimbSpeed + OwnerCharacter.FixedSpeedEffect.Value) * OwnerCharacter.RelativeSpeedEffect.Value) * Time.deltaTime;

            if (speedEffected < 0f)
            {
                speedEffected = 0f;
            }
            var curSpeed = OwnerCharacter.CharacterController.Value.MoveVector.Value.y * speedEffected;

            ClimbLadder(curSpeed, speedEffected);
        }

        /// <summary>
        /// Called when [state enter exiting top].
        /// </summary>
        public virtual void OnStateEnterExitingTop()
        {
            _smoothness = 3f * Time.deltaTime;
            // calculate offsets
            _targetPosition = GetExitPosition();   
        }

        /// <summary>
        /// Called when [state update exiting top].
        /// </summary>
        public virtual void OnStateUpdateExitingTop()
        {
            if (CurrentInteractableTarget != null)
            {
                MoveTowardsInteractableTarget(_targetPosition, _smoothness);
            }
            if (ReachedTargetPosition(_targetPosition, LadderClimbing.ApproachMargin))
            {
                LadderClimbing.TransitionToStateRelease();
            }   
        }
        #endregion

        #region abstracts
        /// <summary>
        /// Makes character face the ladder.
        /// </summary>
        public abstract void FaceLadder();

        /// <summary>
        /// Makes character climb the ladder.
        /// </summary>
        /// <param name="curSpeed">The current speed.</param>
        /// <param name="speedEffected">The speed effected.</param>
        public abstract void ClimbLadder(float curSpeed, float speedEffected);

        /// <summary>
        /// Gets the exit position.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 GetExitPosition();

        /// <summary>
        /// Does the jump.
        /// </summary>
        public abstract void DoJump();
        #endregion        
    }
}