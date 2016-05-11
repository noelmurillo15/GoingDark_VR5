using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// StaticStatusEffectBase is a base model for effects that should be applied as a contstant effect for its duration
    /// </summary>
    public abstract class StaticStatusEffectBase : StatusEffectBase
    {
        // Constructor properties
        public float Duration { get; private set; }
        public bool IsInfinite { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticStatusEffectBase"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="strengthType">Type of the strength.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="isInfinite">if set to <c>true</c> this status effect [is infinite].</param>
        protected StaticStatusEffectBase(float strength, StatusEffectStrengthType strengthType, float duration, bool isInfinite)
            : base(strength, strengthType)
        {
            Duration = duration;
            IsInfinite = isInfinite;
        }
    }
}
