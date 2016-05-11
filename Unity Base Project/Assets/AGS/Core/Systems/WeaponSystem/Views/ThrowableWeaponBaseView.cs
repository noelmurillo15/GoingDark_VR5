using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// ThrowableWeaponBaseView for all types ot throwables. Throwable weapons only differ in variable data.
    /// </summary>
    [Serializable]
    public abstract class ThrowableWeaponBaseView : WeaponBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public bool TriggerOnContact;
        public bool DestroyOnTrigger;
        public float TimerSeconds;
        public bool StickToTargets;
        public ThrowableWeaponType ThrowableWeaponType;
        
        // References to be set in the editor
        public AreaOfEffectView AreaOfEffectView;
        public ExplosionEffectBaseView ExplosionEffectView;
        public ThrowableWeapon ThrowableWeapon;
        #endregion

        public bool MarkedForDestroy { get; set; } // Used for disabling further contact but not destroying the gameobject immediately
        public bool StuckOnTarget { get; set; } // True if a StickToTargets throwable has connected with a target

        #region AGS Setup
        public override void InitializeView()
        {
            if (ThrowableWeapon == null)
            {
                ThrowableWeapon = new ThrowableWeapon(transform, AnimationBasedFiring, TriggerOnContact, DestroyOnTrigger, TimerSeconds, StickToTargets, ThrowableWeaponType);
            }
            SolveModelDependencies(ThrowableWeapon);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (AreaOfEffectView != null)
            {
                ThrowableWeapon.AreaOfEffect.Value = AreaOfEffectView.AreaOfEffect;
            }
            if (ExplosionEffectView != null)
            {
                ThrowableWeapon.ExplosionEffect.Value = ExplosionEffectView.ExplosionEffect;
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            ThrowableWeapon.ThrowableWeaponThrowAction += Throw;
            base.InitializeActionModel(model);
        }
        #endregion

        #region MonoBehaviours
        void OnCollisionEnter(Collision collision)
        {
            OnCollision(collision); // For making sure non-abstract childs has a collision function implemented
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [collision] occurs.
        /// </summary>
        /// <param name="other">The other.</param>
        protected abstract void OnCollision(Collision other);

        /// <summary>
        /// Throws the weapon with specified throw force.
        /// </summary>
        /// <param name="throwForce">The throw force.</param>
        protected abstract void Throw(Vector3 throwForce);
        #endregion
    }
}