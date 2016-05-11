using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement;
using UnityEngine;

namespace AGS.Core.Examples.GameViewExamples
{
    /// <summary>
    /// CharacterControllerHorizontalMovementView for CharacterController
    /// </summary>    
    public class CharacterControllerHorizontalMovementView : HorizontalMovementBaseView
    {
        private CharacterController _characterController;
        private float _originalCharacterHeight;
        private Vector3 _originalCharacterHeightCenter;

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _characterController = GetComponentInParent<CharacterController>();

            if (_characterController != null)
            {
                _originalCharacterHeight = _characterController.height;
                _originalCharacterHeightCenter = _characterController.center;
            }
        }
        #endregion

        #region abstract overrides
        /// <summary>
        /// Moves the avatar.
        /// </summary>
        public override void MoveAvatar()
        {
            HorizontalMovement.CurrentSpeed.Value = 0f;
        }

        /// <summary>
        /// Moves the avatar.
        /// </summary>
        /// <param name="speed">The speed.</param>
        public override void MoveAvatar(float speed)
        {

            if (_characterController == null) return;

            if (OwnerCharacter.MovementSkills.Value.VerticalMovement.Value != null)
            {
                if (OwnerCharacter.MovementSkills.Value.VerticalMovement.Value.VerticalMovementCurrentState.Value == VerticalMovementState.WallJumping)
                {
                    return;
                }
            }

            var speedEffected = speed * OwnerCharacter.RelativeSpeedEffect.Value;
            speedEffected = speedEffected + OwnerCharacter.FixedSpeedEffect.Value;
            if (speedEffected < 0f)
            {
                speedEffected = 0f;
            }
            if (OwnerCharacter.CharacterController.Value.MoveVector.Value.y < 0)
            {
                speedEffected *= 0.5f;
            }
            HorizontalMovement.CurrentSpeed.Value = speedEffected * OwnerCharacter.CharacterController.Value.MoveVector.Value.y;
            if (OwnerCharacter.AirborneSpeedEffect.Value > 0 && OwnerCharacter.AirborneSpeedEffect.Value < 1)
            {
                // adjust speed with any movement multiplier
                HorizontalMovement.CurrentSpeed.Value = HorizontalMovement.CurrentSpeed.Value * OwnerCharacter.AirborneSpeedEffect.Value;
            }
            if (OwnerCharacter.OwnerGameLevel != null)
            {
                StopHorizontalMovement = OwnerCharacter.CharacterOutOfBoundsHorizontal.Value;
            }

            if (StopHorizontalMovement)
            {
                HorizontalMovement.CurrentSpeed.Value = 0f;
            }
        }

        /// <summary>
        /// Resets the height of the character.
        /// </summary>
        protected override void ResetCharacterHeight()
        {
            OwnerCharacter.Height.Value = _originalCharacterHeight;
            OwnerCharacter.HeightCenter.Value = _originalCharacterHeightCenter;
        }

        /// <summary>
        /// Sets the height of the character when crouching.
        /// </summary>
        public override void SetCrouchingRelativeHeight()
        {
            OwnerCharacter.Height.Value *= OwnerCharacter.MovementSkills.Value.HorizontalMovement.Value.CrouchRelativeHeight;
            OwnerCharacter.HeightCenter.Value *= OwnerCharacter.MovementSkills.Value.HorizontalMovement.Value.CrouchRelativeHeight;
        }

        /// <summary>
        /// Sets the horizontal movement state intention.
        /// </summary>
        /// <returns></returns>
        protected override HorizontalMovementIntention SetHorizontalMovementStateIntention()
        {
            if (HorizontalMovement == null) return HorizontalMovementIntention.Idle;
            if (HorizontalMovement.IsEnabled.Value != true) return HorizontalMovementIntention.Idle;

            if (OwnerCharacterController == null) return HorizontalMovementIntention.Idle;

            if (HorizontalMovement.HorizontalMovementCurrentState.Value == HorizontalMovementState.Crouching)
            {
                if (OwnerCharacterController.Crouch.Value)
                {
                    return HorizontalMovementIntention.Idle;
                }
                if (OwnerCharacter.MovementSkills.Value.VerticalMovement.Value != null
                    &&
                    OwnerCharacter.MovementSkills.Value.VerticalMovement.Value.VerticalMovementCurrentState.Value == VerticalMovementState.Jumping)
                {
                    return HorizontalMovementIntention.Idle;
                }
                return HorizontalMovementIntention.Crouch;
            }
            if (OwnerCharacterController.Crouch.Value)
            {
                return HorizontalMovementIntention.Crouch;
            }

            return (Mathf.Abs(OwnerCharacterController.MoveVector.Value.x) > 0.01f
                            ||
                            Mathf.Abs(OwnerCharacterController.MoveVector.Value.y) > 0.01f)
                            ? HorizontalMovementIntention.Move
                            : HorizontalMovementIntention.Idle;
        }
        #endregion

    }
}

