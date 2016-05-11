using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// ProjectileWeapons can both fire projectiles, and be used to strike with
    /// </summary>
    public class ProjectileWeapon : EquipableWeaponBase
    {
         #region Properties
        // Constructor properties
        public ProjectileType LoadedProjectileType { get; private set; }
        public ProjectileSubType LoadedProjectileSubType { get; private set; }
        public Transform ProjectileSpawnPosition { get; private set; }
        public float ProjectileSpeed { get; private set; }
        
        // Subscribable properties
        public ActionList<Projectile> FiredProjectiles { get; private set; } // Temporary list that tracks fired projectiles
        
        public Action<Projectile> ProjectileFiredAction { get; set; } // Subscribe to this action to get notified that this weapon fired a projectile
        public Action<ProjectileHitData> ProjectileHitAction { get; set; } // Subscribe to this action to get notified that this weapon hit something with a projectile
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectileWeapon" /> class.
        /// </summary>
        /// <param name="transform">The Weapons transform.</param>
        /// <param name="animationBasedFiring">if set to <c>true</c> [animation based firing].</param>
        /// <param name="range">The weapon range.</param>
        /// <param name="combatMoveSetType">Type of the combat move set.</param>
        /// <param name="weaponGripLeft">Reference to the avatars left hand. For use with IK animation.</param>
        /// <param name="weaponGripRight">Reference to the avatars right hand. For use with IK animation.</param>
        /// <param name="spawnPosition">The spawn position.</param>
        /// <param name="projectileSpeed">The projectile speed.</param>
        /// <param name="initialProjectileType">Initial type of the projectile.</param>
        /// <param name="initialProjectileSubType">Initial sub type of the projectilee.</param>
        public ProjectileWeapon(Transform transform, bool animationBasedFiring, float range, CombatMoveSetType combatMoveSetType, Transform weaponGripLeft, Transform weaponGripRight, Transform spawnPosition, float projectileSpeed, ProjectileType initialProjectileType, ProjectileSubType initialProjectileSubType)
            : base(transform, animationBasedFiring, range, combatMoveSetType, weaponGripLeft, weaponGripRight)
        {
            ProjectileSpawnPosition = spawnPosition;
            ProjectileSpeed = projectileSpeed;
            LoadedProjectileType = initialProjectileType;
            LoadedProjectileSubType = initialProjectileSubType;
            FiredProjectiles = new ActionList<Projectile>();
            FiredProjectiles.ListItemAdded += OnProjectileAdded;
        }

        #region protected functions
        /// <summary>
        /// Fires the weapon.
        /// </summary>
        public override void FireWeapon()
        {
            base.FireWeapon();
            if (AnimationBasedFiring) return; // Ignore firing if weapon is set to fire from animation triggers
            FireProjectile();
        }

        /// <summary>
        /// ListItem notification. Projectile was added.
        /// </summary>
        /// <param name="projectile">The projectile.</param>
        protected void OnProjectileAdded(Projectile projectile)
        {
            projectile.OwnerProjectileWeapon.Value = this;
            projectile.SpawningCombatMoveRef.Value = OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove.Value;
            projectile.ProjectileHitKillableAction += (damageable, hittingFromBehind) => ProjectileHitKillableEvent(projectile, damageable, hittingFromBehind);
            projectile.ProjectileHitMovableAction += (movable, hittingFromBehind) => ProjectileHitMovableEvent(projectile, movable, hittingFromBehind);
            projectile.ProjectileDestroyAction += projectileHitInfo =>
            {
                if (ProjectileHitAction != null)
                {
                    ProjectileHitAction(projectileHitInfo);  
                }
              
                FiredProjectiles.Remove(projectile);

            };
        }

        /// <summary>
        /// Projectile hit the killable event.
        /// </summary>
        /// <param name="projectile">The projectile.</param>
        /// <param name="killable">The killable.</param>
        /// <param name="hittingFromBehind">if set to <c>true</c> then projectile is [hitting from behind].</param>
        protected void ProjectileHitKillableEvent(Projectile projectile, KillableBase killable, bool hittingFromBehind)
        {
            if (projectile.SpawningCombatMoveRef.Value != null)
            {
                if (projectile.SpawningCombatMoveRef.Value.HitEffects.Value != null
                    &&
                        (projectile.SpawningCombatMoveRef.Value.HitEffects.Value.ResourceEffects.Any()
                        ||
                        projectile.SpawningCombatMoveRef.Value.HitEffects.Value.SuperNaturalEffectsEffects.Any()))
                {
                    projectile.SpawningCombatMoveRef.Value.HitKillable(killable, hittingFromBehind);
                }
            }
            HitKillable(killable);            
        }

        /// <summary>
        /// Projectile hit the movable event.
        /// </summary>
        /// <param name="projectile">The projectile.</param>
        /// <param name="movable">The movable.</param>
        /// <param name="hittingFromBehind">if set to <c>true</c> then projectile is [hitting from behind].</param>
        protected void ProjectileHitMovableEvent(Projectile projectile, IMovable movable, bool hittingFromBehind)
        {
            if (projectile.SpawningCombatMoveRef.Value != null)
            {
                if (projectile.SpawningCombatMoveRef.Value.HitEffects.Value != null && projectile.SpawningCombatMoveRef.Value.HitEffects.Value.PushEffects.Any())
                {
                    projectile.SpawningCombatMoveRef.Value.HitMovable(movable, hittingFromBehind);
                }
            }
            HitMovable(movable);
        }
        #endregion

        #region public functions
        /// <summary>
        /// Fires a projectile of the loaded type.
        /// </summary>
        public void FireProjectile()
        {
            if (ProjectileFiredAction != null)
            {
                ProjectileFiredAction(new Projectile(ProjectileSpeed, LoadedProjectileType, LoadedProjectileSubType));    
            }           
        }

        /// <summary>
        /// Select next projectile type. Not yet in use
        /// </summary>
        public void NextProjectileType()
        {

        }

        /// <summary>
        /// Select next projectile sub type. Not yet in use.
        /// </summary>
        public void NextProjectileSubType()
        {

        }
        #endregion        
    }
}
