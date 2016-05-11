using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.MovementSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.VerticalMovement
{
    /// <summary>
    /// VerticalMovementBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class VerticalMovementBaseView : MovementSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float JumpSpeed;
        public bool CanComboJump;
        public float ComboJumpMultiplier;
        public int CombosEnabled;
        public float ComboTimerSeconds;
        public bool CanWallJump;
        public float WallJumpSpeedVertical;
        public float WallJumpSpeedHorizontal;
        #endregion

        public VerticalMovement VerticalMovement;
        private UpdatePersistantGameObject _verticalMovementIntentionUpdater; // For use when updating the characters intention
        private Animator _animator;
        private VerticalMovementState _previousVerticalMovementState;
        private TimerTemporaryGameObject _closeToGroundTimer;
        private TimerTemporaryGameObject _comboJumpTimer;
        #region AGS Setup
        public override void InitializeView()
        {
            VerticalMovement = new VerticalMovement(JumpSpeed, CanComboJump, ComboJumpMultiplier, CombosEnabled, ComboTimerSeconds, CanWallJump, WallJumpSpeedVertical, WallJumpSpeedHorizontal);
            SolveModelDependencies(VerticalMovement);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            VerticalMovement.ResourceCost.Value = ResourceEffectCostCombo != null ? ResourceEffectCostCombo.ResourceEffectCombo : null;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            VerticalMovement.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
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
        /// Only subscribe to SlidingCurrentState and only run the intention update method if skill is enabled.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {

            if (isEnabled)
            {
                _verticalMovementIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _verticalMovementIntentionUpdater.UpdateMethod = () =>
                {
                    VerticalMovement.Intention.Value = SetVerticalMovementStateIntention();
                };
                VerticalMovement.VerticalMovementCurrentState.OnValueChanged += (sender, verticalMovementState) => OnCurrentStateChanged(verticalMovementState.Value);
            }
            else
            {
                if (_verticalMovementIntentionUpdater != null)
                {
                    _verticalMovementIntentionUpdater.Stop();
                }
                VerticalMovement.VerticalMovementCurrentState.OnValueChanged -= OnVerticalMovementDisabled;
            }
        }

        /// <summary>
        /// Called when [vertical movement disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="verticalMovementState">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnVerticalMovementDisabled(object sender, ActionPropertyEventArgs<VerticalMovementState> verticalMovementState)
        {
            OnCurrentStateChanged(verticalMovementState.Value);
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the vertical movement state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game
        /// </summary>
        /// <returns></returns>
        protected abstract VerticalMovementIntention SetVerticalMovementStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        protected virtual void OnCurrentStateChanged(VerticalMovementState currentState)
        {
            // Stop possible CloseToGroundTimer on state change
            if (_closeToGroundTimer != null)
            {
                _closeToGroundTimer.FinishTimer();
            }

            /* VerticalMovementStates 
             * Idle == 0
             * Landing == 1
             * Jumping == 2
             * Falling == 3
             * WallJumping == 4 */
            _animator.SetInteger("VerticalMovementState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public virtual void OnStateEnterIdle()
        {
            if (VerticalMovement == null) return;
            // Reset all values
            VerticalMovement.CloseToGround.Value = true;
            VerticalMovement.JustJumped.Value = false;
            _previousVerticalMovementState = VerticalMovementState.Idle;
        }

        /// <summary>
        /// Called when [state enter falling].
        /// </summary>
        public virtual void OnStateEnterFalling()
        {
            if (VerticalMovement.CanComboJump)
            {
                // If character is falling after a jump. Increase combo jumps executed
                if (VerticalMovement.JustJumped.Value)
                {
                    VerticalMovement.ComboJumpsExecuted.Value++;
                }
                else
                {
                    VerticalMovement.ComboJumpsExecuted.Value = 0;
                }
            }

            // Sets up a close to ground timer, if character wasnt jumping or walljumping before falling
            if (_previousVerticalMovementState != VerticalMovementState.Jumping
                ||
                _previousVerticalMovementState != VerticalMovementState.WallJumping)
            {
                _closeToGroundTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Close to ground timer");
                _closeToGroundTimer.TimerMethod = () => VerticalMovement.CloseToGround.Value = false;
                _closeToGroundTimer.Invoke(0.2f);
            }
            _previousVerticalMovementState = VerticalMovementState.Falling;
        }

        /// <summary>
        /// Called when [state enter jumping].
        /// </summary>
        public virtual void OnStateEnterJumping()
        {
            VerticalMovement.SetCurrentJumpSpeed((VerticalMovement.JumpSpeed + OwnerCharacter.FixedSpeedEffect.Value) * OwnerCharacter.RelativeSpeedEffect.Value);
            if (VerticalMovement.CanComboJump)
            {
                // Increase jumping speed for combos
                if (VerticalMovement.ComboJumpsExecuted.Value > 0)
                {
                    VerticalMovement.SetCurrentJumpSpeed(VerticalMovement.JumpSpeed + VerticalMovement.CurrentJumpSpeed.Value * VerticalMovement.ComboJumpsExecuted.Value * VerticalMovement.ComboJumpMultiplier + 1f);
                }
            }

            // Make the jump
            DoJump(VerticalMovement.CurrentJumpSpeed.Value);

            // Set up a close to ground timer
            _closeToGroundTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Close to ground timer");
            _closeToGroundTimer.TimerMethod = () => VerticalMovement.CloseToGround.Value = false;
            _closeToGroundTimer.Invoke(0.2f);
            _previousVerticalMovementState = VerticalMovementState.Jumping;
        }

        /// <summary>
        /// Called when [state enter landing].
        /// </summary>
        public virtual void OnStateEnterLanding()
        {
            VerticalMovement.CloseToGround.Value = true;
            VerticalMovement.JustJumped.Value = false;


            if (!VerticalMovement.CanComboJump || VerticalMovement.ComboJumpsExecuted.Value >= VerticalMovement.CombosEnabled)
            {
                // if cant combo jump or reached combo limit - reset and proceed to idle
                VerticalMovement.ComboJumpsExecuted.Value = 0;
                VerticalMovement.TransitionToStateIdle();
            }
            else
            {

                // start timer for combo input. Timer will be stopped if state changes
                _comboJumpTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Combo jump timer");
                _comboJumpTimer.TimerMethod = () =>
                {
                    if (VerticalMovement != null)
                    {
                        VerticalMovement.ComboJumpsExecuted.Value = 0;
                    }
                    VerticalMovement.TransitionToStateIdle();
                };
                _comboJumpTimer.Invoke(VerticalMovement.ComboTimer);
            }
            _previousVerticalMovementState = VerticalMovementState.Landing;
        }

        /// <summary>
        /// Called when [state exit landing].
        /// </summary>
        public virtual void OnStateExitLanding()
        {
            if (_comboJumpTimer != null)
            {
                _comboJumpTimer.FinishTimer();
            }
        }

        /// <summary>
        /// Called when [state enter wall jumping].
        /// </summary>
        public virtual void OnStateEnterWallJumping()
        {
            VerticalMovement.ComboJumpsExecuted.Value = 0;

            // Make sure character is completely still before wall jump
            OwnerCharacter.StopMovement();
            OwnerCharacter.TurnInDirection(OwnerCharacter.ForwardReflectionVector.Value);

            // disable movement direction to prevent continued walljumping on same wall
            OwnerCharacter.DirectionLocked.Value = true;

            // Make the wall jump
            DoWallJump();
            _previousVerticalMovementState = VerticalMovementState.WallJumping;
        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Does the jump.
        /// </summary>
        /// <param name="jumpSpeed">The jump speed.</param>
        public abstract void DoJump(float jumpSpeed);

        /// <summary>
        /// Does the wall jump.
        /// </summary>
        public abstract void DoWallJump();
        #endregion


    }
}