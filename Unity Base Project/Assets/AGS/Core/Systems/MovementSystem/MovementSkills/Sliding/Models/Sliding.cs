using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.MovementSkills.Sliding
{
    /// <summary>
    /// Sliding skill is needed for both natural and manual sliding behaviours, especially for Rigidbody characters. Without sliding skill, Rigidbodies will slide down even on the slightest slope
    /// </summary>
    public class Sliding : MovementSkillBase
    {
        #region Properties
        // Constructor properties
        public float SlidingSpeed;
        public float SlopeLimitNaturalSliding;
        public float SlopeLimitHelplessSliding;
        public float MaxManualSlidingSpeed;
        public float MaxHelplessSlidingSpeed;

        // Subscribable properties
        public ActionProperty<Vector3> CurrentSlidingSpeed { get; set; } // The current sliding speed
        public ActionProperty<SlidingState> SlidingCurrentState { get; set; } // sliding state machine. Partially dependent on Intention
        public ActionProperty<SlidingIntention> Intention { get; set;  } // The intention value handles the characters "intention". It could, but is not required to, change the SlidingCurrentState
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Sliding"/> class.
        /// </summary>
        /// <param name="slidingSpeed">The sliding speed used for manual sliding.</param>
        /// <param name="slopeLimitNaturalSliding">Determines the ground angle when the character should start to slide naturally (i.e. at what angle should we stop prevent sliding).</param>
        /// <param name="slopeLimitHelplessSliding">Determines the ground angle when the character should start to slide helplessly.</param>
        /// <param name="maxManualSlidingSpeed">Max speed the character can reach when sliding.</param>
        /// <param name="maxHelplessSlidingSpeed">Max speed the character can reach when sliding helplessly.</param>
        public Sliding(float slidingSpeed, float slopeLimitNaturalSliding, float slopeLimitHelplessSliding, float maxManualSlidingSpeed, float maxHelplessSlidingSpeed)
        {
            SlidingSpeed = slidingSpeed;
            SlopeLimitNaturalSliding = slopeLimitNaturalSliding;
            SlopeLimitHelplessSliding = slopeLimitHelplessSliding;
            MaxManualSlidingSpeed = maxManualSlidingSpeed;
            MaxHelplessSlidingSpeed = maxHelplessSlidingSpeed;
            CurrentSlidingSpeed = new ActionProperty<Vector3>();
            SlidingCurrentState = new ActionProperty<SlidingState>();
            Intention = new ActionProperty<SlidingIntention>() { Value = SlidingIntention.None };
            Intention.OnValueChanged += (sender, intention) => SetSlidingState(intention.Value);
            IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                if (!isEnabled.Value)
                {
                    // Upon disable, set intention to none to make view ready for intentin change as soon as skill is enabled again
                    Intention.Value = SlidingIntention.None;
                }
            };
        }
        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            SlidingCurrentState.Value = SlidingState.Idle;
        }

        /// <summary>
        /// Transitions to state manual slide.
        /// </summary>
        public void TransitionToStateManualSlide()
        {
            if (SlidingCurrentState.Value == SlidingState.Idle
                || 
                SlidingCurrentState.Value == SlidingState.PreventSliding
                ||
                SlidingCurrentState.Value == SlidingState.NaturalSliding)
            {
                SlidingCurrentState.Value = SlidingState.ManualSliding;   
            }
        }

        /// <summary>
        /// Transitions to state natural slide.
        /// </summary>
        public void TransitionToStateNaturalSlide()
        {
            SlidingCurrentState.Value = SlidingState.NaturalSliding;
        }

        /// <summary>
        /// Transitions to state helpless slide.
        /// </summary>
        public void TransitionToStateHelplessSlide()
        {
            SlidingCurrentState.Value = SlidingState.HelplessSliding;
        }

        /// <summary>
        /// Transitions to state prevent slide.
        /// </summary>
        public void TransitionToStatePreventSlide()
        {
            SlidingCurrentState.Value = SlidingState.PreventSliding;
        }
        #endregion

        #region public functions
        /// <summary>
        /// Sets the current sliding speed.
        /// </summary>
        /// <param name="currentSlidingSpeed">The current sliding speed.</param>
        public void SetCurrentSlidingSpeed(Vector3 currentSlidingSpeed)
        {
            CurrentSlidingSpeed.Value = currentSlidingSpeed;
        }
        #endregion

        #region private functions
        /// <summary>
        /// Sets the state of the sliding based on intention.
        /// </summary>
        /// <param name="intention">The intention.</param>
        private void SetSlidingState(SlidingIntention intention)
        {
            switch (intention)
            {
                case SlidingIntention.None:
                    TransitionToStateIdle();
                    break;
                case SlidingIntention.ManualSlide:
                    TransitionToStateManualSlide();
                    break;
                case SlidingIntention.NaturalSlide:
                    TransitionToStateNaturalSlide();
                    break;
                case SlidingIntention.HelplessSlide:
                    TransitionToStateHelplessSlide();
                    break;
                case SlidingIntention.PreventSlide:
                    TransitionToStatePreventSlide();
                    break;
            }
        }
        #endregion functions

    }
}
