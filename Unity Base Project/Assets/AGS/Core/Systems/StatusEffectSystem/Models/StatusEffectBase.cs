using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// Base model for any status effect. Provides strength and strength type variables
    /// </summary>
    public abstract class StatusEffectBase : ActionModel{

        // Constructor properties
        public float Strength { get; private set; }
        public StatusEffectStrengthType StrengthType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectBase"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="strengthType">Type of the strength.</param>
        protected StatusEffectBase(float strength, StatusEffectStrengthType strengthType)
        {
            Strength = strength;
            StrengthType = strengthType;
        }
    }
}
