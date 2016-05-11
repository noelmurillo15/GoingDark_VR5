using System;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// MeleeWeaponView is simply an EquipableWeaponBaseView that can be instaniated
    /// </summary>
    [Serializable]
    public class MeleeWeaponView : EquipableWeaponBaseView
    {
        public MeleeWeapon MeleeWeapon;

        #region AGS Setup
        public override void InitializeView()
        {
            MeleeWeapon = new MeleeWeapon(transform, AnimationBasedFiring, Range, CombatMoveSetType, WeaponGripLeftHand, WeaponGripRightHand);
            SolveModelDependencies(MeleeWeapon);
        }
        #endregion
    }
}