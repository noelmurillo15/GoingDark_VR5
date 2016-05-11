using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// A PushEffect can used to push around characters or movables
    /// </summary>
    public class PushEffect : PeriodicStatusEffectBase
    {
        // Constructor properties
        public ForceType ForceType { get; private set; }
        public VectorDirection Direction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PushEffect"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="ticks">The ticks.</param>
        /// <param name="secondsBetweenTicks">The seconds between ticks.</param>
        /// <param name="forceType">The force mode.</param>
        /// <param name="vectorDirection">The vector direction.</param>
        public PushEffect(float strength, int ticks, float secondsBetweenTicks, ForceType forceType, VectorDirection vectorDirection)
            : base(strength, StatusEffectStrengthType.FixedValue, ticks, secondsBetweenTicks)
        {
            ForceType = forceType;
            Direction = vectorDirection;                        
        }
    }
}
