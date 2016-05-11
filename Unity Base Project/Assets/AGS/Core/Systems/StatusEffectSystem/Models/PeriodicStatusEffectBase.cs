using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// PeriodicStatusEffectBase is a base model for periodic ticking status effects
    /// </summary>
    public abstract class PeriodicStatusEffectBase : StatusEffectBase
    {
        // Constructor properties
        public int Ticks { get; private set; }
        public float SecondsBetweenTicks { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicStatusEffectBase"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="strengthType">Type of the strength.</param>
        /// <param name="ticks">Numer of ticks before status effect is done.</param>
        /// <param name="secondsBetweenTicks">The seconds between the ticks.</param>
        protected PeriodicStatusEffectBase(float strength, StatusEffectStrengthType strengthType, int ticks, float secondsBetweenTicks)
            : base(strength, strengthType)
        {
            Ticks = ticks;
            SecondsBetweenTicks = secondsBetweenTicks;
        }
    }
}