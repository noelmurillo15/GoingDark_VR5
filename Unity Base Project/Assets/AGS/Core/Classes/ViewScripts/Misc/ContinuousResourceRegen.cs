using System;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that sets up a continuous regeneration of specified resource and interval.
    /// </summary>
    public class ContinuousResourceRegen : ViewScriptBase
    {
        public DamageableResourceType ResourceType;
        public int Strength;
        public float TicksPerSecond;
        private KillableBaseView _killableBaseView;
        private KillableBase _killableBase;
        private DamageableResource _characterResourceSupply;
        private TimerComponent _resourceRegenInterval;
        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _killableBaseView = ViewReference as KillableBaseView;
                if (_killableBaseView == null) return;

                _killableBase = _killableBaseView.Killable;

            }
            if (_killableBase == null) return;
            _characterResourceSupply = _killableBase.Resources.FirstOrDefault(x => x.ResourceType.Value == ResourceType);
            RegenCharacterResource(_characterResourceSupply, Strength, TicksPerSecond); 

        }

        /// <summary>
        /// Regens the character resource.
        /// </summary>
        /// <param name="characterResourceSupply">The character resource supply.</param>
        /// <param name="strength">The strength.</param>
        /// <param name="ticksPerSecond">The ticks per second.</param>
        private void RegenCharacterResource(DamageableResource characterResourceSupply, int strength, float ticksPerSecond)
        {

            _resourceRegenInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Resource regen Interval");
            _resourceRegenInterval.TimerMethod = () => {
                    if (characterResourceSupply.Current.Value < characterResourceSupply.Max.Value)
                    {
                        characterResourceSupply.Current.Value += strength;
                    }
                };
            _resourceRegenInterval.SetupIntervalInfinite(TimeSpan.FromSeconds(1 / ticksPerSecond));

        }
    }
}
