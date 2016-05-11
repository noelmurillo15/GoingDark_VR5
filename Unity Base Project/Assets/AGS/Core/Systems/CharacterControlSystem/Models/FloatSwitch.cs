using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterControlSystem
{

    /// <summary>
    /// Simple Input switch that can be used to determine input based on a changing float value, for example mouse wheel scrolling.
    /// </summary>
    public class FloatSwitch : ActionModel
    {

        #region Properties
        // Subscribable properties
        public ActionProperty<float> InputValue { get; private set; } // Current float value of this 1D axis
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatSwitch"/> class.
        /// </summary>
        public FloatSwitch()
        {
            InputValue = new ActionProperty<float>();
		}
    }
}
