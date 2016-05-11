using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.AISystem
{
    /// <summary>
    /// Each DetectionVolume determines if it can detect the player on its own
    /// </summary>
    public abstract class DetectionVolumeBase : ActionModel
    {        
        #region Properties
        // Subscribable properties
        public ActionProperty<AI> OwnerAI { get; private set; } // Reference to the owner AI
        public ActionProperty<bool> IsDetectingPlayer { get; private set; } // Is this detection volume currently detecting the player?
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="DetectionVolumeBase"/> class.
        /// </summary>
        protected DetectionVolumeBase()
        {
            OwnerAI = new ActionProperty<AI>();
            IsDetectingPlayer = new ActionProperty<bool>();
        }

    }
}
