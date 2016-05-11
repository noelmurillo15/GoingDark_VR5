using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    public class EquipableWeaponBase : WeaponBase
    {

        #region Properties
        // Constructor properties        
        public float Range { get; private set; }
        public CombatMoveSetType CombatMoveSetType { get; private set; }
        public Transform WeaponGripLeftHand { get; private set; } 
        public Transform WeaponGripRightHand { get; private set; }

        // Subscribable properties
        public ActionProperty<bool> Enabled { get; private set; } // Determines if the weapons is usable at this moment
        public ActionProperty<HitVolume> CurrentHitVolume { get; private set; } // The hit volume that currently have its collider activated
        public ActionList<HitVolume> HitVolumes { get; private set; } // All owned hitvolumes
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipableWeaponBase" /> class.
        /// </summary>
        /// <param name="transform">The Weapons transform.</param>
        /// <param name="animationBasedFiring">if set to <c>true</c> [animation based firing].</param>
        /// <param name="range">The weapon range.</param>
        /// <param name="combatMoveSetType">Type of the combat move set.</param>
        /// <param name="weaponGripLeft">Reference to the avatars left hand. For use with IK animation.</param>
        /// <param name="weaponGripRight">Reference to the avatars right hand. For use with IK animation.</param>
        public EquipableWeaponBase(Transform transform, bool animationBasedFiring, float range, CombatMoveSetType combatMoveSetType, Transform weaponGripLeft, Transform weaponGripRight)
            : base(transform, animationBasedFiring)
        {
            Range = range;
            CombatMoveSetType = combatMoveSetType;
            WeaponGripLeftHand = weaponGripLeft;
            WeaponGripRightHand = weaponGripRight;
            Enabled = new ActionProperty<bool>() { Value = true };
            CurrentHitVolume = new ActionProperty<HitVolume>();
            HitVolumes = new ActionList<HitVolume>();
            HitVolumes.ListItemAdded += HitVolumeAdded;
        }

        #region private functions
        /// <summary>
        /// ListItem notification. HitVolume was added
        /// </summary>
        /// <param name="hitVolumeAdd">The hit volume add.</param>
        private void HitVolumeAdded(HitVolume hitVolumeAdd)
        {
            hitVolumeAdd.OwnerEquipableWeapon.Value = this;
        }

        /// <summary>
        /// Enables this weapon.
        /// </summary>
        public void Enable()
        {
            Enabled.Value = true;
        }

        /// <summary>
        /// Disables this weapon.
        /// </summary>
        public void Disable()
        {
            Enabled.Value = false;
        }
        #endregion
    }
}
