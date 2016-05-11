using System;
using System.Collections.Generic;
using System.Linq;
using AGS.Core.Classes.Helpers;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.RagdollSystem
{
    /// <summary>
    /// Add this to a Unity Ragdoll
    /// </summary>
    [Serializable]
	public class RagdollView : ActionView
    {
        public Ragdoll Ragdoll;

        #region AGS Setup
        public override void InitializeView()
        {
            if (Ragdoll == null)
            {
                Ragdoll = new Ragdoll("Prefab name missing");
            }
            SolveModelDependencies(Ragdoll);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            foreach (var ragdollPhysicsLimb in transform.GetComponentsInChildren<Transform>()
                    .Where(y => y.GetComponent<Rigidbody>() != null).ToArray())
            {
                Ragdoll.RigidbodyLimbs.Add(ragdollPhysicsLimb);
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            Ragdoll.PushEffectAction += HandlePushEffects;
        }
        #endregion


        #region private functions

        /// <summary>
        /// Handles the push effects. Checks if push effect has additional ticks.
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> ragdoll is [hit from behind].</param>
        private void HandlePushEffects(PushEffect pushEffect, bool hitFromBehind)
        {
            if (!Ragdoll.RigidbodyLimbs.Any()) return;

            if (pushEffect == null) return;

            if (pushEffect.Ticks == 0)
            {
                ApplyPushEffectToTargets(Ragdoll.RigidbodyLimbs, pushEffect);
            }
            else
            {
                // set up a timer for ticking push effects
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject);
                timerComponent.TimerMethod = () => ApplyPushEffectToTargets(Ragdoll.RigidbodyLimbs, pushEffect);
                timerComponent.SetupIntervalFinite(TimeSpan.FromSeconds(pushEffect.SecondsBetweenTicks), pushEffect.Ticks);    
            }
            

        }

        /// <summary>
        /// Applies the push effect to all Ridigbody targets.
        /// </summary>
        /// <param name="rigidBodyTransforms">The Rigidbody transforms.</param>
        /// <param name="pushEffect">The push effect.</param>
        private void ApplyPushEffectToTargets(IEnumerable<Transform> rigidBodyTransforms, PushEffect pushEffect)
        {
            foreach (var targetTransform in rigidBodyTransforms)
            {
                ApplyPushEffect(targetTransform, pushEffect);
            }
        }

        /// <summary>
        /// Applies the push effect.
        /// </summary>
        /// <param name="transformTarget">The transform target.</param>
        /// <param name="pushEffect">The push effect.</param>
        private static void ApplyPushEffect(Transform transformTarget, PushEffect pushEffect)
        {
            Vector3 forceToAdd;
            switch (pushEffect.Direction)
            {
                case VectorDirection.Forward:
                    forceToAdd = transformTarget.forward * pushEffect.Strength;
                    break;
                case VectorDirection.Back:
                    forceToAdd = -transformTarget.forward * pushEffect.Strength;
                    break;
                case VectorDirection.Up:
                    forceToAdd = Vector3.up * pushEffect.Strength;
                    break;
                case VectorDirection.Down:
                    forceToAdd = -Vector3.up * pushEffect.Strength;
                    break;
                default:
                    forceToAdd = Vector3.zero;
                    break;
            }
            transformTarget.GetComponent<Rigidbody>().AddForce(forceToAdd, ForceConverter.ForceTypeToUnityForceMode(pushEffect.ForceType));
        }
        #endregion

        #region public functions
        /// <summary>
        /// Sets the Ragdolls transform.
        /// </summary>
        public void SetTransform()
        {
            if (Ragdoll.Transform == null)
            {
                return;
            }
            transform.position = Ragdoll.Transform.position;
            transform.rotation = Ragdoll.Transform.rotation;
            RagdollHelper.SyncTransformsRecursively(Ragdoll.Transform, transform);
        }

        /// <summary>
        /// Sets the Ragdolls motion.
        /// </summary>
        public void SetMotion()
        {
            if (Ragdoll.InitMotion.Value == null)
            {
                return;
            }
            RagdollHelper.SyncMomentumRecursively(transform, Ragdoll.InitMotion.Value.Velocity, Ragdoll.InitMotion.Value.AngularVelocity);
            //Ragdoll.MotionSynced.Value = true;
        }
        #endregion
	}
}