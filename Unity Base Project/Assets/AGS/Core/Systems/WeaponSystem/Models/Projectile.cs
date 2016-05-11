using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.Helpers;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// A projectile must be fired by a projectile weapon
    /// </summary>
    public class Projectile : ActionModel
    {
        #region Properties
        // Constructor properties
        public float Speed { get; private set; }
        public ProjectileType ProjectileType { get; private set; }
        public ProjectileSubType ProjectileSubType { get; private set; }

        // Subscribable properties
        public ActionProperty<ProjectileWeapon> OwnerProjectileWeapon { get; set; } // Reference to the owner projectile weapon
        public ActionProperty<StatusEffectCombo> HitEffects { get; private set; } // Owned hit effects
        public ActionProperty<ExplosionEffect> ExplosionEffect { get; private set; } // Owned explosion effect
        public ActionProperty<AreaOfEffect> AreaOfEffect { get; private set; } // AreaOfEffect reference
        public ActionProperty<CombatMove> SpawningCombatMoveRef { get; private set; } // Reference to the combat move that was used to fire this projectile
        public ActionProperty<Vector3> FiringDirection { get; private set; } // The projectiles direction is based on which way the weapon muzzle is pointing at

        public Action<ProjectileHitData> ProjectileDestroyAction { get; set; } // Subscribe to this to get notified of when this projectile is destroyed
        public Action<KillableBase, bool> ProjectileHitKillableAction { get; set; } // Subscribe to this to get notified of when this projectile hit something killable
        public Action<IMovable, bool> ProjectileHitMovableAction { get; set; } // Subscribe to this to get notified of when this projectile hit something movable
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Projectile"/> class.
        /// </summary>
        /// <param name="speed">The projectile speed.</param>
        /// <param name="projectileType">Type of the projectile.</param>
        /// <param name="projectileSubType">Type of the projectile sub.</param>
        public Projectile(float speed, ProjectileType projectileType, ProjectileSubType projectileSubType)
        {
            Speed = speed;
            ProjectileType = projectileType;
            ProjectileSubType = projectileSubType;
            OwnerProjectileWeapon = new ActionProperty<ProjectileWeapon>();
            HitEffects = new ActionProperty<StatusEffectCombo>();
            ExplosionEffect = new ActionProperty<ExplosionEffect>();
            AreaOfEffect = new ActionProperty<AreaOfEffect>();
            SpawningCombatMoveRef = new ActionProperty<CombatMove>();
            FiringDirection = new ActionProperty<Vector3>();
        }


        #region public functions
        /// <summary>
        /// Triggers the projectiles effects.
        /// </summary>
        public void TriggerProjectileEffects()
        {
            if (ExplosionEffect != null && ExplosionEffect.Value != null)
            {
                ExplosionEffect.Value.TriggerExplosion();
            }
            if (HitEffects.Value == null) return;
            if (AreaOfEffect.Value != null)
            {
                TriggerProjectileAreaEffects();
            }
        }

        /// <summary>
        /// Triggers the projectiles area effects.
        /// </summary>
        private void TriggerProjectileAreaEffects()
        {
            foreach (var targetKillable in AreaOfEffect.Value.KillableTargets)
            {
                var hittingFromBehind = CombatHelper.HittingFromBehind(OwnerProjectileWeapon.Value.Transform, targetKillable);
                CombatHelper.ApplyResourceEffects(targetKillable, HitEffects.Value.ResourceEffects, hittingFromBehind);
                CombatHelper.ApplySuperNaturalEffects(targetKillable, HitEffects.Value.SuperNaturalEffectsEffects);
                var character = targetKillable as CharacterBase;
                if (character == null) return;
                CombatHelper.ApplyPushEffects(character, HitEffects.Value.PushEffects, hittingFromBehind);
                CombatHelper.ApplyMovementffects(character, HitEffects.Value.MovementEffects);
            }
            foreach (var targetRagdoll in AreaOfEffect.Value.RagdollTargets)
            {
                CombatHelper.ApplyPushEffects(targetRagdoll, HitEffects.Value.PushEffects, CombatHelper.HittingFromBehind(OwnerProjectileWeapon.Value.Transform, targetRagdoll));
            }
            foreach (var targetMovable in AreaOfEffect.Value.MovableTargets)
            {
                CombatHelper.ApplyPushEffects(targetMovable, HitEffects.Value.PushEffects, CombatHelper.HittingFromBehind(OwnerProjectileWeapon.Value.Transform, targetMovable));
            }
        }

        /// <summary>
        /// Call this to make the projectile hit the killable with all effects
        /// </summary>
        /// <param name="targetKillable">The target killable.</param>
        /// <param name="projectileHitInfo">The projectile hit information.</param>
        public void ProjectileHitKillable(KillableBase targetKillable, ProjectileHitData projectileHitInfo)
        {
            if (AreaOfEffect.Value == null)
            {
                TriggerProjectileEffects();
                if (HitEffects.Value != null)
                {
                    var hittingFromBehind = CombatHelper.HittingFromBehind(OwnerProjectileWeapon.Value.Transform, targetKillable);
                    CombatHelper.ApplyResourceEffects(targetKillable, HitEffects.Value.ResourceEffects, hittingFromBehind);
                    CombatHelper.ApplySuperNaturalEffects(targetKillable, HitEffects.Value.SuperNaturalEffectsEffects);
                    var character = targetKillable as CharacterBase;
                    if (character == null) return;
                    CombatHelper.ApplyPushEffects(character, HitEffects.Value.PushEffects, hittingFromBehind);
                    CombatHelper.ApplyMovementffects(character, HitEffects.Value.MovementEffects);
                }
            }
            else
            {
                if (!AreaOfEffect.Value.KillableTargets.Contains(targetKillable))
                {
                    AreaOfEffect.Value.KillableTargets.Add(targetKillable);
                }
                TriggerProjectileEffects();
            }
            if (ProjectileHitKillableAction != null)
            {
                ProjectileHitKillableAction(targetKillable, CombatHelper.HittingFromBehind(projectileHitInfo.HitTransform, targetKillable));    
            }
            if (ProjectileDestroyAction != null)
            {
                ProjectileDestroyAction(projectileHitInfo);
            }
        }

        /// <summary>
        /// Call this to make the projectile hit the movable with all push effects
        /// </summary>
        /// <param name="movable">The movable.</param>
        /// <param name="projectileHitInfo">The projectile hit information.</param>
        public void ProjectileHitMovable(IMovable movable, ProjectileHitData projectileHitInfo)
        {
            if (AreaOfEffect.Value == null)
            {
                TriggerProjectileEffects();
                if (HitEffects.Value != null)
                {
                    CombatHelper.ApplyPushEffects(movable, HitEffects.Value.PushEffects, CombatHelper.HittingFromBehind(projectileHitInfo.HitTransform, movable));
                }
            }
            else
            {
                if (!AreaOfEffect.Value.MovableTargets.Contains(movable))
                {
                    AreaOfEffect.Value.MovableTargets.Add(movable);
                }
                TriggerProjectileEffects();
            }
           
            if (ProjectileHitMovableAction != null)
            {
                ProjectileHitMovableAction(movable, CombatHelper.HittingFromBehind(projectileHitInfo.HitTransform, movable));
            }
            if (ProjectileDestroyAction != null)
            {
                ProjectileDestroyAction(projectileHitInfo);
            }
        }

        /// <summary>
        /// Triggers the projectile effects, and notifies any subscribers that this projectile is being destroyed.
        /// </summary>
        /// <param name="projectileHitInfo">The projectile hit information.</param>
        public void ProjectileDestroy(ProjectileHitData projectileHitInfo)
        {
            TriggerProjectileEffects();
            if (ProjectileDestroyAction != null)
            {
                ProjectileDestroyAction(projectileHitInfo);
            }
        }
        #endregion
    }
}
