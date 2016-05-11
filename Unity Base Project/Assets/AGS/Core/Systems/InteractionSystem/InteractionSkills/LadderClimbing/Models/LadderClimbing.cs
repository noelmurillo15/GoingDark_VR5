using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.LadderClimbing
{
    /// <summary>
    /// Main workload of the LadderClimbing model is to provide the ladder climbing state machine
    /// </summary>
    public class LadderClimbing : InteractionSkillBase
    {
        #region Properties
        // Constructor properties
        public float ClimbOffsetVertical { get; private set; }
        public float ClimbOffsetHorizontal { get; private set; }
        public float ClimbSpeed { get; private set; }
        public float LadderJumpSpeedHorizontal { get; private set; }
        public float LadderJumpSpeedVertical { get; private set; }

        // Subscribable properties
        public ActionProperty<LadderClimbingState> LadderClimbingCurrentState { get; private set; } // Ladder climbing state machine

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="LadderClimbing" /> class.
        /// </summary>
        /// <param name="approachMargin">The approach margin.</param>
        /// <param name="offsetVertical">The offset vertical.</param>
        /// <param name="offsetHorizontal">The offset horizontal.</param>
        /// <param name="climbOffsetVertical">Offset for when exiting a ladder by climbing.</param>
        /// <param name="climbOffsetHorizontal">Offset for when exiting a ladder by climbing.</param>
        /// <param name="climbSpeed">The climb speed.</param>
        /// <param name="ladderJumpSpeedHorizontal">The ladder jump speed horizontal.</param>
        /// <param name="ladderJumpSpeedVertical">The ladder jump speed vertical.</param>
        public LadderClimbing(float approachMargin, float offsetVertical, float offsetHorizontal, float climbOffsetVertical, float climbOffsetHorizontal, float climbSpeed, float ladderJumpSpeedHorizontal, float ladderJumpSpeedVertical)
            : base(approachMargin, offsetVertical, offsetHorizontal)
        {
            ClimbOffsetVertical = climbOffsetVertical;
            ClimbOffsetHorizontal = climbOffsetHorizontal;
            ClimbSpeed = climbSpeed;
            LadderJumpSpeedHorizontal = ladderJumpSpeedHorizontal;
            LadderJumpSpeedVertical = ladderJumpSpeedVertical;
            LadderClimbingCurrentState = new ActionProperty<LadderClimbingState>() { Value = LadderClimbingState.Idle };
        }

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public override void TransitionToStateIdle()
        {
            if (LadderClimbingCurrentState.Value == LadderClimbingState.Releasing)
            {
                LadderClimbingCurrentState.Value = LadderClimbingState.Idle;
            }
        }

        /// <summary>
        /// Transitions to state approach.
        /// </summary>
        public override void TransitionToStateApproach()
        {
            if (LadderClimbingCurrentState.Value == LadderClimbingState.Idle)
            {
                LadderClimbingCurrentState.Value = LadderClimbingState.Approaching;
            }
            
        }

        /// <summary>
        /// Transitions to state interact.
        /// </summary>
        public override void TransitionToStateInteract()
        {
            if (LadderClimbingCurrentState.Value == LadderClimbingState.Approaching)
            {
                LadderClimbingCurrentState.Value = LadderClimbingState.Climbing;
            }
        }

        /// <summary>
        /// Transitions to state release.
        /// </summary>
        public override void TransitionToStateRelease()
        {
            if (LadderClimbingCurrentState.Value == LadderClimbingState.Climbing
                ||
                LadderClimbingCurrentState.Value == LadderClimbingState.Jumping
                ||
                LadderClimbingCurrentState.Value == LadderClimbingState.ExitingTop)
            {
                LadderClimbingCurrentState.Value = LadderClimbingState.Releasing;
            }
        }

        /// <summary>
        /// Transitions to state jump.
        /// </summary>
        public void TransitionToStateJump()
        {
            if (LadderClimbingCurrentState.Value == LadderClimbingState.Climbing)
            {
                LadderClimbingCurrentState.Value = LadderClimbingState.Jumping;
            }
        }

        /// <summary>
        /// Transitions to state exit top.
        /// </summary>
        public void TransitionToStateExitTop()
        {
            if (LadderClimbingCurrentState.Value == LadderClimbingState.Climbing)
            {
                LadderClimbingCurrentState.Value = LadderClimbingState.ExitingTop;
            }
        }
        #endregion
    }
}
