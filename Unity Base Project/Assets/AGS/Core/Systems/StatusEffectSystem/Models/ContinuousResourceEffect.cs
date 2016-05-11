using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{

    /// <summary>
    /// A special ticking resource effect. To be used with sustained skills that contantly drains a resource effect while active, or similar.
    /// </summary>
    public class ContinuousResourceEffect : StatusEffectBase
    {
        // Constructor properties
        public float SecondsInterval { get; private set; }
        public ResourceEffectType EffectType { get; private set; }
        public DamageableResourceType DamageableType { get; private set; }
        public bool IsActive { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuousResourceEffect"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="strengthType">Type of the strength.</param>
        /// <param name="isActive">if set to <c>true</c> this status effect is [is active].</param>
        /// <param name="secondsInterval">The seconds interval between ticks.</param>
        /// <param name="resourceEffectType">Type of the resource effect.</param>
        /// <param name="damageableResourceType">Type of the damageable resource.</param>
        public ContinuousResourceEffect(float strength, StatusEffectStrengthType strengthType, bool isActive, float secondsInterval,
            ResourceEffectType resourceEffectType, DamageableResourceType damageableResourceType)
            : base(strength, strengthType)
        {
            EffectType = resourceEffectType;
            DamageableType = damageableResourceType;
            SecondsInterval = secondsInterval;
            IsActive = isActive;
        }
    }
}
