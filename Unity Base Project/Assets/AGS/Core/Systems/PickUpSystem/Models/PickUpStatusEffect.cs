using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// A PickUp that contains status effects
    /// </summary>
    public class PickUpStatusEffect : PickUpItemBase
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<StatusEffectCombo> EffectsCombo { get; private set; } // Owned effects

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="PickUpStatusEffect"/> class.
        /// </summary>
        public PickUpStatusEffect()
        {
            EffectsCombo = new ActionProperty<StatusEffectCombo>();
        }

        #region public functions
        /// <summary>
        /// Applies the status effects.
        /// </summary>
        /// <param name="character">The character.</param>
        public void ApplyEffects(CharacterBase character)
        {
            foreach (var resourceEffect in EffectsCombo.Value.ResourceEffects)
            {
                character.ApplyResourceEffect(resourceEffect);
            }
            foreach (var superNaturalEffect in EffectsCombo.Value.SuperNaturalEffectsEffects)
            {
                character.ApplySuperNaturalEffect(superNaturalEffect);
            }
            foreach (var movementEffect in EffectsCombo.Value.MovementEffects)
            {
                character.ApplyMovementEffect(movementEffect);
            }
            foreach (var pushEffect in EffectsCombo.Value.PushEffects)
            {
                character.ApplyPushEffect(pushEffect);
            }
        }
        #endregion
    }
}
