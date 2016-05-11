using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// Base View for any weapon that can be carried and used with combat moves
    /// </summary>
    [Serializable]
    public abstract class EquipableWeaponBaseView : WeaponBaseView
    {
        #region Public properties
        // Fields to be se in the editor
        public float Range;
        public CombatMoveSetType CombatMoveSetType;

        // References to be set in the editor
        public Transform HitVolumesContainer; // Put any hit volumes for this weapon on separate child GameObjects to this Transform
        public Transform WeaponGripLeftHand; // For use with IK animation
        public Transform WeaponGripRightHand; // For use with IK animation
        #endregion

        public EquipableWeaponBase EquipableWeapon;
        private HitVolumeView[] _hitVolumeViews;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            EquipableWeapon = model as EquipableWeaponBase;
            if (EquipableWeapon == null) return;
            if (HitVolumesContainer != null)
            {
                _hitVolumeViews = HitVolumesContainer.GetComponentsInChildren<HitVolumeView>();
                foreach (var hitVolumeView in _hitVolumeViews)
                {
                    EquipableWeapon.HitVolumes.Add(hitVolumeView.HitVolume);
                }
            }

        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            if (EquipableWeapon.OwnerCombatEntity.Value == null)
            {
                var ownerCombatEntityView = GetComponentInParent<CombatEntityBaseView>();
                if (ownerCombatEntityView != null && ownerCombatEntityView.CombatEntity != null)
                {
                    if (!ownerCombatEntityView.CombatEntity.Weapons.Contains(EquipableWeapon))
                    {
                        ownerCombatEntityView.CombatEntity.Weapons.Add(EquipableWeapon);
                    }
                }
            }
            // Subscribe to the weapons enabled property to activate or deactivate it
            EquipableWeapon.Enabled.OnValueChanged += (sender, isEnabled) => WeaponSetActive(isEnabled.Value);
        }
        #endregion

        /// <summary>
        /// Activates this weapon.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> the weapon [is enabled].</param>
        private void WeaponSetActive(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
            if (_hitVolumeViews == null) return;
            foreach (var hitVolumeView in _hitVolumeViews)
            {
                hitVolumeView.gameObject.SetActive(isEnabled);
            }
        }

    }
}