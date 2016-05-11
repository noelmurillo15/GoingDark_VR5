using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.Helpers;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// WeaponBase is base model for any equipable or thrown weapon. It containts Transform info and the weapons HitEffects (i.e stats)
    /// </summary>
    public abstract class WeaponBase : ActionModel
    {
        #region Properties
        // Constructor properties
        public Transform Transform;
        public bool AnimationBasedFiring;
        
        // Subscribable properties
        public ActionProperty<CombatEntityBase> OwnerCombatEntity; // Reference to the owner CombatEntity
        public ActionProperty<StatusEffectCombo> HitEffects { get; private set; } // All of this Weapons hit effects

        public Action FireAction { get; set; } // Subscribe to this action for non-animation based firing
        public Action WeaponHitAction { get; set; } // Subscribe to this to get notified of when the weapon hits something
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="WeaponBase"/> class.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="animationBasedFiring">if set to <c>true</c> [animation based firing].</param>
        protected WeaponBase(Transform transform, bool animationBasedFiring)
        {
            Transform = transform;
            AnimationBasedFiring = animationBasedFiring;
            OwnerCombatEntity = new ActionProperty<CombatEntityBase>();
            HitEffects = new ActionProperty<StatusEffectCombo>();
        }


        #region public functions
        /// <summary>
        /// Fires the weapon.
        /// </summary>
        public virtual void FireWeapon()
        {
            if (FireAction != null)
            {
                FireAction();
            }
        }

        /// <summary>
        /// Notify any subscriber of that this Weapon triggers its effects.
        /// </summary>
        public virtual void TriggerWeaponEffects()
        {
            if (WeaponHitAction != null)
            {
                WeaponHitAction();    
            }
            
        }

        /// <summary>
        /// Hits the killable with all HitEffects
        /// </summary>
        /// <param name="targetKillable">The target killable.</param>
        public virtual void HitKillable(KillableBase targetKillable)
        {
            TriggerWeaponEffects();
            var hittingFromBehind = CombatHelper.HittingFromBehind(OwnerCombatEntity.Value.Transform, targetKillable);
            if (OwnerCombatEntity.Value.ActiveCombatMoveSet.Value != null
                &&
                OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove.Value != null)
            {
                OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove.Value.HitKillable(targetKillable, hittingFromBehind); 
            }

            if (HitEffects.Value == null) return;
            CombatHelper.ApplyResourceEffects(targetKillable, HitEffects.Value.ResourceEffects, hittingFromBehind);
            CombatHelper.ApplySuperNaturalEffects(targetKillable, HitEffects.Value.SuperNaturalEffectsEffects);
            var character = targetKillable as CharacterBase;
            if (character == null) return;
            CombatHelper.ApplyPushEffects(character, HitEffects.Value.PushEffects, hittingFromBehind);
            CombatHelper.ApplyMovementffects(character, HitEffects.Value.MovementEffects);

        }

        /// <summary>
        /// Hits the movable with all PushEffects
        /// </summary>
        /// <param name="targetMovable">The target movable.</param>
        public virtual void HitMovable(IMovable targetMovable)
        {
            TriggerWeaponEffects();
            var hittingFromBehind = CombatHelper.HittingFromBehind(OwnerCombatEntity.Value.Transform, targetMovable);
            if (OwnerCombatEntity.Value.ActiveCombatMoveSet.Value != null
                &&
                OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove.Value != null)
            {
                OwnerCombatEntity.Value.ActiveCombatMoveSet.Value.ActiveCombatMove.Value.HitMovable(targetMovable, hittingFromBehind);
            }
            if (HitEffects.Value == null) return;
            if (targetMovable == null) return;
            CombatHelper.ApplyPushEffects(targetMovable, HitEffects.Value.PushEffects);
        }
        #endregion
    }
}
