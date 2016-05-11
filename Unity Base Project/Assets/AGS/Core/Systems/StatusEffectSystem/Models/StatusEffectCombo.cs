using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// A bundle of any time of status effects
    /// </summary>
    public class StatusEffectCombo : ActionModel
    {
        // Subscribable properties
        public ActionList<ResourceEffect> ResourceEffects {get; private set; }
        public ActionList<SuperNaturalEffect> SuperNaturalEffectsEffects { get; private set; }
        public ActionList<PushEffect> PushEffects { get; private set; }
        public ActionList<MovementEffect> MovementEffects { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectCombo"/> class.
        /// </summary>
        public StatusEffectCombo()
        {
            ResourceEffects = new ActionList<ResourceEffect>();
            SuperNaturalEffectsEffects = new ActionList<SuperNaturalEffect>();
            PushEffects = new ActionList<PushEffect>();
            MovementEffects = new ActionList<MovementEffect>();
        }

    }
}
