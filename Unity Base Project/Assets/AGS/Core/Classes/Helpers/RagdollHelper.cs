using UnityEngine;

namespace AGS.Core.Classes.Helpers
{
    /// <summary>
    /// Static helper to sync instantiated ragdoll limbs, and transferring motion to them
    /// </summary>
    public static class RagdollHelper
    {
        /// <summary>
        /// Synchronizes one transforms position with another transforms position.
        /// </summary>
        /// <param name="from">From Transform</param>
        /// <param name="to">To Transform</param>
        public static void SyncTransformsRecursively(Transform from, Transform to)
        {
            if (from == null || from.transform == null || from.transform.childCount <= 0) return;

            foreach (Transform fromTransform in from)
            {
                var toTransform = to.transform.FindChild(fromTransform.name);
                if (toTransform == null) continue;

                SyncTransformsRecursively(fromTransform, toTransform);
                toTransform.localPosition = fromTransform.localPosition;
                toTransform.localRotation = fromTransform.localRotation;
            }
        }

        /// <summary>
        /// Synchronizes the momentum recursively.
        /// </summary>
        /// <param name="toSync">Transform to synchronize.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="angularVelocity">The angular velocity.</param>
        public static void SyncMomentumRecursively(Transform toSync, Vector3 velocity, Vector3 angularVelocity)
        {
            if (toSync == null) return;
            foreach (Transform limbTransform in toSync)
            {
                if (limbTransform == null) return;
                SyncMomentumRecursively(limbTransform, velocity, angularVelocity);
                var limbRigidbody = limbTransform.GetComponent<Rigidbody>();
                if (limbRigidbody == null) continue;
                limbRigidbody.velocity = velocity;
                limbRigidbody.angularVelocity = angularVelocity;
            }
        }
    }

}

