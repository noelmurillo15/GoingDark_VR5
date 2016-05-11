using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// ExplosionEffect is a special type of effect that should be used for triggering an explosion force in the UnityEngine
    /// </summary>
    public class ExplosionEffect : ActionModel
    {
        // Constructor properties
        public float Strength { get; private set; }
        public float Radius { get; private set; }
        public float UpwardsModifier { get; private set; }

        public Action ExplosionEffectTriggerAction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionEffect"/> class.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="upwardsModifier">The upwards modifier.</param>
        public ExplosionEffect(float strength, float radius, float upwardsModifier)
        {
            Strength = strength;

            Radius = radius;
            UpwardsModifier = upwardsModifier;
        }

        /// <summary>
        /// Triggers the explosion.
        /// </summary>
        public void TriggerExplosion()
        {
            ExplosionEffectTriggerAction();
        }
    }
}
