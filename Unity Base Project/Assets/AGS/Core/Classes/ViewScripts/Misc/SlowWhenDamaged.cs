using System.Linq;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that slows a character down when a damage threshold is reached 
    /// </summary>
    public class SlowWhenDamaged : ViewScriptBase
    {
        [Range(0, 100f)]
        public float ThresholdPercentage;
        [Range(0, 100f)]
        public float SlowEffectPercentage;

        protected CharacterBaseView CharacterBaseView;
        public CharacterBase Character;

        private MovementEffect _slowEffect;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                CharacterBaseView = ViewReference as CharacterBaseView;
                if (CharacterBaseView == null) return;

                Character = CharacterBaseView.Character;
                _slowEffect = new MovementEffect(-SlowEffectPercentage, StatusEffectStrengthType.Percentage, 0f, true, MovementEffectType.SpeedChange);
            }
            if (Character == null) return;
            var characterHealth = Character.Resources.FirstOrDefault(resource => resource.ResourceType.Value == DamageableResourceType.Health);
            if (characterHealth != null)
            {
                characterHealth.Current.OnValueChanged += (sender, health) => OnCharacterHealthChanged(characterHealth.Max.Value, health.Value);
            }
        }

        /// <summary>
        /// Called when [character health changed].
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="current">The current.</param>
        private void OnCharacterHealthChanged(int max, int current)
        {
            var currentRelativeHealth = ((float)current / max) * 100f;
            if (currentRelativeHealth < ThresholdPercentage)
            {
                Character.ApplyMovementEffect(_slowEffect);
            }
            else
            {
               Character.RemoveMovementEffect(_slowEffect);
            }
        }
    }
}
