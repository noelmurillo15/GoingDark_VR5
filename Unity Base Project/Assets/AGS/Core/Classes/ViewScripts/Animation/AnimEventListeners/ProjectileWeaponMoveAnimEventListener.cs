using AGS.Core.Systems.WeaponSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// AnimEventListeners should be used to recieve events raised by mecanim animations.
    /// Used as a bridge to connect animations with GameView
    /// </summary>
    public class ProjectileWeaponMoveAnimEventListener : CombatSkillAnimEventListenerBase
    {
        /// <summary>
        /// Call this functions from animation event.
        /// </summary>
        public void Fire()
        {
            var currentProjectileWeapon = CombatEntityBase.CurrentWeapon.Value as ProjectileWeapon;
            if (currentProjectileWeapon == null) return;
            currentProjectileWeapon.FireProjectile();
        }
    }
}
