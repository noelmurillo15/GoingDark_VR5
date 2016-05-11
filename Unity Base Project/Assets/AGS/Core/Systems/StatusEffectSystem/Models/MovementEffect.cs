using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// A movementeffect is primary used to affect a characters movability
    /// </summary>
    public class MovementEffect : StaticStatusEffectBase
    {
        // Constructor properties
        public MovementEffectType EffectType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementEffect"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="strengthType">Type of the strength.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="isInfinite">if set to <c>true</c> [is infinite].</param>
        /// <param name="effectType">Type of the effect.</param>
        public MovementEffect(float strength, StatusEffectStrengthType strengthType, float duration, bool isInfinite, MovementEffectType effectType)
            : base(strength, strengthType, duration, isInfinite)
        {
            EffectType = effectType;
        }
    }
}
