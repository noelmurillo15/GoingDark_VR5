using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// Base class for interactable objects. Inherit this for anything that should be interacted with by an interaction skill
    /// </summary>
    public class InteractableBase : ActionModel
    {
        #region Properties
        // Subscribable properties
        public Transform Transform { get; set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractableBase"/> class.
        /// </summary>
        /// <param name="transform">The interactbles transform.</param>
        public InteractableBase(Transform transform)
        {
            Transform = transform;
        }
    }
}
