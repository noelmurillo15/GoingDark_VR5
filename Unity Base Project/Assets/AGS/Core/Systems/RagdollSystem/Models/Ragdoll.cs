using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Systems.RagdollSystem
{
    /// <summary>
    /// Ragdoll to be used with Units Ragdoll system
    /// </summary>
    public class Ragdoll : ActionModel, IMovable
    {
        #region Properties
        // Constructor properties
        public string PrefabName { get; private set; }
        
        // Subscribable properties
        public ActionProperty<MotionData> InitMotion { get; private set; } // Ragdolls initial motion
        public ActionList<Transform> RigidbodyLimbs { get; private set; } // Ragdolls Rigidbody limbs

        public Transform Transform { get; set; } // The ragdolls transform

        public Action<PushEffect, bool> PushEffectAction { get; set; }
        public Action<Transform> TransformChangedAction { get; set; }
        public Action<MotionData> MotionChangedAction { get; set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Ragdoll"/> class.
        /// </summary>
        /// <param name="prefabName">Name of the ragdoll prefab.</param>
        public Ragdoll(string prefabName)
        {
            PrefabName = prefabName;
            InitMotion = new ActionProperty<MotionData>();
            RigidbodyLimbs = new ActionList<Transform>();
        }

        #region public functions
        /// <summary>
        /// Sets the transform.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public void SetTransform(Transform transform)
        {
            Transform = transform;
        }

        /// <summary>
        /// Sets the motion.
        /// </summary>
        /// <param name="motion">The motion.</param>
        public void SetMotion(MotionData motion)
        {
            InitMotion.Value = motion;
        }

		/// <summary>
		/// Applies the push effect.
		/// </summary>
		/// <param name="pushEffect">The push effect.</param>
		public void ApplyPushEffect(PushEffect pushEffect)
		{
			ApplyPushEffect (pushEffect, false);
		}

        /// <summary>
        /// Applies the push effect.
        /// </summary>
        /// <param name="pushEffect">The push effect.</param>
        /// <param name="hitFromBehind">if set to <c>true</c> [hit from behind].</param>
        public void ApplyPushEffect(PushEffect pushEffect, bool hitFromBehind)
        {
            PushEffectAction(pushEffect, hitFromBehind);
        }

        /// <summary>
        /// Applies the movement effect.
        /// </summary>
        /// <param name="movementEffect">The movement effect.</param>
        public void ApplyMovementEffect(MovementEffect movementEffect)
        {
            // Dummy. Implement if need to apply movement effects to ragdolls (not very likely)
        }
        #endregion
    }
}
