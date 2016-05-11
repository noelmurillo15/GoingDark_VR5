using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;

namespace AGS.Core.Systems.SkillSystem
{
    /// <summary>
    /// Base class for any AGS skill. Supports resource costs (both instant costs and continuous) and a simple IsEnabled boolean.
    /// </summary>
    public abstract class SkillBase : ActionModel
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<bool> OutOfResources { get; private set; } // Is the character out of skill resources?
        public ActionProperty<ResourceEffectCombo> ResourceCost { get; private set; } // Skill resource cost of this skill
        public ActionProperty<bool> IsEnabled { get; private set; } // Is this skill enabled at this moment?
        private readonly ActionList<TimerTemporaryGameObject> _continuousEffects; // Updates continuous resource effects
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillBase"/> class.
        /// </summary>
        protected SkillBase()
        {
            OutOfResources = new ActionProperty<bool>();
            ResourceCost = new ActionProperty<ResourceEffectCombo>();
            IsEnabled = new ActionProperty<bool>() { Value = true }; // Default skill state is enabled
            _continuousEffects = new ActionList<TimerTemporaryGameObject>();
        }

        #region state transitions
        /// <summary>
        /// Enables the skill
        /// </summary>
        public void SkillTransitionToStateEnable()
        {
            IsEnabled.Value = true;
        }

        /// <summary>
        /// Disables the skill
        /// </summary>
        public void SkillTransitionToStateDisable()
        {
            IsEnabled.Value = false;
        }
        #endregion

        #region public functions
        /// <summary>
        /// Checks resources and sets OutOfResource if run out
        /// </summary>
        /// <param name="killable">The killable.</param>
        public void CheckSupplyResourceEffects(KillableBase killable)
        {
            if (ResourceCost.Value == null || !ResourceCost.Value.ResourceEffects.Any()) return;
            foreach (var resourceEffect in ResourceCost.Value.ResourceEffects)
            {
                var characterResourceToCheck =
                    killable.Resources.FirstOrDefault(resource => resource.ResourceType.Value == resourceEffect.DamageableType);
                if (characterResourceToCheck == null) continue;
                if (!(characterResourceToCheck.Current.Value < resourceEffect.Strength))
                {
                    OutOfResources.Value = false;
                    continue;
                }
                OutOfResources.Value = true;
                return;
            }
        }

        /// <summary>
        /// Checks resources and sets OutOfResource if run out
        /// </summary>
        /// <param name="killable">The killable.</param>
        public void CheckSupplyContinuousResourceEffects(KillableBase killable)
        {
            if (ResourceCost.Value == null || !ResourceCost.Value.ContinuousResourceEffects.Any()) return;
            foreach (var resourceEffect in ResourceCost.Value.ContinuousResourceEffects)
            {
                var characterResourceToCheck =
                    killable.Resources.FirstOrDefault(resource => resource.ResourceType.Value == resourceEffect.DamageableType);
                if (characterResourceToCheck == null) continue;
                if (characterResourceToCheck.Current.Value >= resourceEffect.Strength)
                {
                    OutOfResources.Value = false;
                    continue;
                }
                OutOfResources.Value = true;
                return;
            }
        }

        /// <summary>
        /// Applies the resource cost.
        /// </summary>
        /// <param name="killable">The killable.</param>
        public virtual void ApplyResourceCost(KillableBase killable)
        {
            if (ResourceCost.Value == null || !ResourceCost.Value.ResourceEffects.Any()) return;
            foreach (var effect in ResourceCost.Value.ResourceEffects)
            {
                killable.ApplyResourceEffect(effect, false);
            }
        }

        /// <summary>
        /// Activates all continuous resource costs associated with the skill
        /// </summary>
        /// <param name="killable">The killable.</param>
        public virtual void ActivateContinuousResourceCosts(KillableBase killable)
        {
            if (ResourceCost.Value == null || !ResourceCost.Value.ContinuousResourceEffects.Any()) return;


            foreach (var continuousResourceEffect in ResourceCost.Value.ContinuousResourceEffects)
            {
                var effect = continuousResourceEffect;
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Continuous resource costs");
                timerComponent.TimerMethod = () => killable.ApplyContinuousResourceEffect(effect);
                timerComponent.SetupIntervalInfinite(continuousResourceEffect.SecondsInterval);
                _continuousEffects.Add(timerComponent);
                TimerComponents.Add(timerComponent);
            }
        }

        /// <summary>
        /// Stops all active continuous resource cost
        /// </summary>
        public virtual void DeactivateContinuousResourceCosts()
        {
            if (_continuousEffects == null || _continuousEffects.Count <= 0) return;
            foreach (var continuousEffect in _continuousEffects)
            {
                continuousEffect.FinishTimer();
            }
            _continuousEffects.Clear();
        }

        #endregion functions
    }
}
