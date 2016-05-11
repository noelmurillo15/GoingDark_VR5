using System;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// SwingUnit
    /// </summary>
    public class SwingUnit : InteractableBase
    {
		#region Properties
        // Constructor properties
        public bool IsBaseUnit { get; private set; }
        public bool IsEndUnit { get; private set; }

		// Subscribable properties
        public ActionProperty<Swing> OwnerSwing; // Reference to owner swing
        public ActionProperty<bool> IsGrabbed { get; private set; } // Is this unit currently grabbed by a character?
        public ActionProperty<SwingUnitState> SwingUnitCurrentState { get; private set; } // Current state of the unit

        // For notying the view of incoming force
        public Action<Vector3> AddForceAction { get; set; }
		#endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="SwingUnit"/> class.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="isBaseUnit">if set to <c>true</c> [is base unit].</param>
        /// <param name="isEndUnit">if set to <c>true</c> [is end unit].</param>
		public SwingUnit (Transform transform, bool isBaseUnit, bool isEndUnit)
            : base(transform)
        {
            OwnerSwing = new ActionProperty<Swing>();
            IsBaseUnit = isBaseUnit;
            IsEndUnit = isEndUnit;
            IsGrabbed = new ActionProperty<bool>();
            SwingUnitCurrentState = new ActionProperty<SwingUnitState>();
		}

        #region state transitions
        /// <summary>
        /// Transitions to state idle.
        /// </summary>
        public void TransitionToStateIdle()
        {
            SwingUnitCurrentState.Value = SwingUnitState.Idle;
        }

        /// <summary>
        /// Transitions to state swing natural.
        /// </summary>
        public void TransitionToStateSwingNatural()
        {
            SwingUnitCurrentState.Value = SwingUnitState.SwingingNatural;
        }

        /// <summary>
        /// Transitions to state reduce speed.
        /// </summary>
        public void TransitionToStateReduceSpeed()
        {
            SwingUnitCurrentState.Value = SwingUnitState.ReducingSpeed;
        }

        #endregion

        /// <summary>
        /// Grabs this SwingUnit.
        /// </summary>
        public void Grab()
        {
            IsGrabbed.Value = true;
        }

        /// <summary>
        /// Releases this SwingUnit.
        /// </summary>
        public void Release()
        {
            IsGrabbed.Value = false;
        }

        /// <summary>
        /// Adds the force to this SwingUnit.
        /// </summary>
        /// <param name="forceToAdd">The force to add.</param>
        public void AddForce(Vector3 forceToAdd)
        {
            AddForceAction(forceToAdd);
        }
    }
}
