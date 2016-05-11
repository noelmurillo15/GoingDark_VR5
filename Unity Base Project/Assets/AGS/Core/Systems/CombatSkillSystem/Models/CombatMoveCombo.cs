using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// This model holds a Combo skill sequence reference. Names should match CombatMove SkillName's
    /// </summary>
    public class CombatMoveCombo : ActionModel
    {
        public string[] SkillSequence;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatMoveCombo"/> class.
        /// </summary>
        /// <param name="skillSequence">The skill sequence.</param>
        public CombatMoveCombo(string[] skillSequence)
        {
            SkillSequence = skillSequence;
        }
    }
}
