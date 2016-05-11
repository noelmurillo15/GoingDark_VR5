using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.LedgeClimbing
{
    /// <summary>
    /// Main workload of the LedgeClimbing model is to provide the ledge climbing state machine and handle the ledge climbing intention of the character
    /// </summary>
    public class LedgeClimbing : InteractionSkillBase
    {
        #region Properties
        // Constructor properties
        public float ClimbOffsetHorizontal { get; private set; }
        public float ClimbOffsetVertical { get; private set; }
        public float ExitMargin { get; private set; }
        public float LedgeClimbSpeed { get; private set; }
        public float LedgeJumpSpeedVertical { get; private set; }
        public float LedgeJumpSpeedHorizontal { get; private set; }

        // Subscribable properties
        public ActionProperty<LedgeClimbingState> LedgeClimbingCurrentState { get; private set; } // ledge climbing state machine. Partially dependent on Intention
        public ActionProperty<LedgeClimbingIntention> Intention { get; private set; } // The intention value handles the characters "intention". It could, but is not required to, change the LedgeClimbingCurrentState

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="LedgeClimbing" /> class.
        /// </summary>
        /// <param name="approachMargin">The approach margin.</param>
        /// <param name="offsetVertical">The offset vertical.</param>
        /// <param name="offsetHorizontal">The offset horizontal.</param>
        /// <param name="climbOffsetHorizontal">Offset for when exiting a ledge by climbing.</param>
        /// <param name="climbOffsetVertical">Offset for when exiting a ledge by climbing.</param>
        /// <param name="exitMargin">Margin for when a climbing exit has been reached.</param>
        /// <param name="ledgeClimbSpeed">The ledge climb speed.</param>
        /// <param name="ledgeJumpSpeedVertical">The ledge jump speed vertical.</param>
        /// <param name="ledgeJumpSpeedHorizontal">The ledge jump speed horizontal.</param>
        public LedgeClimbing(float approachMargin, float offsetVertical, float offsetHorizontal, float climbOffsetHorizontal, float climbOffsetVertical, float exitMargin, float ledgeClimbSpeed, float ledgeJumpSpeedVertical, float ledgeJumpSpeedHorizontal)
            : base(approachMargin, offsetVertical, offsetHorizontal)
        {
            ClimbOffsetHorizontal = climbOffsetHorizontal;
            ClimbOffsetVertical = climbOffsetVertical;
            ExitMargin = exitMargin;
            LedgeClimbSpeed = ledgeClimbSpeed;
            LedgeJumpSpeedVertical = ledgeJumpSpeedVertical;
            LedgeJumpSpeedHorizontal = ledgeJumpSpeedHorizontal;
            LedgeClimbingCurrentState = new ActionProperty<LedgeClimbingState>();
            Intention = new ActionProperty<LedgeClimbingIntention>();
            Intention.OnValueChanged += (sender, intention) => SetLedgeClimbingState(intention.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to none to make view ready for intention change as soon as skill is enabled again
                    Intention.Value = LedgeClimbingIntention.None;
                }
            };
        }

        #region private functions
        /// <summary>
        /// Sets the ledge climbing state based on intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetLedgeClimbingState(LedgeClimbingIntention intention)
        {
            switch (intention)
            {
                case LedgeClimbingIntention.Climb:
                    TransitionToStateClimb();
                    break;
                case LedgeClimbingIntention.Release:
                    TransitionToStateRelease();
                    break;
                case LedgeClimbingIntention.Jump:
                    TransitionToStateJump();
                    break;
            }
        }
        #endregion

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public override void TransitionToStateIdle()
        {
            LedgeClimbingCurrentState.Value = LedgeClimbingState.Idle;
        }

        /// <summary>
        /// Transitions to state approach.
        /// </summary>
        public override void TransitionToStateApproach()
        {
            if (LedgeClimbingCurrentState.Value == LedgeClimbingState.Idle)
            {
                LedgeClimbingCurrentState.Value = LedgeClimbingState.Approaching;    
            }
        }

        /// <summary>
        /// Transitions to state interact.
        /// </summary>
        public override void TransitionToStateInteract()
        {
            if (LedgeClimbingCurrentState.Value == LedgeClimbingState.Approaching)
            {
                LedgeClimbingCurrentState.Value = LedgeClimbingState.Grabbing;
            }
        }

        /// <summary>
        /// Transitions to state release.
        /// </summary>
        public override void TransitionToStateRelease()
        {
            if (LedgeClimbingCurrentState.Value == LedgeClimbingState.Grabbing
                ||
                LedgeClimbingCurrentState.Value == LedgeClimbingState.Climbing
                ||
                LedgeClimbingCurrentState.Value == LedgeClimbingState.Jumping)
            {
                LedgeClimbingCurrentState.Value = LedgeClimbingState.Releasing;
            }
        }

        /// <summary>
        /// Transitions to state climb.
        /// </summary>
        public void TransitionToStateClimb()
        {
            if (LedgeClimbingCurrentState.Value == LedgeClimbingState.Grabbing)
            {
                LedgeClimbingCurrentState.Value = LedgeClimbingState.Climbing;
            }
        }

        /// <summary>
        /// Transitions to state jump.
        /// </summary>
        public void TransitionToStateJump()
        {
            if (LedgeClimbingCurrentState.Value == LedgeClimbingState.Grabbing)
            {
                LedgeClimbingCurrentState.Value = LedgeClimbingState.Jumping;
            } 
        }
        #endregion
    }
}
