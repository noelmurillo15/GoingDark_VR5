using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// Base view for any weapon.
    /// </summary>
    [Serializable]
    public abstract class WeaponBaseView : ActionView
    {

        #region Public properties
        public bool AnimationBasedFiring;

        // References to be set in the editor
        public StatusEffectComboView StatusEffectComboView; // Owned status effect combo
        #endregion

        public WeaponBase Weapon;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Weapon = model as WeaponBase;
            if (Weapon == null) return;
            if (StatusEffectComboView != null)
            {
                Weapon.HitEffects.Value = StatusEffectComboView.StatusEffectCombo;
            }
        }
        #endregion

        #region protected functions

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