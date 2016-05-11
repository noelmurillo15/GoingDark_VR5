using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.Helpers;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// EnvironmentHazards are single target hazards with statuseffects and/or an explosion effect. It delivers all its effects to its victim upon trigger.
    /// </summary>
    public class EnvironmentHazard : HazardBase
    {
        #region Properties
        // Constructor properties
        public Transform Transform { get; private set; }

        // Subscribable properties
        public ActionProperty<StatusEffectCombo> EffectsCombo { get; private set; } // Owned effects combo
        public ActionProperty<ExplosionEffect> ExplosionEffect { get; private set; } // Owned explosion effect
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentHazard"/> class.
        /// </summary>
        /// <param name="secondsActive">The seconds active.</param>
        /// <param name="secondsRecharging">The seconds recharging.</param>
        /// <param name="deactivateOnTrigger">if set to <c>true</c> [deactivate on trigger].</param>
        /// <param name="destroyOnTrigger">if set to <c>true</c> [destroy on trigger].</param>
        /// <param name="transform">The hazards transform.</param>
        public EnvironmentHazard(float secondsActive, float secondsRecharging, bool deactivateOnTrigger, bool destroyOnTrigger, Transform transform)
            : base(secondsActive, secondsRecharging, deactivateOnTrigger, destroyOnTrigger)
        {
            Transform = transform;
            EffectsCombo = new ActionProperty<StatusEffectCombo>();
            ExplosionEffect = new ActionProperty<ExplosionEffect>();
        }

        #region public functions

        /// <summary>
        /// Triggers the hazard.
        /// </summary>
        public override void TriggerHazard()
        {
            base.TriggerHazard();
            if (ExplosionEffect == null || ExplosionEffect.Value == null) return;
            ExplosionEffect.Value.TriggerExplosion();
        }

        /// <summary>
        /// Hits the killable.
        /// </summary>
        /// <param name="targetKillable">The target killable.</param>
        public void HitKillable(KillableBase targetKillable)
        {
            TriggerHazard();
            var hittingFromBehind = CombatHelper.HittingFromBehind(Transform, targetKillable);
            if (EffectsCombo.Value == null) return;
            CombatHelper.ApplyResourceEffects(targetKillable, EffectsCombo.Value.ResourceEffects, hittingFromBehind);
            CombatHelper.ApplySuperNaturalEffects(targetKillable, EffectsCombo.Value.SuperNaturalEffectsEffects);
            var character = targetKillable as CharacterBase;
            // If the Killables is not a movable character there is no need to apply push & movement effects.
            if (character == null) return;
            CombatHelper.ApplyPushEffects(character, EffectsCombo.Value.PushEffects, hittingFromBehind);
            CombatHelper.ApplyMovementffects(character, EffectsCombo.Value.MovementEffects);
        }

        /// <summary>
        /// Hits the movable.
        /// </summary>
        /// <param name="targetMovable">The target movable.</param>
        public void HitMovable(IMovable targetMovable)
        {
            TriggerHazard();
            var hittingFromBehind = CombatHelper.HittingFromBehind(Transform, targetMovable);
            if (EffectsCombo.Value == null) return;
            if (targetMovable == null) return;
            CombatHelper.ApplyPushEffects(targetMovable, EffectsCombo.Value.PushEffects, hittingFromBehind);
        }
        #endregion
    }
}
