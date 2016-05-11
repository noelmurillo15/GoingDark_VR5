using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;
using AGS.Core.Base;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// ProjectileWeaponView for any type of projectile.
    /// </summary>
    [Serializable]
    public class ProjectileWeaponView : EquipableWeaponBaseView
    {

        #region Public properties
        // Fields to be set in the editor
        public float ProjectileSpeed;
        public ProjectileType InitialProjectileType; // Starting projetile type
        public ProjectileSubType InitialProjectileSubType; // Starting projectile sub type

        // References to be set in the editor
        public Transform ProjectileSpawnPosition; // Reference to the Transform from where projectiles should be fired        
        #endregion

        public ProjectileWeapon ProjectileWeapon;

        #region AGS Setup
        public override void InitializeView()
        {
            ProjectileWeapon = new ProjectileWeapon(transform, AnimationBasedFiring, Range, CombatMoveSetType, WeaponGripLeftHand, WeaponGripRightHand, ProjectileSpawnPosition, ProjectileSpeed, InitialProjectileType, InitialProjectileSubType);
            SolveModelDependencies(ProjectileWeapon);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            ProjectileWeapon.ProjectileFiredAction += FireProjectile;
        }
        #endregion

        #region mono functions
        public override void Awake()
        {
            base.Awake();

            if (ProjectileSpawnPosition == null)
            {
                ProjectileSpawnPosition = transform;
            }
        }
        #endregion

        #region protected functions
        /// <summary>
        /// Instaniates a new projectile, makes it a child to the projectiles container and add its to FireProjectiles list.
        /// </summary>
        /// <param name="projectile">The projectile.</param>
        protected virtual void FireProjectile(Projectile projectile)
        {
            var projectileObj = Instantiate(Resources.Load(string.Format("Projectiles/{0}{1}", projectile.ProjectileSubType, projectile.ProjectileType)), ProjectileWeapon.ProjectileSpawnPosition.position, ProjectileWeapon.ProjectileSpawnPosition.rotation) as GameObject;
            if (projectileObj == null) return;
            var projectileBaseView = projectileObj.GetComponent<ProjectileBaseView>();
            if (projectileBaseView != null)
            {

                projectileBaseView.transform.parent = GameManager.ProjectilesContainer.transform;
                projectileBaseView.ViewReady.OnValueChanged += (sender, viewReady) =>
                {
                    if (!viewReady.Value) return;
                    ProjectileWeapon.FiredProjectiles.Add(projectileBaseView.Projectile);
                };
            }

        }
        #endregion

    }
}