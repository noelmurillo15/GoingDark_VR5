using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// PickUpItemBase is a base model for all types of pick ups. It provides the PickUp and SetActive functions
    /// </summary>
    public abstract class PickUpItemBase : ActionModel
    {
        #region Properties

        public Action PickUpAction { get; set; }
        public Action<bool> SetActiveAction { get; set; }
        #endregion Properties

        #region public functions

        /// <summary>
        /// Notify any subscribers that this was picked up.
        /// </summary>
        public void PickUp()
        {
            if (PickUpAction != null)
            {
                PickUpAction();    
            }
            
        }

        /// <summary>
        /// Notify any subscribers with value a where this pick should be active.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void SetActive(bool value)
        {
            if (SetActiveAction != null)
            {
                SetActiveAction(value);
            }

        }
        #endregion
    }
}
