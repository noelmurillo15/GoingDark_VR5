using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.Helpers;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.WeaponSystem
{
    /// <summary>
    /// ThrowableWeapon is a special weapon that can be thrown by a character
    /// </summary>
    public class ThrowableWeapon : WeaponBase
    {
        #region Properties
        // Constructor properties
        public bool TriggerOnContact { get; private set; }
        public bool DestroyOnTrigger { get; private set; }
        public float TimerSeconds { get; private set; }
        public float SecondsCharging { get; private set; }
        public float SecondsRecharging { get; private set; }
        public bool StickToTargets { get; private set; }
        public ThrowableWeaponType ThrowableWeaponType { get; private set; }
        public Vector3 ThrowingSpeed { get; private set; }

        // Subscribable properties
        public ActionProperty<ExplosionEffect> ExplosionEffect { get; private set; } // Optional explosion effect
        public ActionProperty<AreaOfEffect> AreaOfEffect { get; private set; } // Optional area of effect for HitEffects


        public Action BounceAction { get; set; } // Subscribe to this to get notified of when the throwable weapon bounced off something (but didnt trigger its effects)
        public Action<Vector3> ThrowableWeaponThrowAction { get; set; } // Subscribe to this to get notified of when the throwable weapon is thrown and with what speed

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowableWeapon" /> class.
        /// </summary>
        /// <param name="transform">The throwable weapons transform.</param>
        /// <param name="animationBasedFiring">if set to <c>true</c> [animation based firing].</param>
        /// <param name="triggerOnContact">if set to <c>true</c> then throwable will [trigger on contact].</param>
        /// <param name="destroyOnTrigger">if set to <c>true</c> then throwable will be [destroyed on trigger].</param>
        /// <param name="timerSeconds">The timer in seconds if its a timed throwable.</param>
        /// <param name="stickToTargets">if set to <c>true</c> throwable will [stick to targets].</param>
        /// <param name="throwableWeaponThrowableWeaponType">Type of the throwable weapon.</param>
        public ThrowableWeapon(Transform transform, bool animationBasedFiring, bool triggerOnContact, bool destroyOnTrigger, float timerSeconds, bool stickToTargets, ThrowableWeaponType throwableWeaponThrowableWeaponType)
            : base(transform, animationBasedFiring)
        {
            TriggerOnContact = triggerOnContact;
            DestroyOnTrigger = destroyOnTrigger;
            TimerSeconds = timerSeconds;
            StickToTargets = stickToTargets;
            ThrowableWeaponType = throwableWeaponThrowableWeaponType;
            ExplosionEffect = new ActionProperty<ExplosionEffect>();
            AreaOfEffect = new ActionProperty<AreaOfEffect>();

        }

        #region public functions

        /// <summary>
        /// Notify any subscribers that this weapon bounced.
        /// </summary>
        public void Bounce()
        {
            if (BounceAction != null)
            {
                BounceAction();    
            }
            
        }

        /// <summary>
        /// Notify any subscriber of that this Weapon triggers its effects.
        /// </summary>
        public override void TriggerWeaponEffects()
        {
            base.TriggerWeaponEffects();
            if (ExplosionEffect == null || ExplosionEffect.Value == null) return;
            ExplosionEffect.Value.TriggerExplosion();
            if (HitEffects.Value == null) return;
            if (AreaOfEffect.Value != null)
            {
                TriggerWeaponAreaEffects();
            }
        }

        /// <summary>
        /// Notify any subscriber of that this Weapon triggers its area of effects.
        /// </summary>
        private void TriggerWeaponAreaEffects()
        {
            foreach (var targetKillable in AreaOfEffect.Value.KillableTargets)
            {
                var hittingFromBehind = CombatHelper.HittingFromBehind(Transform, targetKillable);
                CombatHelper.ApplyResourceEffects(targetKillable, HitEffects.Value.ResourceEffects, hittingFromBehind);
                CombatHelper.ApplySuperNaturalEffects(targetKillable, HitEffects.Value.SuperNaturalEffectsEffects);
                var character = targetKillable as CharacterBase;
                if (character == null) return;
                CombatHelper.ApplyPushEffects(character, HitEffects.Value.PushEffects, hittingFromBehind);
                CombatHelper.ApplyMovementffects(character, HitEffects.Value.MovementEffects);
            }
            foreach (var targetRagdoll in AreaOfEffect.Value.RagdollTargets)
            {
                CombatHelper.ApplyPushEffects(targetRagdoll, HitEffects.Value.PushEffects, CombatHelper.HittingFromBehind(Transform, targetRagdoll));
            }
            foreach (var targetMovable in AreaOfEffect.Value.MovableTargets)
            {
                CombatHelper.ApplyPushEffects(targetMovable, HitEffects.Value.PushEffects, CombatHelper.HittingFromBehind(Transform, targetMovable));
            }
        }

        /// <summary>
        /// Hits the killable with all HitEffects
        /// </summary>
        /// <param name="targetKillable">The target killable.</param>
        public override void HitKillable(KillableBase targetKillable)
        {
            if (AreaOfEffect.Value == null)
            {
                TriggerWeaponEffects();
                var hittingFromBehind = CombatHelper.HittingFromBehind(Transform, targetKillable);
                CombatHelper.ApplyResourceEffects(targetKillable, HitEffects.Value.ResourceEffects, hittingFromBehind);
                CombatHelper.ApplySuperNaturalEffects(targetKillable, HitEffects.Value.SuperNaturalEffectsEffects);
                var character = targetKillable as CharacterBase;
                if (character == null) return;
                CombatHelper.ApplyPushEffects(character, HitEffects.Value.PushEffects, hittingFromBehind);
                CombatHelper.ApplyMovementffects(character, HitEffects.Value.MovementEffects);
            }
            else
            {
                if (!AreaOfEffect.Value.KillableTargets.Contains(targetKillable))
                {
                    AreaOfEffect.Value.KillableTargets.Add(targetKillable);
                }
                TriggerWeaponEffects();
            }
        }

        /// <summary>
        /// Hits the movable with all PushEffects.
        /// </summary>
        /// <param name="movable">The movable.</param>
        public override void HitMovable(IMovable movable)
        {
            if (AreaOfEffect.Value == null)
            {
                TriggerWeaponEffects();
                CombatHelper.ApplyPushEffects(movable, HitEffects.Value.PushEffects, CombatHelper.HittingFromBehind(Transform, movable));
            }
            else
            {
                if (!AreaOfEffect.Value.MovableTargets.Contains(movable))
                {
                    AreaOfEffect.Value.MovableTargets.Add(movable);
                }
                TriggerWeaponEffects();
            }
        }

        /// <summary>
        /// Throws the throwable weapon with specified throw force.
        /// </summary>
        /// <param name="throwForce">The throw force.</param>
        public void Throw(Vector3 throwForce)
        {
            ThrowableWeaponThrowAction(throwForce);
        }
        #endregion functions
    }
}
