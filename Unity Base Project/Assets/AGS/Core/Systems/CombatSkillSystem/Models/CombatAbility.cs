using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.StatusEffectSystem;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// Not implemented yet. Meant to be used for non-weapon based combat skills.
    /// </summary>
    public class CombatAbility : CombatSkillBase
    {

        #region Properties
        // Constructor properties

        // Subscribable properties
        public ActionProperty<AreaOfEffect> AreaOfEffect { get; private set; } // Reference to possible AOE of this ability
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatAbility"/> class.
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <param name="needActiveTarget">if set to <c>true</c> [need active target].</param>
        /// <param name="secondsCharging">Determines how many seconds does it take to charge this skill</param>
        /// <param name="secondsRecharging">Determines how many seconds does it take to recharge this skill</param>
        /// <param name="secondsFiring">Determines how many seconds does it take to fire this skill</param>
        /// <param name="combatSkillFireType">FireType of the combat skill.</param>
        /// <param name="combatSkillAttackType">AttackType of the combat skill.</param>
        public CombatAbility(string skillName, bool needActiveTarget, float secondsCharging, float secondsRecharging, float secondsFiring, CombatSkillFireType combatSkillFireType, CombatSkillAttackType combatSkillAttackType) : base(skillName, needActiveTarget, secondsCharging, secondsRecharging, secondsFiring, combatSkillFireType, combatSkillAttackType)
        {
            AreaOfEffect = new ActionProperty<AreaOfEffect>();
        }
    }
}
