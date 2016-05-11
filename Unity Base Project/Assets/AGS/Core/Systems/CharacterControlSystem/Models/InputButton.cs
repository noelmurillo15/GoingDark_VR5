using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// InputButtons can be used for with Unitys GetButton, GetButtonDown and GetButtonUp
    /// as well as custom button triggers DoubleTap and DoubleTapAndHold.
    /// </summary>
    public class InputButton : ActionModel
    {
        #region Properties
        // Constructor properties
        public string InputAxisName { get; private set; }
        public InputButtonType InputButtonType { get; private set; }

        // Subscribable properties
        public ActionProperty<bool> IsOn { get; private set; } // Is this button currently pressed?
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InputButton"/> class.
        /// </summary>
        /// <param name="inputAxisName">Name of the input axis in the Unity editor input settings.</param>
        /// <param name="inputButtonType">Type of the input button.</param>
		public InputButton (string inputAxisName, InputButtonType inputButtonType) {
            InputAxisName = inputAxisName;
		    InputButtonType = inputButtonType;
            IsOn = new ActionProperty<bool>();            
		}
	}
}
