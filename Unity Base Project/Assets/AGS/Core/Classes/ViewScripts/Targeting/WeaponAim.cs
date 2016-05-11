using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AGS.Core.Systems.WeaponSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles projectile weapon aiming. Weapons must have the muzzle pointing forward in the z-axis direction.
    /// </summary>
    public class WeaponAim : ViewScriptBase
    {
        private Animator _animator;
        protected ProjectileWeaponView ProjectileWeaponView;
        protected ProjectileWeapon ProjectileWeapon;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                ProjectileWeaponView = ViewReference as ProjectileWeaponView;
                if (ProjectileWeaponView == null) return;

                ProjectileWeapon = ProjectileWeaponView.ProjectileWeapon;

            }
            if (ProjectileWeapon == null) return;

        }

        public override void Update()
        {
            base.Update();
            if (ProjectileWeapon.OwnerCombatEntity.Value.Aiming.Value == null) return;
            if (ProjectileWeapon.OwnerCombatEntity.Value.Aiming.Value.AimingCurrentState.Value == Enums.AimingStateMachineState.Idle) return;
            transform.LookAt(ProjectileWeapon.OwnerCombatEntity.Value.Aiming.Value.AimTarget.Value);
        }

    }
}
