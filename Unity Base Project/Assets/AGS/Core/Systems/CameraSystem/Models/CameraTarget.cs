using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CameraSystem
{
    /// <summary>
    /// CameraTarget can either follow the player or the players instaniated ragdoll.
    /// </summary>
    public class CameraTarget : ActionModel
    {

        #region Properties
        // Constructor properties
        public Vector3 Offset { get; set; }

        // Subscribable properties
        public ActionProperty<bool> FollowRagdoll; // Should the camera follow the player or the players ragdoll?
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraTarget"/> class.
        /// </summary>
        /// <param name="offset">The camera offset.</param>
        public CameraTarget(Vector3 offset)
        {
            Offset = offset;
            FollowRagdoll = new ActionProperty<bool>() { Value = false };
        }

        #region public functions
        /// <summary>
        /// Set camera to follow the player.
        /// </summary>
        public void CameraFollowPlayer()
        {
            FollowRagdoll.Value = false;
        }

        /// <summary>
        /// Set camera to follow the ragdoll.
        /// </summary>
        public void CameraFollowRagdoll()
        {
            FollowRagdoll.Value = true;
        }
        #endregion
    }
}
