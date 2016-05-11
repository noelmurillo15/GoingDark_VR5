using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// A HitVolume is owned by a weapon and can hit a target when it is activated during a combat move
    /// </summary>
    public class HitVolume : ActionModel
    {
        #region Properties
        // Constructor properties
        public Transform Transform { get; private set; }
        public HitVolumeIndex HitVolumeIndex { get; private set; }
        
        // Subscribable properties
        public ActionProperty<EquipableWeaponBase> OwnerEquipableWeapon;
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="HitVolume"/> class.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="hitVolumeIndex">Index of the hit volume. For weapons with several hitvolumes, the active combat move determines which index should be active.</param>
        public HitVolume(Transform transform, HitVolumeIndex hitVolumeIndex)
        {
            Transform = transform;
            HitVolumeIndex = hitVolumeIndex;
            OwnerEquipableWeapon = new ActionProperty<EquipableWeaponBase>();
        }
    }
}
