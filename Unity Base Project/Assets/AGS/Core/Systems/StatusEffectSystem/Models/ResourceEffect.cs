using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// A ResourceEffect affects a Killables damageable resources
    /// </summary>
    public class ResourceEffect : PeriodicStatusEffectBase
    {
        // Constructor properties
        public ResourceEffectType EffectType { get; private set; }
        public DamageableResourceType DamageableType { get; private set; }
        public bool UseProximityModifier { get; private set; }
        public Transform Origin { get; private set; }
        public bool UseHitFromBehindModifier { get; private set; }
        public float HitFromBehindModifier { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceEffect"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="strengthType">Type of the strength.</param>
        /// <param name="ticks">The ticks.</param>
        /// <param name="secondsBetweenTicks">The seconds between ticks.</param>
        /// <param name="resourceEffectType">Type of the resource effect.</param>
        /// <param name="damageableResourceType">Type of the damageable resource.</param>
        /// <param name="useProximityModifier">if set to <c>true</c> then [use proximity modifier].</param>
        /// <param name="origin">The transform origin of effect. For use with proximity.</param>
        /// <param name="useHitFromBehindModifier">if set to <c>true</c> then [use hit from behind modifier].</param>
        /// <param name="hitFromBehindModifier">The hit from behind modifier.</param>
        public ResourceEffect(float strength, StatusEffectStrengthType strengthType, int ticks, float secondsBetweenTicks,
            ResourceEffectType resourceEffectType, DamageableResourceType damageableResourceType, bool useProximityModifier, Transform origin, bool useHitFromBehindModifier, float hitFromBehindModifier)
            : base(strength, strengthType, ticks, secondsBetweenTicks)
        {
            EffectType = resourceEffectType;
            DamageableType = damageableResourceType;
            UseProximityModifier = useProximityModifier;
            Origin = origin;
            UseHitFromBehindModifier = useHitFromBehindModifier;
            HitFromBehindModifier = hitFromBehindModifier;
        }
    }
}
