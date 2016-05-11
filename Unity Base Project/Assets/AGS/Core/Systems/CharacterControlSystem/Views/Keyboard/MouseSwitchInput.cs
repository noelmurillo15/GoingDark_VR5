using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem.Keyboard
{
    public class MouseSwitchInput : FloatSwitchBaseView
    {
        /// <summary>
        /// Gets the switch input value.
        /// </summary>
        /// <returns></returns>
        public override float GetInputValue()
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
    }
}
