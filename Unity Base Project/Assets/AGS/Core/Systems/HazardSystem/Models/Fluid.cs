using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// Fluids are advanced AreaHazards with several fluid variables.
    /// It simulates buoyancy to its targets with a constant push effect, and also handles out breath damage if the target below surface and has an air supply.
    /// </summary>
    public class Fluid : AreaHazard
    {
        #region Properties
        // Constructor properties
        public float SurfaceTension { get; private set; } // 
        public float DampeningFactor { get; private set; } // 
        public float SurfaceLevel { get; private set; }
        public Collider Collider { get; private set; } // 
        public float SecondsBetweenAirDamageHits { get; private set; } // 


        // Subscribable properties
        public ActionProperty<PushEffect> Buoyancy { get; private set; } // Buoyancy power of this fluid is based on continuous push effects
        public ActionProperty<ResourceEffect> BreathDamage { get; private set; } // Determines how fast killables with air supply drowns

        // For notifing the FluidView to handle out dampening
        public Action<Transform> FluidDampeningAction { get; set; }
        
        //// For notifing the FluidView that a character hit the surface
        //public Action<CharacterBase> CharacterOnSurfaceAction { get; set; } // TODO remove
        
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Fluid"/> class.
        /// </summary>
        /// <param name="secondsActive">The seconds active.</param>
        /// <param name="secondsRecharging">The seconds recharging.</param>
        /// <param name="secondsBetweenTicks">The seconds between ticks.</param>
        /// <param name="deactivateOnTrigger">if set to <c>true</c> [deactivate on trigger].</param>
        /// <param name="destroyOnTrigger">if set to <c>true</c> [destroy on trigger].</param>
        /// <param name="transform">The transform.</param>
        /// <param name="surfaceTension">The surface tension determines an offset of where the target hits the surface</param>
        /// <param name="dampeningFactor">The dampening factor determines how "heavy" the fluid is, i.e how hard is it to move in</param>
        /// <param name="collider">Reference to the collider in the scene view..</param>
        /// <param name="secondsBetweenAirDamageHits">Determines how often the Fluid applies effects to its victims</param>
        public Fluid(float secondsActive, float secondsRecharging, float secondsBetweenTicks, bool deactivateOnTrigger, bool destroyOnTrigger, Transform transform,
                    float surfaceTension, float dampeningFactor, Collider collider, float secondsBetweenAirDamageHits)
            : base(secondsActive, secondsRecharging, secondsBetweenTicks, deactivateOnTrigger, destroyOnTrigger, transform)
        {
            SurfaceTension = surfaceTension;
            DampeningFactor = dampeningFactor;
            Collider = collider;
            SurfaceLevel = collider.bounds.max.y;
            SecondsBetweenAirDamageHits = secondsBetweenAirDamageHits;
            Buoyancy = new ActionProperty<PushEffect>();
            BreathDamage = new ActionProperty<ResourceEffect>();
        }

        #region private functions
        private bool TargetIsOnSurface(Transform transform)
        {
            if (transform == null
                ||
                transform.GetComponent<Collider>() == null) return false;
            var distanceToSurface = transform.GetComponent<Collider>().bounds.max.y - (SurfaceLevel + SurfaceTension);
            return distanceToSurface > 0f;
        }

        /// <summary>
        /// Applies possible fluid dampening.
        /// </summary>
        /// <param name="targetTransform">The target transform.</param>
        private void ApplyFluidDampening(Transform targetTransform)
        {
            FluidDampeningAction(targetTransform);
        }
        #endregion

        #region public functions
        /// <summary>
        /// Applies the breath damage to killable targets.
        /// </summary>
        /// <param name="breathDamage">The breath damage.</param>
        public void ApplyBreathDamageToKillableTargets(ResourceEffect breathDamage)
        {
            if (AreaOfEffect.Value == null) return;
            // Apply breath damage to applicable targets that are below surface
            foreach (var target in AreaOfEffect.Value.KillableTargets.Where(target => !TargetIsOnSurface(target.Transform)))
            {
                target.ApplyResourceEffect(breathDamage);
            }
        }

        /// <summary>
        /// Cleanups the killable targets.
        /// Removes targets that was killed while below surface.
        /// </summary>
        public void CleanupKillableTargets()
        {
            if (AreaOfEffect.Value == null) return;
            if (AreaOfEffect.Value.KillableTargets.All(x => x.Transform != null)) return;

            for (var i = 0; i < AreaOfEffect.Value.KillableTargets.Count; i++)
            {
                if (AreaOfEffect.Value.KillableTargets[i].Transform == null)
                {
                    AreaOfEffect.Value.KillableTargets.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Cleanups the ragdolls.
        /// Removes ragdoll targets that was removed from the scene while below surface.
        /// </summary>
        public void CleanupRagdolls()
        {
            if (AreaOfEffect.Value == null) return;
            if (AreaOfEffect.Value.RagdollTargets.All(x => x != null && x.RigidbodyLimbs != null && x.RigidbodyLimbs.All(y => y != null && y.transform != null))) return;

            for (var i = 0; i < AreaOfEffect.Value.RagdollTargets.Count; i++)
            {
                if (AreaOfEffect.Value.RagdollTargets[i] == null || AreaOfEffect.Value.RagdollTargets[i].RigidbodyLimbs == null || AreaOfEffect.Value.RagdollTargets[i].RigidbodyLimbs.Any(y => y == null || y.transform == null))
                {
                    AreaOfEffect.Value.RagdollTargets.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// Cleanups the movables.
        /// Removes movable targets that was removed from the scene while below surface.
        /// </summary>
        public void CleanupMovables()
        {
            if (AreaOfEffect.Value == null) return;
            if (AreaOfEffect.Value.MovableTargets.All(x => x.Transform != null)) return;

            for (var i = 0; i < AreaOfEffect.Value.MovableTargets.Count; i++)
            {
                if (AreaOfEffect.Value.MovableTargets[i].Transform == null)
                {
                    AreaOfEffect.Value.MovableTargets.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Applies the buoyancy to push effect to applicable character targets.
        /// </summary>
        public void ApplyBuoyancyToCharacterTargets()
        {
            if (AreaOfEffect.Value == null) return;
            foreach (var target in AreaOfEffect.Value.KillableTargets)
            {
                if (target.Transform == null) continue;

                var character = target as CharacterBase;
                if (character == null) return;

                if (TargetIsOnSurface(target.Transform))
                {
                    // target is on or above surface, notify views to zero out its vertical velocity to prevent movement jitter on surface
                    //CharacterOnSurfaceAction(character); // TODO remove and change comment above
                    continue;
                }                
                character.ApplyPushEffect(Buoyancy.Value);
            }
        }

        /// <summary>
        /// Applies buoyancy push effect to ragdoll targets.
        /// </summary>
        public void ApplyBuoyancyToRagdolls()
        {
            if (AreaOfEffect.Value == null) return;
            foreach (var ragdollTarget in AreaOfEffect.Value.RagdollTargets)
            {
                if (ragdollTarget == null || !ragdollTarget.RigidbodyLimbs.Any()) continue;

                foreach (var physicsLimb in ragdollTarget.RigidbodyLimbs)
                {
                    if (physicsLimb == null) return;
                    
                    if (TargetIsOnSurface(physicsLimb))
                    {
                        // target is on or above surface, zero out its vertical velocity to prevent movement jitter and continue
                        physicsLimb.GetComponent<Rigidbody>().useGravity = true;
                        var targetVelocity = physicsLimb.GetComponent<Rigidbody>().velocity;
                        if (targetVelocity.y > 0f)
                        {

                            targetVelocity = new Vector3(targetVelocity.x, 0f, targetVelocity.z);
                            physicsLimb.GetComponent<Rigidbody>().velocity = targetVelocity;
                        }

                    }
                    else
                    {
                        physicsLimb.GetComponent<Rigidbody>().useGravity = false;
                    }
                    ApplyFluidDampening(physicsLimb);
                }
                
                ragdollTarget.ApplyPushEffect(Buoyancy.Value);
            }

        }

        /// <summary>
        /// Applies the buoyancy to movables.
        /// </summary>
        public void ApplyBuoyancyToMovables()
        {
            if (AreaOfEffect.Value == null) return;
            foreach (var movable in AreaOfEffect.Value.MovableTargets)
            {
                if (movable == null) continue;
                ApplyFluidDampening(movable.Transform);
                if (TargetIsOnSurface(movable.Transform))
                {
                    // target is on or above surface, zero out its vertical velocity to prevent movement jitter and continue
                    var targetVelocity = movable.Transform.GetComponent<Rigidbody>().velocity;
                    if (targetVelocity.y > 0f)
                    {
                        targetVelocity = new Vector3(targetVelocity.x, 0f, targetVelocity.z);
                        movable.Transform.GetComponent<Rigidbody>().velocity = targetVelocity;
                    }
                    continue;
                }

                movable.ApplyPushEffect(Buoyancy.Value);
            }

        }
        #endregion
    }
}
