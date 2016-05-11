using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// Base view for any projectile view
    /// </summary>
    [Serializable]
    public abstract class ProjectileBaseView : ActionView
    {		
		#region Public properties
        // Fields to be set in the editor
		public float Speed;
        public ProjectileType ProjectileType;
        public ProjectileSubType ProjectileSubType;

        // References to be set in the editor
        public StatusEffectComboView HitEffectsView; // Owned hit effectts
        public ExplosionEffectBaseView ExplosionEffectBaseView; // Owned explosion effect
        public AreaOfEffectView AreaOfEffectView; // Owned area of effect
		#endregion

		public Projectile Projectile;
		
		#region AGS Setup
        public override void InitializeView()
        {
            if (Projectile == null)
            {
                Projectile = new Projectile(Speed, ProjectileType, ProjectileSubType);    
            }			
			SolveModelDependencies(Projectile);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (HitEffectsView != null)
            {
                Projectile.HitEffects.Value = HitEffectsView.StatusEffectCombo;
            }
            if (ExplosionEffectBaseView != null)
            {
                Projectile.ExplosionEffect.Value = ExplosionEffectBaseView.ExplosionEffect;
            }
            if (AreaOfEffectView != null)
            {
                Projectile.AreaOfEffect.Value = AreaOfEffectView.AreaOfEffect;
            }    	
        }
        #endregion				

        
        #region MonoBehaviours
        void OnCollisionEnter(Collision collision)
        {
            OnCollision(collision); // For making sure non-abstract childs has a collision function implemented
        }
        #endregion

        #region protected functions
        /// <summary>
        /// Called when [collision] occurs.
        /// </summary>
        /// <param name="other">The other.</param>
        protected abstract void OnCollision(Collision other);

        /// <summary>
        /// Destroys the physics body.
        /// </summary>
        protected void DetachBody()
        {
            // Destroy possible joint before destroying Rigidbody
            var joint = GetComponent<Joint>();
            if (joint != null)
            {
                Destroy(joint);

            }
            // Set possible collider to trigger mode
            var col = GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;
            }

            // Destroy renderers
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
            {
                Destroy(meshRenderer);
            }
        }
        #endregion
    }
}