using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.SkillSystem;

namespace AGS.Core.Systems.MovementSystem.Base
{
    /// <summary>
    /// Base class for any movement skill
    /// </summary>
    public abstract class MovementSkillBase : SkillBase
    {
        #region Properties
        public ActionProperty<MovementSkills> OwnerMovementSkills; // Reference to owner movement skills

        // Subscribable properties
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementSkillBase"/> class.
        /// </summary>
        protected MovementSkillBase()
        {
            OwnerMovementSkills = new ActionProperty<MovementSkills>();
        }
    }
}
