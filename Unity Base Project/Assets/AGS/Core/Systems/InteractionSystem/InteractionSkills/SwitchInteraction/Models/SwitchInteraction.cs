using System.Security.Cryptography;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.InteractionSystem.Base;

namespace AGS.Core.Systems.InteractionSystem.InteractionSkills.SwitchInteraction
{
    /// <summary>
    /// Main workload of the SwitchInteraction model is to provide the switch interaction state machine
    /// </summary>
    public class SwitchInteraction : InteractionSkillBase
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<SwitchInteractionState> SwitchInteractionCurrentState { get; private set; } // Current switch interaction state

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchInteraction"/> class.
        /// </summary>
        /// <param name="approachMargin">Margin for determining when the characters has come close enough to grab a target.</param>
        /// <param name="offsetVertical">Offset for grabbing position.</param>
        /// <param name="offsetHorizontal">Offset for grabbing position.</param>
        public SwitchInteraction(float approachMargin, float offsetVertical, float offsetHorizontal)
            : base(approachMargin, offsetVertical, offsetHorizontal)
        {
            SwitchInteractionCurrentState = new ActionProperty<SwitchInteractionState>();
        }

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public override void TransitionToStateIdle()
        {
            if (SwitchInteractionCurrentState.Value == SwitchInteractionState.Switching)
            {
                SwitchInteractionCurrentState.Value = SwitchInteractionState.Idle;
            }
        }

        /// <summary>
        /// Transitions to state approach.
        /// </summary>
        public override void TransitionToStateApproach()
        {
            if (SwitchInteractionCurrentState.Value == SwitchInteractionState.Idle)
            {
                SwitchInteractionCurrentState.Value = SwitchInteractionState.Approaching;
            }

        }

        /// <summary>
        /// Transitions to state interact.
        /// </summary>
        public override void TransitionToStateInteract()
        {
            if (SwitchInteractionCurrentState.Value == SwitchInteractionState.Approaching)
            {
                SwitchInteractionCurrentState.Value = SwitchInteractionState.Switching;
            }
        }

        /// <summary>
        /// Transitions to state release.
        /// </summary>
        public override void TransitionToStateRelease()
        {
            if (SwitchInteractionCurrentState.Value == SwitchInteractionState.Switching)
            {
                SwitchInteractionCurrentState.Value = SwitchInteractionState.Idle;
                OwnerInteractionSkills.Value.ForceClear();
            }

        }

        #endregion
    }
}
