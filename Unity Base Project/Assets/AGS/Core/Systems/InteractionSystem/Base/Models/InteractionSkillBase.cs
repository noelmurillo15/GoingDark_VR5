using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.SkillSystem;

namespace AGS.Core.Systems.InteractionSystem.Base
{
    /// <summary>
    /// Base class for all interaction skills. Inherits from SkillBase and thus recieves resource cost functionality and the enabled boolean.
    /// Rather than providing its own "interaction skill state machine", it requires childs to implement general state transitions that are shared by all interaction skills.
    /// Otherwise interaction skills would need another state machine for their specific states.
    /// </summary>
    public abstract class InteractionSkillBase : SkillBase
    {
        #region Properties
        // Constructor properties
        public float ApproachMargin { get; private set; }
        public float OffsetVertical { get; private set; }
        public float OffsetHorizontal { get; private set; }

        // Subscribable properties
        public ActionProperty<InteractionSkills> OwnerInteractionSkills;

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionSkillBase"/> class.
        /// </summary>
        /// <param name="approachMargin">Margin for determining when the characters has come close enough to grab a target.</param>
        /// <param name="offsetVertical">Offset for grabbing position.</param>
        /// <param name="offsetHorizontal">Offset for grabbing position.</param>
        protected InteractionSkillBase(float approachMargin, float offsetVertical, float offsetHorizontal)
        {
            ApproachMargin = approachMargin;
            OffsetVertical = offsetVertical;
            OffsetHorizontal = offsetHorizontal;
            OwnerInteractionSkills = new ActionProperty<InteractionSkills>();
        }

        #region state transitions
        public abstract void TransitionToStateIdle();
        public abstract void TransitionToStateApproach();
        public abstract void TransitionToStateInteract();
        public abstract void TransitionToStateRelease();
        #endregion
    }
}
