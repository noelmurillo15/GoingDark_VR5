using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.HazardSystem;
using AGS.Core.Systems.MovementSystem.Base;
using AGS.Core.Utilities;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement
{
    /// <summary>
    /// HorizontalMovementBaseView requires a mecanim animator and is responsible for setting the animator in the correct state based on the models current state.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class HorizontalMovementBaseView : MovementSkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float SprintSpeed;
        public float CrouchSpeed;
        public float SneakSpeed;
        public float StrafeSpeed;
        public float CrouchingRelativeHeight;
        #endregion

        public bool StopHorizontalMovement { get; set; }
        public bool BeenCrouching { get; set; }
        public bool CanStandUp { get; set; }
        protected float OriginalCharacterHeight;
        protected Vector3 OriginalCharacterHeightCenter;
        protected LayerMask GroundMask; // Mask for ground detection

        public HorizontalMovement HorizontalMovement;

        private UpdatePersistantGameObject _horizontalMovementIntentionUpdater; // For use when updating the characters intention
        private Animator _animator;

        #region AGS Setup
        public override void InitializeView()
        {
            HorizontalMovement = new HorizontalMovement(SprintSpeed, CrouchSpeed, SneakSpeed, StrafeSpeed, CrouchingRelativeHeight);
            SolveModelDependencies(HorizontalMovement);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            HorizontalMovement.ResourceCost.Value = ResourceEffectCostCombo != null ? ResourceEffectCostCombo.ResourceEffectCombo : null;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            HorizontalMovement.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
        }
        #endregion
        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            if (GetComponent<GroundMask>() != null)
            {
                GroundMask = GetComponent<GroundMask>().Mask;
            }
        }
        
        #endregion
        #region private functions      
        /// <summary>
        /// Called when [skill state changed].
        /// Only subscribe to HorizontalMovementCurrentState and only run the intention update method if skill is enabled.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                _horizontalMovementIntentionUpdater = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                _horizontalMovementIntentionUpdater.UpdateMethod = () =>
                {
                    HorizontalMovement.Intention.Value = SetHorizontalMovementStateIntention();
                };
                HorizontalMovement.HorizontalMovementCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            }
            else
            {
                if (_horizontalMovementIntentionUpdater != null)
                {
                    _horizontalMovementIntentionUpdater.Stop();
                }
                HorizontalMovement.HorizontalMovementCurrentState.OnValueChanged -= OnHorizontalMovementDisabled;
            }
        }

        /// <summary>
        /// Called when [horizontal movement disabled].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnHorizontalMovementDisabled(object sender, ActionPropertyEventArgs<HorizontalMovementState> state)
        {
            OnCurrentStateChanged(state.Value);
        }

        /// <summary>
        /// Checks if there is room to stand up.
        /// </summary>
        /// <returns></returns>
        private bool CheckRoomToStandUp()
        {
            var rayDirection = transform.up;
            var rayVector = new Vector3(transform.position.x, transform.position.y + OwnerCharacter.Height.Value, transform.position.z);
            Debug.DrawRay(rayVector, rayDirection * (OriginalCharacterHeight - OwnerCharacter.Height.Value), Color.white);
            RaycastHit hitInfo;
            var rayCastHit = Physics.Raycast(rayVector, rayDirection, out hitInfo, OriginalCharacterHeight - OwnerCharacter.Height.Value, GroundMask);
            if (!rayCastHit)
            {
                return true;
            }
            return hitInfo.transform.GetComponent<HazardBaseView>(); // Let the character stand up if there is a trap above
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Sets the horizontal movement state intention.
        /// Intention input should always be implemented in the child view since controls differs from game to game.
        /// </summary>
        /// <returns></returns>
        protected abstract HorizontalMovementIntention SetHorizontalMovementStateIntention();

        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(HorizontalMovementState currentState)
        {
            if (BeenCrouching)
            {
                ResetCharacterHeight();
                BeenCrouching = false;
            }

            /* HorizontalMovementStates 
             * Idle == 0
             * Moving == 1
             * Crouching == 2
             * Sprinting == 3
             * Sneaking == 4
             * Strafing == 5 */
            _animator.SetInteger("HorizontalMovementState", (int)currentState);

        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public virtual void OnStateEnterIdle()
        {
            HorizontalMovement.CurrentSpeed.Value = 0f;
        }

        /// <summary>
        /// Called when [state update idle].
        /// </summary>
        public virtual void OnStateUpdateIdle()
        {
            // Even when idle, we may need to move if we are on moving environment
            MoveAvatar();
        }

        /// <summary>
        /// Called when [state update moving].
        /// </summary>
        public virtual void OnStateUpdateMoving()
        {
            // Move with character speed if we are not getting pushed at this moment
            if (!OwnerCharacter.BeingPushed.Value)
            {
                MoveAvatar(OwnerCharacter.Speed);
            }
        }

        /// <summary>
        /// Called when [state update sneaking].
        /// </summary>
        public virtual void OnStateUpdateSneaking()
        {
            // Move with sneak speed if we are not getting pushed at this moment
            if (!OwnerCharacter.BeingPushed.Value)
            {
                MoveAvatar(HorizontalMovement.SneakSpeed);
            }
        }

        /// <summary>
        /// Called when [state enter crouching].
        /// </summary>
        public virtual void OnStateEnterCrouching()
        {
            // Make the character collider smaller
            SetCrouchingRelativeHeight();
            BeenCrouching = true;
            CanStandUp = false;
        }

        /// <summary>
        /// Called when [state update crouching].
        /// </summary>
        public virtual void OnStateUpdateCrouching()
        {

            // Move with crouching speed if we are not getting pushed at this moment
            if (!OwnerCharacter.BeingPushed.Value)
            {
                MoveAvatar(HorizontalMovement.CrouchSpeed);
            }
            CanStandUp = CheckRoomToStandUp();
        }

        /// <summary>
        /// Called when [state enter sprinting].
        /// </summary>
        public virtual void OnStateEnterSprinting()
        {
            // Activate continuous resource costs when sprinting
            HorizontalMovement.ActivateContinuousResourceCosts(OwnerCharacter);
        }

        /// <summary>
        /// Called when [state update sprinting].
        /// </summary>
        public virtual void OnStateUpdateSprinting()
        {
            // If we have run out of resources - transition to normal move
            if (HorizontalMovement.OutOfResources.Value)
            {
                HorizontalMovement.TransitionToStateMove();
            }
            // Move with sprinting if we are not getting pushed at this moment
            if (!OwnerCharacter.BeingPushed.Value)
            {
                MoveAvatar(HorizontalMovement.SprintSpeed);
            }
        }

        /// <summary>
        /// Called when [state exit sprinting].
        /// </summary>
        public virtual void OnStateExitSprinting()
        {
            // Deactivate continuous resource costs when done sprinting
            HorizontalMovement.DeactivateContinuousResourceCosts();
        }

        /// <summary>
        /// Called when [state enter strafing].
        /// </summary>
        public virtual void OnStateEnterStrafing()
        {
            
        }

        /// <summary>
        /// Called when [state update strafing].
        /// </summary>
        public virtual void OnStateUpdateStrafing()
        {
            StrafeAvatar();
        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Resets the height of the character.
        /// </summary>
        protected abstract void ResetCharacterHeight();

        /// <summary>
        /// Moves the avatar.
        /// </summary>
        public abstract void MoveAvatar();

        /// <summary>
        /// Moves the avatar.
        /// </summary>
        /// <param name="speed">The speed.</param>
        public abstract void MoveAvatar(float speed);

        /// <summary>
        /// Sets the height of the character when crouching.
        /// </summary>
        public abstract void SetCrouchingRelativeHeight();

        /// <summary>
        /// Strafes the avatar.
        /// </summary>
        public virtual void StrafeAvatar() {}
        #endregion

        /// <summary>
        /// Check if there is environment ahead that is too steep to walk on.
        /// </summary>
        /// <returns></returns>
        protected bool HorizontalCorrection()
        {
            return (OwnerCharacter.HitForward.Value != HitObstacle.None)
                   &&
                   (Mathf.Abs(OwnerCharacter.HitForwardAngle.Value) > OwnerCharacter.SlopeLimitMoving || Mathf.Abs(OwnerCharacter.HitForwardAngle.Value) < 0.001f);
        }

        /// <summary>
        /// Calculate a vertical movement multiplier for smooth movement on slopes.
        /// </summary>
        /// <param name="curSpeed">The current speed.</param>
        /// <returns></returns>
        protected float HandleVerticalSlopes(float curSpeed)
        {

            var slopeSpeedCorrection = 0f;

            if (OwnerCharacter.HitBelow.Value != HitObstacle.Ground
                    &&
                    OwnerCharacter.HitBelow.Value != HitObstacle.MovingGround
                     &&
                    OwnerCharacter.HitBelow.Value != HitObstacle.Stairs)
            {
                return slopeSpeedCorrection;
            }
            if (Mathf.Abs(OwnerCharacter.SlopeAngleForward.Value) > 0.001f)
            {
                if (OwnerCharacter.SlopeAngleForward.Value <= OwnerCharacter.SlopeLimitMoving)
                {
                    slopeSpeedCorrection = Mathf.Abs((Mathf.Tan(OwnerCharacter.SlopeAngleForward.Value * Mathf.Deg2Rad) * curSpeed));
                    if (OwnerCharacter.SlopeAngleForward.Value < 0)
                    {
                        slopeSpeedCorrection *= -1;
                    }
                }
            }
            return slopeSpeedCorrection;
        }
    }
}
