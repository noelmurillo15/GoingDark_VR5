using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// MeleeWeapon is a straight implementation of an equipable weapon
    /// </summary>
    public class MeleeWeapon : EquipableWeaponBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeleeWeapon"/> class.
        /// </summary>
        /// <param name="transform">The Weapons transform.</param>
        /// <param name="range">The weapon range.</param>
        /// <param name="combatMoveSetType">Type of the combat move set.</param>
        /// <param name="weaponGripLeft">Reference to the avatars left hand. For use with IK animation.</param>
        /// <param name="weaponGripRight">Reference to the avatars right hand. For use with IK animation.</param>
        public MeleeWeapon(Transform transform, bool animationBasedFiring, float range, CombatMoveSetType combatMoveSetType, Transform weaponGripLeft, Transform weaponGripRight)
            : base(transform, animationBasedFiring, range, combatMoveSetType, weaponGripLeft, weaponGripRight)
        {

        }
    }
}
