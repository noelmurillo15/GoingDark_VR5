using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.VerticalMovement
{
    /// <summary>
    /// VerticalMovement is meant to be used with any type of vertical movement
    /// </summary>
    public class VerticalMovement : MovementSkillBase
    {      
        #region Properties
        // Constructor properties
        public float JumpSpeed { get; set; }
        public bool CanComboJump { get; set; }
        public float ComboJumpMultiplier { get; set; }
        public int CombosEnabled { get; set; }
        public float ComboTimer { get; set; }
        public bool CanWallJump { get; set; }
        public float WallJumpSpeedVertical { get; set; }
        public float WallJumpSpeedHorizontal { get; set; }

        // Subscribable properties
        public ActionProperty<float> CurrentJumpSpeed { get; private set; } // Effective jump speed after all modifiers including combos
        public ActionProperty<bool> JustJumped { get; set; } // Boolean used to determine if character is airborne after a jump or not, also used to prevent jump spamming
        public ActionProperty<bool> CloseToGround { get; set; } // Did the character just lose connecting with the ground? Can be used to allow a little time window where the character can jump even if just fell off a ledge
        public ActionProperty<int> ComboJumpsExecuted { get; set; }  // Number of combos that has currently been executed
        public ActionProperty<VerticalMovementState> VerticalMovementCurrentState { get; private set; } // vertical movement state machine. Partially dependent on Intention
        public ActionProperty<VerticalMovementIntention> Intention { get; private set; } // The intention value handles the characters "intention". It could, but is not required to, change the VerticalMovementCurrentState

        //private bool _preparingWallJump;        TODO, remove if removing snippet in WallJump
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalMovement"/> class.
        /// </summary>
        /// <param name="jumpSpeed">The jump speed.</param>
        /// <param name="canComboJump">if set to <c>true</c> character [can combo jump].</param>
        /// <param name="comboJumpMultiplier">The combo jump multiplier to be added for each combo jump.</param>
        /// <param name="combosEnabled">Determines how many combo jumps are enabled.</param>
        /// <param name="comboTimer">Determines the combo time window.</param>
        /// <param name="canWallJump">if set to <c>true</c> character [can wall jump].</param>
        /// <param name="wallJumpSpeedVertical">The vertical wall jump speed.</param>
        /// <param name="wallJumpSpeedHorizontal">The horizontal wall jump speed.</param>
        public VerticalMovement(float jumpSpeed, bool canComboJump, float comboJumpMultiplier, int combosEnabled, float comboTimer, bool canWallJump, float wallJumpSpeedVertical, float wallJumpSpeedHorizontal)
        {
            JumpSpeed = jumpSpeed;
            CanComboJump = canComboJump;
            ComboJumpMultiplier = comboJumpMultiplier;
            CombosEnabled = combosEnabled;
            ComboTimer = comboTimer;
            CanWallJump = canWallJump;
            CurrentJumpSpeed = new ActionProperty<float>();
            JustJumped = new ActionProperty<bool> { Value = false };
            CloseToGround = new ActionProperty<bool> { Value = true };
            WallJumpSpeedVertical = wallJumpSpeedVertical;
            WallJumpSpeedHorizontal = wallJumpSpeedHorizontal;
            ComboJumpsExecuted = new ActionProperty<int> { Value = 0 };
            VerticalMovementCurrentState = new ActionProperty<VerticalMovementState>();
            Intention = new ActionProperty<VerticalMovementIntention>() { Value = VerticalMovementIntention.None };
            Intention.OnValueChanged += (sender, intention) => SetMovementState(intention.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to idle to make view ready for intention change as soon as skill is enabled again
                    Intention.Value = VerticalMovementIntention.Idle;
                    if (CanComboJump)
                    {
                        // Also reset combo jumps executed
                        ComboJumpsExecuted.Value = 0;    
                    }                    
                }
            };
        }
        #region private functions
        /// <summary>
        /// Sets the state of the movement based on intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetMovementState(VerticalMovementIntention intention)
        {
            switch (intention)
            {
                case VerticalMovementIntention.None:
                    break;
                case VerticalMovementIntention.Idle:
                    TransitionToStateIdle();
                    break;
                case VerticalMovementIntention.Jump:
                    TransitionToStateJump();
                    break;
                case VerticalMovementIntention.WallJump:
                    TransitionToStateWallJump();
                    break;
                case VerticalMovementIntention.Fall:
                    TransitionToStateFall();
                    break;
                case VerticalMovementIntention.Land:
                    TransitionToStateLand();
                    break;
            }
        }
        #endregion

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            if (VerticalMovementCurrentState.Value != VerticalMovementState.Landing) return;
            VerticalMovementCurrentState.Value = VerticalMovementState.Idle;
            OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = false;
        }

        /// <summary>
        /// Transitions to state land.
        /// </summary>
        public void TransitionToStateLand()
        {
            if (VerticalMovementCurrentState.Value == VerticalMovementState.Falling
                ||
                VerticalMovementCurrentState.Value == VerticalMovementState.Jumping
                ||
                VerticalMovementCurrentState.Value == VerticalMovementState.WallJumping)
            {
                VerticalMovementCurrentState.Value = VerticalMovementState.Landing;
                OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = false;
            }
        }

        /// <summary>
        /// Transitions to state jump.
        /// </summary>
        public void TransitionToStateJump()
        {
            if (VerticalMovementCurrentState.Value == VerticalMovementState.Idle
                ||
                VerticalMovementCurrentState.Value == VerticalMovementState.Landing)
            {
                VerticalMovementCurrentState.Value = VerticalMovementState.Jumping;
                ApplyResourceCost(OwnerMovementSkills.Value.OwnerCharacter.Value);
                OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = true;
            }
        }

        /// <summary>
        /// Transitions to state fall.
        /// </summary>
        public void TransitionToStateFall()
        {
            // Check to see that character is not preparing a wall jump
            //Debug.Log(_preparingWallJump);
            //if (!_preparingWallJump)
            //{
            
            VerticalMovementCurrentState.Value = VerticalMovementState.Falling;
            OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = true;    
            //}
            
        }

        /// <summary>
        /// Transitions to state wall jump.
        /// </summary>
        public void TransitionToStateWallJump()
        {
            if (VerticalMovementCurrentState.Value == VerticalMovementState.Jumping
                ||
                VerticalMovementCurrentState.Value == VerticalMovementState.Falling)
            {
                // TODO This seems to be unnecessary. Keep until certain
                //_preparingWallJump = true; // To prevent a possible immediate transition to fall if trying to wall jump exactly when velocity gets negative
                //var wallJumpPrepareTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Prepare walljump timer");
                //wallJumpPrepareTimer.TimerMethod = () =>
                //{
                //    Debug.Log("setting false");
                //    _preparingWallJump = false;
                //};
                //wallJumpPrepareTimer.Invoke(0f);

                VerticalMovementCurrentState.Value = VerticalMovementState.WallJumping;
                ApplyResourceCost(OwnerMovementSkills.Value.OwnerCharacter.Value);
                OwnerMovementSkills.Value.OwnerCharacter.Value.VerticalVelocityLocked.Value = true;

            }
        }
        #endregion

        #region public functions
        /// <summary>
        /// Sets the current jump speed.
        /// </summary>
        /// <param name="currentJumpSpeed">The current jump speed.</param>
        public void SetCurrentJumpSpeed(float currentJumpSpeed)
        {
            CurrentJumpSpeed.Value = currentJumpSpeed;
        }
        #endregion
    }
}
