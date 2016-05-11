using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// Switch
    /// </summary>
    public class Switch : InteractableBase
    {

        #region Properties
        // Constructor properties
        public bool ReLockable { get; private set; }
        public float SecondsSwitching { get; private set; }

        // Subscribable properties
        public ActionProperty<UnlockableObject> OwnerUnlockableObject { get; private set; } // Reference to owner unlockable object
        public ActionProperty<bool> On { get; private set; } // Determines if this switch is on/off
        #endregion Properties

        private bool _unLocked;

        /// <summary>
        /// Initializes a new instance of the <see cref="Switch"/> class.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="reLockable">if set to <c>true</c> [re lockable]. Is it possible to switch this off again after its been switched on?</param>
        /// <param name="secondsSwitching">The seconds switching determines the time it takes to switch</param>
        public Switch(Transform transform, bool reLockable, float secondsSwitching)
            : base(transform)
        {
            OwnerUnlockableObject = new ActionProperty<UnlockableObject>();
            ReLockable = reLockable;
            SecondsSwitching = secondsSwitching;
            On = new ActionProperty<bool>();
        }

        #region public functions
        /// <summary>
        /// Activates the switch with value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void Activate(bool value)
        {
            // Sets up a timer for when to switch after activation.
            var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Switching timer");
            timerComponent.TimerMethod = () =>
            {
                if (!_unLocked && value)
                {
                    _unLocked = true;
                }
                else
                {
                    _unLocked = false;
                }
                On.Value = _unLocked;
            };
            timerComponent.Invoke(SecondsSwitching);
            TimerComponents.Add(timerComponent);
        }
        #endregion
    }
}
