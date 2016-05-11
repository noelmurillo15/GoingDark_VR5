using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// CombatMoves are weapon based attack moves. They belong to a specific CombatMoveSet and can be chained to perform combat move combos.
    /// </summary>
    public class CombatMove : CombatSkillBase
    {
        #region Properties

        // Constructor properties
        public float ComboChainTimer { get; private set; } // 
        public CombatMoveType CombatMoveType { get; private set; }
        public CombatMovePosition RequiredPosition { get; private set; } // 
        public HitVolumeIndex HitVolumeIndex { get; private set; } // 

        // Subscribable properties
        public ActionProperty<CombatMoveSet> OwnerCombatMoveSet; // Reference to the owner combat move set
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatMove"/> class.
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <param name="needActiveTarget">if set to <c>true</c> [need active target].</param>
        /// <param name="secondsCharging">The seconds charging.</param>
        /// <param name="secondsRecharging">The seconds recharging.</param>
        /// <param name="secondsFiring">The seconds firing.</param>
        /// <param name="combatSkillFireType">Type of the combat skill fire.</param>
        /// <param name="combatSkillAttackType">Type of the combat skill attack.</param>
        /// <param name="comboChainTimer">Time window before combo chain opportunity expires</param>
        /// <param name="combatMoveType">Type of the combat move.</param>
        /// <param name="requiredPosition">CombatMoves are only enabled when OwnerCombatMoveSets OwnerCombatMoveEntity is in the correct position.</param>
        /// <param name="hitVolumeIndex">Equipable weapons can have multiple hitboxes. Match this index with the weapons hitbox index.</param>
        public CombatMove(string skillName, bool needActiveTarget, float secondsCharging, float secondsRecharging, float secondsFiring,
            CombatSkillFireType combatSkillFireType, CombatSkillAttackType combatSkillAttackType,
            float comboChainTimer, CombatMoveType combatMoveType, CombatMovePosition requiredPosition, HitVolumeIndex hitVolumeIndex)
            : base(skillName, needActiveTarget, secondsCharging, secondsRecharging, secondsFiring, combatSkillFireType, combatSkillAttackType)
        {
            OwnerCombatMoveSet = new ActionProperty<CombatMoveSet>();
            ComboChainTimer = comboChainTimer;
            CombatMoveType = combatMoveType;
            RequiredPosition = requiredPosition;
            HitVolumeIndex = hitVolumeIndex;
        }
    }
}
