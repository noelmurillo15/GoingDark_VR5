using System;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.GameLevelSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Base class for any killable object. Implements IDamagable which is used by the combat helper to supply resource effects and supernatural effects.
    /// Inherit from this class to create anything that could be destroyed.
    /// </summary>
    public abstract class KillableBase : ActionModel, IDamageable
    {
        #region Properties
        // Constructor properties
        public Transform Transform { get; set; }
        public string Name { get; private set; } // 

        // Subscribable properties
        public ActionProperty<GameLevel> OwnerGameLevel; // Reference to owner game level
        public ActionProperty<float> CurrentResistance { get; private set; }    // Float multiplier for damage resistance. Defaults to 1
        public ActionProperty<DamageableState> DamageableCurrentState { get; set; } // Holds the current state of the killable
        public ActionList<DamageableResource> Resources { get; set; } // Default resource types be of type health, stamina and air. Create new types as needed
        public ActionList<SuperNaturalEffect> ActiveSuperNaturalEffects { get; set; } // Default supernatural effects can be invulnerability or damage resistance modifiers. When extending - think of supernatural effects as something that has to do with resources or DamageableState in some way

        public Action DyingAction { get; set; } // Subscribe to this Action to get notified when the Killable is about to die
        public Action<ResourceEffect> ResourceEffectAppliedAction { get; set; } // Subscribe to this Action to get notified when the Killable recieved a resource effect (i.e heal or damage)
        public Func<Transform, float> ProximityModifierFunc { get; set; } // Subscribe to this Func to recieve the proximity of the killable to another transform
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="KillableBase"/> class.
        /// </summary>
        /// <param name="transform">The Killable transform.</param>
        /// <param name="name">Killable name. Is instantiating ragdolls upon death, this need to match Ragdoll prefix. A Killable with the name John will try to instantiate JohnRagdoll</param>
        protected KillableBase(Transform transform, string name)
        {
            Transform = transform;
            Name = name;
            OwnerGameLevel = new ActionProperty<GameLevel>();
            CurrentResistance = new ActionProperty<float> { Value = 1f };
            DamageableCurrentState = new ActionProperty<DamageableState>();

            Resources = new ActionList<DamageableResource>();
            Resources.ListItemAdded += resource =>
            {
                if (resource.IsVital.Value)
                {
                    ObserveVitals(resource);
                }
            };
            ActiveSuperNaturalEffects = new ActionList<SuperNaturalEffect>();
            ActiveSuperNaturalEffects.ListItemAdded += SuperNaturalEffectAdded;
            ActiveSuperNaturalEffects.ListItemAdded += SuperNaturalEffectRemoved;
        }

        #region Private functions
        /// <summary>
        /// Observes characters vitals dies when reaching zero.        
        /// </summary>
        /// <param name="vitalResource">The vital resource.</param>
        private void ObserveVitals(DamageableResource vitalResource)
        {
            vitalResource.Current.OnValueChanged += (sender, current) =>
                {
                    if (current.Value <= 0)
                    {
                        TransitionToStateDie();
                    }
                };
        }

        /// <summary>
        /// Applies a resource effect to a specific damageable resource
        /// </summary>
        /// <param name="strength">ResourceEffect strength.</param>
        /// <param name="strengthType">Type of the ResourceEffects strength.</param>
        /// <param name="damageableResourceType">Type of the damageable resource.</param>
        /// <param name="effectType">Type of the ResourceEffect.</param>
        /// <param name="damageableResource">The damageable resource.</param>
        private void ApplyResourceEffect(float strength, StatusEffectStrengthType strengthType, DamageableResourceType damageableResourceType, ResourceEffectType effectType, DamageableResource damageableResource)
        {
            float effectiveResourceStrength;
            if (strengthType == StatusEffectStrengthType.FixedValue)
            {
                effectiveResourceStrength = strength;
            }
            else
            {
                effectiveResourceStrength = damageableResource.Max.Value * strength / 100f;
            }
            switch (effectType)
            {
                case ResourceEffectType.Heal:
                    damageableResource.Current.Value += Mathf.CeilToInt(effectiveResourceStrength);
                    break;
                case ResourceEffectType.Damage:

                    if (damageableResourceType == DamageableResourceType.Health)
                    {
                        if (DamageableCurrentState.Value != DamageableState.Invulnerable
                            &&
                            DamageableCurrentState.Value != DamageableState.Destroyed)
                        {
                            var damResistance = ActiveSuperNaturalEffects.FirstOrDefault(x => x.EffectType == SuperNaturalEffectType.DamageResistance);
                            if (damResistance != null)
                            {
                                var reducedDamage = effectiveResourceStrength * CurrentResistance.Value;
                                ReduceResourceCurrent(damageableResource, Mathf.CeilToInt(reducedDamage));
                            }
                            else
                            {
                                ReduceResourceCurrent(damageableResource, Mathf.CeilToInt(effectiveResourceStrength));
                            }
                        }
                        // if killable is invulnerable - just ignore any health damage                    
                    }
                    else if (DamageableCurrentState.Value != DamageableState.Destroyed)
                    {
                        ReduceResourceCurrent(damageableResource, Mathf.CeilToInt(effectiveResourceStrength));
                    }

                    break;
            }
        }

        /// <summary>
        /// Reduces the resource current.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="amountToReduce">The amount to reduce.</param>
        private static void ReduceResourceCurrent(DamageableResource resource, int amountToReduce)
        {
            var newCurrent = resource.Current.Value - amountToReduce;
            resource.Current.Value = newCurrent <= 0 ? 0 : newCurrent;
        }

        /// <summary>
        /// List add notification. Subscribes to added SuperNaturalEffects.
        /// </summary>
        /// <param name="superNaturalEffect">The super natural effect.</param>
        private void SuperNaturalEffectAdded(SuperNaturalEffect superNaturalEffect)
        {
            if (superNaturalEffect.EffectType == SuperNaturalEffectType.Invulnerability)
            {
                TransitionToStateInvulnerable();
            }
            else
            {
                CalculateAndSetResistanceState();
            }
        }

        /// <summary>
        /// List remove notification. Subscribes to removed SuperNaturalEffects.
        /// </summary>
        /// <param name="superNaturalEffect">The super natural effect.</param>
        private void SuperNaturalEffectRemoved(SuperNaturalEffect superNaturalEffect)
        {
            if (superNaturalEffect.EffectType == SuperNaturalEffectType.Invulnerability)
            {
                TransitionToStateNormal();
            }
            else
            {
                CalculateAndSetResistanceState();
            }
        }

        /// <summary>
        ///Determines current damage resistance and sets weakend or strenghtened states.
        /// </summary>
        private void CalculateAndSetResistanceState()
        {
            CurrentResistance.Value = 1f; // reset before calculating new resistance


            foreach (
                var damResistanceEffect in
                    ActiveSuperNaturalEffects.Where(x => x.EffectType == SuperNaturalEffectType.DamageResistance
                                                         && x.StrengthType == StatusEffectStrengthType.Percentage)) // fixed value not used for resistances)
            {
                CurrentResistance.Value -= damResistanceEffect.Strength / 100f;
            }
            if (CurrentResistance.Value > 1f)
            {
                TransitionToStateStrengthen();
            }
            else if (CurrentResistance.Value < 1f)
            {
                TransitionToStateWeaken();
            }
            else
            {
                TransitionToStateNormal();
            }
        }
        #endregion

        #region public functions
		/// <summary>
		/// Applies the resource effect.
		/// </summary>
		/// <param name="resourceEffect">The resource effect.</param>
		public virtual void ApplyResourceEffect(ResourceEffect resourceEffect)
		{
			ApplyResourceEffect (resourceEffect, false);
			
		}
        /// <summary>
        /// Applies the resource effect.
        /// </summary>
        /// <param name="resourceEffect">The resource effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> then killable is [hit from behind].</param>
        public virtual void ApplyResourceEffect(ResourceEffect resourceEffect, bool hitFromBehind)
        {
            if (resourceEffect == null) return;
            var characterResource = Resources.FirstOrDefault(x => x.ResourceType.Value == resourceEffect.DamageableType);
            if (characterResource == null) return;
            var relativeStrengthEffect = 1f;
            if (hitFromBehind && resourceEffect.UseHitFromBehindModifier)
            {
                relativeStrengthEffect *= resourceEffect.HitFromBehindModifier;
            }
            if (resourceEffect.UseProximityModifier && ProximityModifierFunc != null)
            {
                relativeStrengthEffect = ProximityModifierFunc(resourceEffect.Origin);
            }
            if (resourceEffect.Ticks == 0)
            {
                ApplyResourceEffect(resourceEffect.Strength * relativeStrengthEffect,
                                        resourceEffect.StrengthType,
                                        resourceEffect.DamageableType,
                                        resourceEffect.EffectType,
                                        characterResource);
            }
            else
            {
                // Sets up a timer for ticking resource effects.
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Timed Resource effect");
                timerComponent.TimerMethod = () =>
                {
                    ApplyResourceEffect(resourceEffect.Strength * relativeStrengthEffect,
                        resourceEffect.StrengthType,
                        resourceEffect.DamageableType,
                        resourceEffect.EffectType,
                        characterResource);

                    // Notifiy possible subscribers
                    if (ResourceEffectAppliedAction != null)
                    {
                        ResourceEffectAppliedAction(resourceEffect);
                    }
                };

                timerComponent.SetupIntervalFinite(TimeSpan.FromSeconds(resourceEffect.SecondsBetweenTicks), resourceEffect.Ticks);
                TimerComponents.Add(timerComponent);
            }

            // Notifiy possible subscribers
            if (ResourceEffectAppliedAction != null)
            {
                ResourceEffectAppliedAction(resourceEffect);
            }

        }

        /// <summary>
        /// Applies the continuous resource effect.
        /// Continuous resource effects is a special static resource effect that should be used for constant healing when out of combat etc.
        /// </summary>
        /// <param name="continuousResourceEffect">The continuous resource effect.</param>
        public virtual void ApplyContinuousResourceEffect(ContinuousResourceEffect continuousResourceEffect)
        {
            if (continuousResourceEffect == null) return;
            var characterResource = Resources.FirstOrDefault(x => x.ResourceType.Value == continuousResourceEffect.DamageableType);
            if (characterResource == null) return;
            ApplyResourceEffect(continuousResourceEffect.Strength, continuousResourceEffect.StrengthType, continuousResourceEffect.DamageableType, continuousResourceEffect.EffectType, characterResource);

        }

        /// <summary>
        /// Applies the supernatural effect if there is not a similar, stronger effect already present. Refreshes duration if same effect is present
        /// </summary>
        /// <param name="superNaturalEffect">The super natural effect.</param>
        public virtual void ApplySuperNaturalEffect(SuperNaturalEffect superNaturalEffect)
        {
            var alreadyPresentSimilarEffect = ActiveSuperNaturalEffects.FirstOrDefault(x => x.EffectType == superNaturalEffect.EffectType
                                                                                &&
                                                                                ((x.Strength > 0 && superNaturalEffect.Strength > 0)
                                                                                    ||
                                                                                    (x.Strength < 0 && superNaturalEffect.Strength < 0)));
            if (alreadyPresentSimilarEffect != null)
            {
                if (alreadyPresentSimilarEffect.Strength > superNaturalEffect.Strength || alreadyPresentSimilarEffect.IsInfinite) return;
                // if there an effect of same type already present, and its not stronger, remove it before apply.
                ActiveSuperNaturalEffects.Remove(alreadyPresentSimilarEffect);
            }
            ActiveSuperNaturalEffects.Add(superNaturalEffect);
            if (!superNaturalEffect.IsInfinite)
            {
                // Sets up a count down timer for when to remove the effect
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Supernatural Effects CountDown Timer");
                timerComponent.TimerMethod = () =>
                {
                    ActiveSuperNaturalEffects.Remove(superNaturalEffect);
                    timerComponent.FinishTimer();
                };
                timerComponent.Invoke(superNaturalEffect.Duration);
                TimerComponents.Add(timerComponent);
            }
        }

        /// <summary>
        /// Removes the super natural effect.
        /// </summary>
        /// <param name="superNaturalEffect">The super natural effect.</param>
        public virtual void RemoveSuperNaturalEffect(SuperNaturalEffect superNaturalEffect)
        {
            ActiveSuperNaturalEffects.Remove(superNaturalEffect);
        }

        /// <summary>
        /// Kills this Killable instantly.
        /// </summary>
        public virtual void InstantDeath()
        {
            var health = Resources.FirstOrDefault(x => x.ResourceType.Value == DamageableResourceType.Health);
            if (health != null)
                health.Current.Value = 0;
        }
        
        #region TransitionToStates
        public virtual void TransitionToStateDie()
        {
            if (DyingAction != null)
            {
                DyingAction();
            }
            DamageableCurrentState.Value = DamageableState.Destroyed;
        }

        public virtual void TransitionToStateNormal()
        {
            DamageableCurrentState.Value = DamageableState.Normal;
        }
        public virtual void TransitionToStateInvulnerable()
        {
            DamageableCurrentState.Value = DamageableState.Invulnerable;
        }

        public virtual void TransitionToStateWeaken()
        {
            DamageableCurrentState.Value = DamageableState.Weakened;
        }
        public virtual void TransitionToStateStrengthen()
        {
            DamageableCurrentState.Value = DamageableState.Strengthened;
        }
        #endregion
        #endregion      
    }
}
