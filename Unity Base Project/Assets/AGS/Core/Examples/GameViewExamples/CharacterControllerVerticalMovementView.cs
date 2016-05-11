using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.MovementSkills.VerticalMovement;
using UnityEngine;

namespace AGS.Core.Examples.GameViewExamples
{
    /// <summary>
    /// CharacterControllerVerticalMovementView for CharacterController characters
    /// </summary>
    public class CharacterControllerVerticalMovementView : VerticalMovementBaseView
    {
        private CharacterController _characterController;

        public override void Start()
        {
            base.Start();
            _characterController = GetComponentInParent<CharacterController>();
        }

        #region abstract overrides
        /// <summary>
        /// Sets the vertical movement state intention.
        /// </summary>
        /// <returns></returns>
        protected override VerticalMovementIntention SetVerticalMovementStateIntention()
        {
            if (VerticalMovement == null) return VerticalMovementIntention.None;
            if (_characterController == null) return VerticalMovementIntention.None;
            if (VerticalMovement.IsEnabled.Value != true) return VerticalMovementIntention.None;
            if (OwnerCharacter == null || OwnerCharacterController == null) return VerticalMovementIntention.None;
            if (OwnerCharacter.IsGrounded.Value)
            {
                if (VerticalMovement.VerticalMovementCurrentState.Value == VerticalMovementState.Idle
                    ||
                    VerticalMovement.VerticalMovementCurrentState.Value == VerticalMovementState.Landing)
                {
                    if (OwnerCharacterController.Jump.Value)
                    {
                        VerticalMovement.CheckSupplyResourceEffects(OwnerCharacter);
                        if (!VerticalMovement.OutOfResources.Value)
                        {
                            return VerticalMovementIntention.Jump;
                        }
                    }
                }
                else if (VerticalMovement.VerticalMovementCurrentState.Value == VerticalMovementState.Falling)
                {
                    return VerticalMovementIntention.Land;
                }
                if (_characterController.velocity.y <= 0
                    &&
                    !VerticalMovement.CloseToGround.Value
                    &&
                    (VerticalMovement.VerticalMovementCurrentState.Value == VerticalMovementState.Jumping))
                {
                    return VerticalMovementIntention.Land;
                }
            }
            else
            {
                return VerticalMovementIntention.Fall;
            }
            return VerticalMovementIntention.None;
        }

        /// <summary>
        /// Does the jump.
        /// </summary>
        /// <param name="jumpSpeed">The jump speed.</param>
        public override void DoJump(float jumpSpeed)
        {
            // prevent jumpspam while close to ground
            if (!VerticalMovement.JustJumped.Value)
            {
                // Due to the nature of CharacterControllers, we add the jump to the movevector in the playerview
                VerticalMovement.JustJumped.Value = true;
            }

        }

        /// <summary>
        /// Does the wall jump.
        /// </summary>
        public override void DoWallJump()
        {
            // Not implemented
        }
        #endregion
    }
}

