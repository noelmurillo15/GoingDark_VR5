using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// A Bundle of resource effects and continuous resource effects
    /// </summary>
    public class ResourceEffectCombo : ActionModel
    {
        // Subscribable properties
        public ActionList<ResourceEffect> ResourceEffects { get; set; }
        public ActionList<ContinuousResourceEffect> ContinuousResourceEffects { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceEffectCombo"/> class.
        /// </summary>
        public ResourceEffectCombo()
        {
            ResourceEffects = new ActionList<ResourceEffect>();
            ContinuousResourceEffects = new ActionList<ContinuousResourceEffect>();
        }
    }
}
