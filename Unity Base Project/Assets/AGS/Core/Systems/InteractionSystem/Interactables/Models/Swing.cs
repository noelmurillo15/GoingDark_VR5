using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// A swing is a chain of linked SwingUnits
    /// </summary>
    public class Swing : ActionModel
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<bool> IsStill { get; private set; } // Are all SwingUnits still?
        public ActionList<SwingUnit> SwingUnits { get; private set; } // Owned swing units

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Swing"/> class.
        /// </summary>
        public Swing()
        {
            IsStill = new ActionProperty<bool>();
            SwingUnits = new ActionList<SwingUnit>();
            SwingUnits.ListItemAdded += SwingUnitAdded;
        }

        #region private functions
        /// <summary>
        /// ListItem notification. SwingUnit was added.
        /// </summary>
        /// <param name="swingUnitAdd">The swing unit add.</param>
        private void SwingUnitAdded(SwingUnit swingUnitAdd)
        {
            swingUnitAdd.OwnerSwing.Value = this;
        }
        #endregion

        #region public functions
        /// <summary>
        /// Stops the swing.
        /// </summary>
        public void StopSwing()
        {
            foreach (var swingUnit in SwingUnits)
            {
                swingUnit.TransitionToStateReduceSpeed();
            }
        }

        /// <summary>
        /// Lets the SwingUnits swing naturally.
        /// </summary>
        public void LetSwing()
        {
            foreach (var swingUnit in SwingUnits)
            {
                swingUnit.TransitionToStateSwingNatural();
            }
        }
        #endregion
    }
}
