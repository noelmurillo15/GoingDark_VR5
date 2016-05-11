using AGS.Core.Classes.ActionProperties;
using AGS.Core.Interfaces;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Classes.Helpers
{
    /// <summary>
    /// Static helper to apply damage effect to IDamageable objects, and Movement type effects to IMovable objects
    /// </summary>
    public static class CombatHelper
    {
        /// <summary>
        /// Calculate if hitting target from behind
        /// </summary>
        /// <param name="sourceTransform">The source transform.</param>
        /// <param name="targetDamageable">The target damageable.</param>
        /// <returns></returns>
        public static bool HittingFromBehind(Transform sourceTransform, IDamageable targetDamageable)
        {
            if (sourceTransform == null || targetDamageable.Transform == null) return false;
            var targetForwardDirection = targetDamageable.Transform.TransformDirection(Vector3.forward);
            var distanceToTarget = targetDamageable.Transform.position - sourceTransform.position;
            var hittingFromBehind = Vector3.Dot(targetForwardDirection, distanceToTarget) > 0;
            return hittingFromBehind;
        }

        /// <summary>
        /// Calculate if hitting target from behind
        /// </summary>
        /// <param name="sourceTransform">The source transform.</param>
        /// <param name="targetMovable">The target movable.</param>
        /// <returns></returns>
        public static bool HittingFromBehind(Transform sourceTransform, IMovable targetMovable)
        {
            if (sourceTransform == null || targetMovable.Transform == null) return false;
            var ownerForwardDirection = sourceTransform.TransformDirection(Vector3.forward);
            var distanceToTarget = targetMovable.Transform.position - sourceTransform.position;
            var hittingFromBehind = Vector3.Dot(ownerForwardDirection, distanceToTarget) < 0;
            return hittingFromBehind;
        }

        /// <summary>
        /// Applies the resource effects.
        /// </summary>
        /// <param name="damageable">The damageable.</param>
        /// <param name="resourceEffects">The resource effects.</param>
        /// <param name="hittingFromBehind">if set to <c>true</c> then [hitting from behind].</param>
        public static void ApplyResourceEffects(IDamageable damageable, ActionList<ResourceEffect> resourceEffects, bool hittingFromBehind)
        {
            foreach (var resourceEffect in resourceEffects)
            {
                damageable.ApplyResourceEffect(resourceEffect, hittingFromBehind);
            }
        }

        /// <summary>
        /// Applies the super natural effects.
        /// </summary>
        /// <param name="killable">The killable.</param>
        /// <param name="superNaturalEffects">The super natural effects.</param>
        public static void ApplySuperNaturalEffects(KillableBase killable, ActionList<SuperNaturalEffect> superNaturalEffects)
        {
            foreach (var superNaturalEffect in superNaturalEffects)
            {
                killable.ApplySuperNaturalEffect(superNaturalEffect);
            }
        }

		/// <summary>
		/// Applies the push effects.
		/// </summary>
		/// <param name="movable">The movable.</param>
		/// <param name="pushEffects">The push effects.</param>
		public static void ApplyPushEffects(IMovable movable, ActionList<PushEffect> pushEffects)
		{
			ApplyPushEffects (movable, pushEffects, false);
		}

        /// <summary>
        /// Applies the push effects.
        /// </summary>
        /// <param name="movable">The movable.</param>
        /// <param name="pushEffects">The push effects.</param>
        /// <param name="hittingFromBehind">if set to <c>true</c> then [hitting from behind].</param>
        public static void ApplyPushEffects(IMovable movable, ActionList<PushEffect> pushEffects, bool hittingFromBehind)
        {
            foreach (var pushEffect in pushEffects)
            {
                movable.ApplyPushEffect(pushEffect, hittingFromBehind);
            }
        }

        /// <summary>
        /// Applies the movementffects.
        /// </summary>
        /// <param name="characterBase">The character.</param>
        /// <param name="movementEffects">The movement effects.</param>
        public static void ApplyMovementffects(CharacterBase characterBase, ActionList<MovementEffect> movementEffects)
        {
            foreach (var movementEffect in movementEffects)
            {
                characterBase.ApplyMovementEffect(movementEffect);
            }

        }
    }

}
