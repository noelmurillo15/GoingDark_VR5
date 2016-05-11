using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem.Keyboard
{
    /// <summary>
    /// Keyboard implementation of input axis
    /// </summary>
    public class KeyBoardAxisInput : InputAxisBaseView
    {
        /// <summary>
        /// Gets the axis position.
        /// </summary>
        /// <returns></returns>
        public override Vector2 GetAxisPosition()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}

