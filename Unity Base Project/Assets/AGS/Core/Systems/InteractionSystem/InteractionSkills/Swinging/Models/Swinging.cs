using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.Swinging
{
    /// <summary>
    /// Main workload of the Swinging model is to provide the swinging state machine and handle the swinging intention of the character.
    /// </summary>
    public class Swinging : InteractionSkillBase
    {
        #region Properties
        // Constructor properties
        public float ClimbSpeed { get; private set; }
        public float ExitMargin { get; private set; }
        public Vector3 JumpSpeed { get; private set; }

        // Subscribable properties
        public ActionProperty<bool> OnTopOfSwing { get; private set; } // Set to true when interacting with the first SwingUnit
        public ActionProperty<bool> OnEndOfSwing { get; private set; } // Set to true when interacting with the last SwingUnit
        public ActionProperty<SwingingState> SwingingCurrentState { get; private set; } // Swinging state machine. Partially dependent on Intention
        public ActionProperty<SwingingStateIntention> Intention { get; private set; } // The intention value handles the characters "intention". It could, but is not required to, change the current swinging state

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Swinging"/> class.
        /// </summary>
        /// <param name="approachMargin">The approach margin.</param>
        /// <param name="offsetVertical">The offset vertical.</param>
        /// <param name="offsetHorizontal">The offset horizontal.</param>
        /// <param name="climbSpeed">The climb speed.</param>
        /// <param name="exitMargin">The exit margin.</param>
        /// <param name="jumpSpeed">The jump speed.</param>
        public Swinging(float approachMargin, float offsetVertical, float offsetHorizontal, float climbSpeed, float exitMargin, Vector3 jumpSpeed)
            : base(approachMargin, offsetVertical, offsetHorizontal)
        {
            ClimbSpeed = climbSpeed;
            ExitMargin = exitMargin;
            JumpSpeed = jumpSpeed;
            OnTopOfSwing = new ActionProperty<bool>() { Value = false };
            OnEndOfSwing = new ActionProperty<bool>() { Value = false };
            SwingingCurrentState = new ActionProperty<SwingingState>();
            Intention = new ActionProperty<SwingingStateIntention>();
            Intention.OnValueChanged += (sender, intention) => SetSwingingState(intention.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to none to make view ready for intentin change as soon as skill is enabled again
                    Intention.Value = SwingingStateIntention.None;
                }
            };
        }

        #region private functions
        /// <summary>
        /// Sets the state of the swinging based on intention
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetSwingingState(SwingingStateIntention intention)
        {
            switch (intention)
            {
                case SwingingStateIntention.Climb:
                    TransitionToStateClimb();
                    break;
                case SwingingStateIntention.Release:
                    TransitionToStateRelease();
                    break;
                case SwingingStateIntention.Jump:
                    TransitionToStateJump();
                    break;
                case SwingingStateIntention.Swing:
                    TransitionToStateSwing();
                    break;
                case SwingingStateIntention.StopSwing:
                    TransitionToStateStopSwing();
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
            if (SwingingCurrentState.Value == SwingingState.Releasing
                ||
                SwingingCurrentState.Value == SwingingState.Jumping)
            {
                SwingingCurrentState.Value = SwingingState.Idle;    
            }
            
        }

        /// <summary>
        /// Transitions to state approach.
        /// </summary>
        public override void TransitionToStateApproach()
        {
            if (SwingingCurrentState.Value == SwingingState.Idle)
            {
                SwingingCurrentState.Value = SwingingState.Approaching;
            }
        }

        /// <summary>
        /// Transitions to state interact.
        /// </summary>
        public override void TransitionToStateInteract()
        {
            if (SwingingCurrentState.Value == SwingingState.Approaching)
            {
                SwingingCurrentState.Value = SwingingState.Swinging;
            }
        }

        /// <summary>
        /// Transitions to state release.
        /// </summary>
        public override void TransitionToStateRelease()
        {
            if (SwingingCurrentState.Value != SwingingState.Idle)
            {
                SwingingCurrentState.Value = SwingingState.Releasing;
            }
        }

        /// <summary>
        /// Transitions to state climb.
        /// </summary>
        private void TransitionToStateClimb()
        {
            if (SwingingCurrentState.Value == SwingingState.Swinging
                ||
                SwingingCurrentState.Value == SwingingState.StopSwinging)
            {
                SwingingCurrentState.Value = SwingingState.Climbing;
            }
        }

        /// <summary>
        /// Transitions to state jump.
        /// </summary>
        private void TransitionToStateJump()
        {
            if (SwingingCurrentState.Value == SwingingState.Swinging
                ||
                SwingingCurrentState.Value == SwingingState.Climbing
                ||
                SwingingCurrentState.Value == SwingingState.StopSwinging)
            {
                SwingingCurrentState.Value = SwingingState.Jumping;
            }
        }

        /// <summary>
        /// Transitions to state swing.
        /// </summary>
        private void TransitionToStateSwing()
        {
            if (SwingingCurrentState.Value == SwingingState.Climbing
                ||
                SwingingCurrentState.Value == SwingingState.StopSwinging)
            {
                SwingingCurrentState.Value = SwingingState.Swinging;
            }
        }

        /// <summary>
        /// Transitions to state stop swing.
        /// </summary>
        private void TransitionToStateStopSwing()
        {
            if (SwingingCurrentState.Value == SwingingState.Climbing
                ||
                SwingingCurrentState.Value == SwingingState.Swinging)
            {
                SwingingCurrentState.Value = SwingingState.StopSwinging;
            }
        }
        #endregion
    }
}
