using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.RagdollSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// Inherit from this to create a FluidView for a specific game type. I.e 3d and 2d need different implementations etc.
    /// </summary>
    [Serializable]
    public abstract class FluidBaseView : AreaHazardView
    {
        #region Public properties
        // fields and references to be set in the editor
        public float SurfaceTension;
        public float DampeningFactor;
        public float SecondsBetweenAirDamageHits;
        public PushEffectView Buoyancy;
        public ResourceEffectView BreathDamage;
        #endregion

        public Fluid Fluid;
        private TimerTemporaryGameObject _airDamageInterval;

        #region AGS Setup
        public override void InitializeView()
        {
            Fluid = new Fluid(SecondsActive, SecondsRecharging, SecondsBetweenTicks, DeactivateOnTrigger, DestroyOnTrigger, transform,
                SurfaceTension, DampeningFactor, transform.GetComponent<Collider>(), SecondsBetweenAirDamageHits);
            SolveModelDependencies(Fluid);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Fluid.Buoyancy.Value = Buoyancy.PushEffect;
            Fluid.BreathDamage.Value = BreathDamage.ResourceEffect;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            //Fluid.CharacterOnSurfaceAction += HandleCharacterOnSurface; // TODO remove
            Fluid.TransitionToStateActivate();
            Fluid.AreaOfEffect.OnValueChanged += OnAreaOfEffectChanged;
            Fluid.FluidDampeningAction += ApplyFluidDampening;
        }
        #endregion
        #region MonoBehaviour
        public override void Update()
        {
            base.Update();
            // Since it does not make any sense to be able to turn off a fluids buoyancy effect, we can safely use the normal update for this.
            ApplyBuoyancyToTargets();
            CleanUpFluidTargets();
        }
        #endregion

        #region private functions
        /// <summary>
        /// Called when [area of effect changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="areaOfEffect">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnAreaOfEffectChanged(object sender, ActionPropertyEventArgs<AreaOfEffect> areaOfEffect)
        {
            // Subscribe to adds and removals of targets within AreaOfEffect
            areaOfEffect.Value.RagdollTargets.ListItemAdded += RagdollTargetAdded;
            areaOfEffect.Value.RagdollTargets.ListItemRemoved += RagdollTargetRemoved;
            areaOfEffect.Value.MovableTargets.ListItemAdded += MovableTargetAdded;
            areaOfEffect.Value.MovableTargets.ListItemRemoved += MovableTargetRemoved;
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [state enter active].
        /// Set up the intervals for status effects as well as air damage
        /// </summary>
        public override void OnStateEnterActive()
        {
            base.OnStateEnterActive();
            if (Fluid == null) return;
            // Setup an extra time interval for when to apply air damage
            _airDamageInterval = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject);
            if (Fluid.SecondsBetweenTicks > 0)
            {
                _airDamageInterval.TimerMethod = () =>
                {
                    if (Fluid.BreathDamage.Value != null)
                    {
                        Fluid.ApplyBreathDamageToKillableTargets(Fluid.BreathDamage.Value);
                    }
                };
                _airDamageInterval.SetupIntervalInfinite(Fluid.SecondsBetweenAirDamageHits);
            }
            //// Setup a time interval for when to hit targets
            //_effectsInterval = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject);
            //if (Fluid.SecondsBetweenTicks > 0)
            //{
            //    _effectsInterval.TimerMethod = () =>
            //    {
            //        Fluid.ApplyEffectsToTargets();
            //        Fluid.ApplyEffectsToRagdolls();
            //    };
            //    _effectsInterval.SetupIntervalInfinite(Fluid.SecondsBetweenTicks);
            //}

            //// do not toggle to deactivated if SecondsActive == 0
            //if (AreaHazard.SecondsActive > 0)
            //{
            //    var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject);
            //    timerComponent.TimerMethod = () => Fluid.TransitionToStateRecharge();
            //    timerComponent.Invoke(Fluid.SecondsActive);
            //}
        }
        #endregion

        #region public functions
        /// <summary>
        /// Applies the buoyancy push effect to applicable targets
        /// </summary>
        public void ApplyBuoyancyToTargets()
        {
            Fluid.ApplyBuoyancyToCharacterTargets();
            Fluid.ApplyBuoyancyToRagdolls();
            Fluid.ApplyBuoyancyToMovables();
        }

        /// <summary>
        /// Cleans up fluid targets.
        /// </summary>
        public void CleanUpFluidTargets()
        {
            Fluid.CleanupKillableTargets();
            Fluid.CleanupRagdolls();
            Fluid.CleanupMovables();
        }
        #endregion

        #region abstract functions
        /// <summary>
        /// Applies fluid dampening to target transform.
        /// </summary>
        /// <param name="targetTransform">The target transform.</param>
        public abstract void ApplyFluidDampening(Transform targetTransform);

        ///// <summary>
        ///// Handles the character on surface.
        ///// </summary>
        ///// <param name="character">The character.</param>
        //protected abstract void HandleCharacterOnSurface(CharacterBase character); // TODO remove

        /// <summary>
        /// ListItem notification. Ragdoll was added.
        /// </summary>
        /// <param name="ragdollAdd">The ragdoll add.</param>
        protected abstract void RagdollTargetAdded(Ragdoll ragdollAdd);

        /// <summary>
        /// ListItem notification. Ragdoll was removed.
        /// </summary>
        /// <param name="ragdollAdd">The ragdoll add.</param>
        protected abstract void RagdollTargetRemoved(Ragdoll ragdollAdd);

        /// <summary>
        /// ListItem notification. IMovable was added.
        /// </summary>
        /// <param name="movable">The movable.</param>
        protected abstract void MovableTargetAdded(IMovable movable);

        /// <summary>
        /// ListItem notification. IMovable was removed.
        /// </summary>
        /// <param name="movable">The movable.</param>
        protected abstract void MovableTargetRemoved(IMovable movable);
        #endregion
    }
}