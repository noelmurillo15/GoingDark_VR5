using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Base
{
    /// <summary>
    /// Interaction volumes are used to scan for interactables throughout the game level. All of its work is done in the InteractionVolumeView
    /// </summary>
    public class InteractionVolume : ActionModel
    {
        #region Properties
        // Constructor properties
        public InteractionTargetHeight TargetHeight { get; private set; }

        // Subscribable properties
        public ActionProperty<InteractionSkills> OwnerInteractionSkills; // Reference to owner interaction skills
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionVolume"/> class.
        /// </summary>
        /// <param name="targetHeight">InteractionTargetHeight of the target.</param>
        public InteractionVolume(InteractionTargetHeight targetHeight)
        {
            TargetHeight = targetHeight;
            OwnerInteractionSkills = new ActionProperty<InteractionSkills>();
        }
    }
}
