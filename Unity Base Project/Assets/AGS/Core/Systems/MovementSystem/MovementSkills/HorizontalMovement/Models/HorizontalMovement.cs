using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.Base;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement
{
    /// <summary>
    /// HorizontalMovement is meant to be used with any type of horizontal movement
    /// </summary>
    public class HorizontalMovement : MovementSkillBase
    {
        #region Properties
        // Constructor properties
        public float SprintSpeed { get; private set; }
        public float CrouchSpeed { get; private set; }
        public float SneakSpeed { get; private set; }
        public float StrafeSpeed { get; private set; }
        public float CrouchRelativeHeight { get; private set; }

        // Subscribable properties
        public ActionProperty<float> CurrentSpeed { get; private set; } // The characters current speed, based on horizontal movement state and any speed effects
        public ActionProperty<bool> CanSprint { get; private set; } // Can the character sprint at this moment?
        public ActionProperty<HorizontalMovementState> HorizontalMovementCurrentState { get; private set; } // horizontal movement state machine. Partially dependent on Intention
        public ActionProperty<HorizontalMovementIntention> Intention { get; private set; } // The intention value handles the characters "intention". It could, but is not required to, change the HorizontalMovementCurrentState
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalMovement"/> class.
        /// </summary>
        /// <param name="sprintSpeed">The characters sprint speed.</param>
        /// <param name="crouchSpeed">The characters crouch speed.</param>
        /// <param name="sneakSpeed">The characters sneak speed.</param>
        /// <param name="crouchRelativeHeight">Character height multiplier when crouching.</param>
        public HorizontalMovement(float sprintSpeed, float crouchSpeed, float sneakSpeed, float strafeSpeed, float crouchRelativeHeight)
        {
            CurrentSpeed = new ActionProperty<float>();
            SprintSpeed = sprintSpeed;
            CrouchSpeed = crouchSpeed;
            SneakSpeed = sneakSpeed;
            StrafeSpeed = strafeSpeed;
            CanSprint = new ActionProperty<bool>() { Value = true };
            CrouchRelativeHeight = crouchRelativeHeight;
            HorizontalMovementCurrentState = new ActionProperty<HorizontalMovementState>() { Value = HorizontalMovementState.Idle };
            Intention = new ActionProperty<HorizontalMovementIntention>() { Value = HorizontalMovementIntention.Idle };
            Intention.OnValueChanged += (sender, intention) => SetMovementState(intention.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to idle to make view ready for intentin change as soon as skill is enabled again
                    Intention.Value = HorizontalMovementIntention.Idle;                    
                }
            };
        }


        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            HorizontalMovementCurrentState.Value = HorizontalMovementState.Idle;
        }

        /// <summary>
        /// Transitions to state move.
        /// </summary>
        public void TransitionToStateMove()
        {
            HorizontalMovementCurrentState.Value = HorizontalMovementState.Moving;
        }

        /// <summary>
        /// Transitions to state crouch.
        /// </summary>
        public void TransitionToStateCrouch()
        {
            if (HorizontalMovementCurrentState.Value != HorizontalMovementState.Sneaking)
            {
                HorizontalMovementCurrentState.Value = HorizontalMovementState.Crouching;    
            }
            
        }

        /// <summary>
        /// Transitions to state sprint.
        /// </summary>
        public void TransitionToStateSprint()
        {
            if (HorizontalMovementCurrentState.Value != HorizontalMovementState.Sneaking
                &&
                HorizontalMovementCurrentState.Value != HorizontalMovementState.Crouching)
            {
                HorizontalMovementCurrentState.Value = HorizontalMovementState.Sprinting;
            }
        }

        /// <summary>
        /// Transitions to state sneak.
        /// </summary>
        public void TransitionToStateSneak()
        {
            if (HorizontalMovementCurrentState.Value != HorizontalMovementState.Sprinting
                &&
                HorizontalMovementCurrentState.Value != HorizontalMovementState.Crouching)
            {
                HorizontalMovementCurrentState.Value = HorizontalMovementState.Sneaking;
            }
        }

        /// <summary>
        /// Transitions to state strafe.
        /// </summary>
        public void TransitionToStateStrafe()
        {
            HorizontalMovementCurrentState.Value = HorizontalMovementState.Strafing;
        }
        #endregion

        #region private functions
        /// <summary>
        /// Sets the state of the movement based on intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetMovementState(HorizontalMovementIntention intention)
        {
            switch (intention)
            {
                case HorizontalMovementIntention.Idle:
                    TransitionToStateIdle();
                    break;
                case HorizontalMovementIntention.Sprint:
                    TransitionToStateSprint();
                    break;
                case HorizontalMovementIntention.Sneak:
                    TransitionToStateSneak();
                    break;
                case HorizontalMovementIntention.Crouch:
                    TransitionToStateCrouch();
                    break;
                case HorizontalMovementIntention.Move:
                    TransitionToStateMove();
                    break;
                case HorizontalMovementIntention.Strafe:
                    TransitionToStateStrafe();
                    break;
            }
        }

        #endregion


    }
}
