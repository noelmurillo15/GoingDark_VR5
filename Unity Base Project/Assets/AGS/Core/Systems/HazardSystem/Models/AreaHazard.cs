using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// AreaHazard has an AreaOfEffect and thus handles over all target detection
    /// </summary>
    public class AreaHazard : EnvironmentHazard
    {
        #region Properties
        // Constructor properties
        public float SecondsBetweenTicks { get; private set; } // 

        // Action properties
        public ActionProperty<AreaOfEffect> AreaOfEffect { get; private set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaHazard"/> class.
        /// </summary>
        /// <param name="secondsActive">The seconds active.</param>
        /// <param name="secondsRecharging">The seconds recharging.</param>
        /// <param name="secondsBetweenTicks">Determines how often the AreaHazard applies effects to its victims</param>
        /// <param name="deactivateOnTrigger">if set to <c>true</c> [deactivate on trigger].</param>
        /// <param name="destroyOnTrigger">if set to <c>true</c> [destroy on trigger].</param>
        /// <param name="transform">The transform.</param>
        public AreaHazard(float secondsActive, float secondsRecharging, float secondsBetweenTicks, bool deactivateOnTrigger, bool destroyOnTrigger, Transform transform)
            : base(secondsActive, secondsRecharging, deactivateOnTrigger, destroyOnTrigger, transform)
        {
            SecondsBetweenTicks = secondsBetweenTicks;
            AreaOfEffect = new ActionProperty<AreaOfEffect>();
            AreaOfEffect.OnValueChanged += OnAreaEffectChanged;
        }

        /// <summary>
        /// Called when [area effect changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="areaOfEffect">The <see cref="ActionPropertyEventArgs{AreaOfEffect}"/> instance containing the event data.</param>
        private void OnAreaEffectChanged(object sender, ActionPropertyEventArgs<AreaOfEffect> areaOfEffect)
        {
            areaOfEffect.Value.KillableTargets.ListItemRemoved += OnDamagaeableRemoved;
        }

        /// <summary>
        /// Called when [damagaeable removed].
        /// When a vicitim leaves the AreaOfEffect we may need to remove some movement effects that was added while victim was inside area.
        /// </summary>
        /// <param name="killableRemoved">The killable removed.</param>
        private void OnDamagaeableRemoved(KillableBase killableRemoved)
        {
            if (EffectsCombo.Value == null) return;

            // check if char has any movement or supernatural effects that needs removal
            if (!EffectsCombo.Value.SuperNaturalEffectsEffects.Any()) return;
            foreach (var superNaturalEffect in EffectsCombo.Value.SuperNaturalEffectsEffects)
            {
                killableRemoved.RemoveSuperNaturalEffect(superNaturalEffect);
            }
            var characterRemoved = killableRemoved as CharacterBase;
            if (characterRemoved == null) return;

            if (!EffectsCombo.Value.MovementEffects.Any()) return;
            foreach (var movementEffect in EffectsCombo.Value.MovementEffects)
            {
                characterRemoved.RemoveMovementEffect(movementEffect);
            }
        }

        #region public
        /// <summary>
        /// Hit all Killables within AreaOfEffect with all effects.
        /// </summary>
        public void ApplyEffectsToTargets()
        {
            if (AreaOfEffect.Value == null) return;
            if (AreaOfEffect.Value.KillableTargets == null) return;
            foreach (var target in AreaOfEffect.Value.KillableTargets)
            {
                if (EffectsCombo == null) continue;
                HitKillable(target);
            }
        }

        /// <summary>
        /// Hit all Ragdolls within AreaOfEffect with all effects.
        /// </summary>
        public void ApplyEffectsToRagdolls()
        {
            if (AreaOfEffect.Value == null) return;
            if (AreaOfEffect.Value.RagdollTargets == null) return;
            foreach (var ragdollTarget in AreaOfEffect.Value.RagdollTargets)
            {
                if (EffectsCombo == null) continue;
                HitMovable(ragdollTarget);
            }
        }

        /// <summary>
        /// Hit all Movables within AreaOfEffect with all effects.
        /// </summary>
        public void ApplyEffectsToMovables()
        {
            if (AreaOfEffect.Value == null) return;
            if (AreaOfEffect.Value.MovableTargets == null) return;
            foreach (var movable in AreaOfEffect.Value.MovableTargets)
            {
                if (EffectsCombo == null) continue;
                HitMovable(movable);
            }
        }
        #endregion
    }
}
