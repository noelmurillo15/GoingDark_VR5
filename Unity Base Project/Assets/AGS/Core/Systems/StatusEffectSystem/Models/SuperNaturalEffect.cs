using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// SuperNaturalEffects affects a Killables damage resistance or provides invulnerability
    /// </summary>
    public class SuperNaturalEffect : StaticStatusEffectBase
    {
        // Constructor properties
        public SuperNaturalEffectType EffectType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperNaturalEffect"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="isInfinite">if set to <c>true</c> [is infinite].</param>
        /// <param name="effectType">Type of the effect.</param>
        public SuperNaturalEffect(float strength, float duration, bool isInfinite, SuperNaturalEffectType effectType)
            : base(strength, StatusEffectStrengthType.Percentage, duration, isInfinite)
        {
            EffectType = effectType;
        }
    }
}
