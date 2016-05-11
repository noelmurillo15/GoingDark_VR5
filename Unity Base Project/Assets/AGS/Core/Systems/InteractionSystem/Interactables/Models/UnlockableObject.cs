using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// UnlockableObject
    /// </summary>
    public class UnlockableObject : ActionModel
    {
        #region Properties
        // Constructor properties
        public bool ReLockable { get; private set; }

        // Subscribable properties
        public ActionProperty<bool> Active { get; private set; } // This is true if all owned Switches are switched on, otherwise false
        public ActionList<Switch> Switches { get; private set; } // Owned switches

        private bool _unLocked;

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlockableObject"/> class.
        /// </summary>
        /// <param name="reLockable">if set to <c>true</c> [re lockable]. Is it possible to lock this again after its been unlocked?</param>
        public UnlockableObject(bool reLockable)
        {
            Active = new ActionProperty<bool>();
            ReLockable = reLockable;
            Switches = new ActionList<Switch>();
            Switches.ListItemAdded += SwitchAdded;

        }

        #region private functions
        /// <summary>
        /// ListItem notification. Switch was added
        /// </summary>
        /// <param name="switchAdd">The switch add.</param>
        private void SwitchAdded(Switch switchAdd)
        {
            switchAdd.OwnerUnlockableObject.Value = this;
            switchAdd.On.OnValueChanged += (sender, on) => CheckSwitchesOn(); // Whenever one switch changes on/off we check all switches again
        }

        /// <summary>
        /// Check all switches to see if we should unlock this.
        /// </summary>
        private void CheckSwitchesOn()
        {
            var allSwitchesOn = Switches.All(x => x.On.Value);
            if (!_unLocked && allSwitchesOn)
            {
                _unLocked = true;
                Active.Value = _unLocked;
            }
            else if (Active.Value && !allSwitchesOn && ReLockable)
            {
                _unLocked = false;
                Active.Value = _unLocked;
            }

        }
        #endregion
    }
}
