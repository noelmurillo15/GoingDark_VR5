using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement;
using AGS.Core.Systems.MovementSystem.MovementSkills.Sliding;
using AGS.Core.Systems.MovementSystem.MovementSkills.Swimming;
using AGS.Core.Systems.MovementSystem.MovementSkills.VerticalMovement;

namespace AGS.Core.Systems.MovementSystem.Base
{
    /// <summary>
    /// A bundle of all available movement skills.
    /// </summary>
    public class MovementSkills : ActionModel
    {
        #region Properties

        // Subscribable properties
        public ActionProperty<CharacterBase> OwnerCharacter; // Reference to owner character
        public ActionProperty<HorizontalMovement> HorizontalMovement { get; set; }
        public ActionProperty<VerticalMovement> VerticalMovement { get; set; }
        public ActionProperty<Swimming> Swimming { get; set; }
        public ActionProperty<Sliding> Sliding { get; set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementSkills"/> class.
        /// </summary>
        public MovementSkills()
        {
            OwnerCharacter = new ActionProperty<CharacterBase>();
            HorizontalMovement = new ActionProperty<HorizontalMovement>();
            HorizontalMovement.OnValueChanged += (sender, movementSkill) => SetParent(movementSkill.Value);
            VerticalMovement = new ActionProperty<VerticalMovement>();
            VerticalMovement.OnValueChanged += (sender, movementSkill) => SetParent(movementSkill.Value);
            Swimming = new ActionProperty<Swimming>();
            Swimming.OnValueChanged += (sender, movementSkill) => SetParent(movementSkill.Value);
            Sliding = new ActionProperty<Sliding>();
            Sliding.OnValueChanged += (sender, movementSkill) => SetParent(movementSkill.Value);
        }

        /// <summary>
        /// Sets the parent of the movement skill.
        /// </summary>
        /// <param name="movementSkill">The movement skill.</param>
        private void SetParent(MovementSkillBase movementSkill)
        {
            movementSkill.OwnerMovementSkills.Value = this;
        }
    }
}
