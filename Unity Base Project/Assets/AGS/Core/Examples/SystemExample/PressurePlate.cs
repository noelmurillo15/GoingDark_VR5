using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Examples.SystemExample
{
    /// <summary>
    /// PressurePlate example model
    /// </summary>
    public class PressurePlate : ActionModel
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<bool> IsPressured { get; set; }
        #endregion Properties

        public PressurePlate()
        {
            IsPressured = new ActionProperty<bool>();
        }

        #region public functions

        /// <summary>
        /// Call this stepping on the pressure plate
        /// </summary>
        public void TriggerMechanism(bool on)
        {
            IsPressured.Value = on;            
        }
        #endregion
    }
}
