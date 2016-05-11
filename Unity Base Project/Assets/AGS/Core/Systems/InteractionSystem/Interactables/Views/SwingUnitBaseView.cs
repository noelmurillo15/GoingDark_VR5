using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// BaseView for SwingUnits. Requires an animator for state behaviours, and is responsible for setting the animator in the correct state. 
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Animator))]
    public abstract class SwingUnitBaseView : ActionView
    {
        #region Public properties
        // fields to be set in the editor
		public bool BaseUnit; // Tick for the first unit in the swing
        public bool EndUnit; // Tick for the last unit in the swing
		#endregion

		public SwingUnit SwingUnit;
        private Animator _animator;
        protected Vector3 StartPos; // This position is set when the scene is starting, for help determining if the swing unit is at its starting position at a later stage

        #region AGS Setup
        public override void InitializeView()
        {
            SwingUnit = new SwingUnit(transform, BaseUnit, EndUnit);   
            SolveModelDependencies(SwingUnit);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            SwingUnit.Transform = transform;
            StartPos = transform.position; // Set the start pos
        }

		public override void InitializeActionModel(ActionModel model)
        {
			base.InitializeActionModel(model);
            // State subscriber
            SwingUnit.SwingUnitCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
            // Incoming force subscriber
			SwingUnit.AddForceAction += AddSwingForce;
        }
        #endregion

        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [current state changed].
        /// Sets mecanim animator in the correct state
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(SwingUnitState currentState)
        {
            /* SwingUnitStates 
             * Idle == 0
             * SwingingNatural == 1
             * ReducingSpeed == 2 */
            _animator.SetInteger("SwingUnitState", (int)currentState);
        }

        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public abstract void OnStateEnterIdle();

        /// <summary>
        /// Called when [state update idle].
        /// </summary>
        public abstract void OnStateUpdateIdle();

        /// <summary>
        /// Called when [state enter reducing speed].
        /// </summary>
        public abstract void OnStateEnterReducingSpeed();

        /// <summary>
        /// Called when [state update reducing speed].
        /// </summary>
        public abstract void OnStateUpdateReducingSpeed();

        /// <summary>
        /// Called when [state enter swinging natural].
        /// </summary>
        public abstract void OnStateEnterSwingingNatural();

        /// <summary>
        /// Called when [state update swinging natural].
        /// </summary>
        public abstract void OnStateUpdateSwingingNatural();
        #endregion

        #region private functions
        /// <summary>
        /// Adds the swing force.
        /// </summary>
        /// <param name="forceToAdd">The force to add.</param>
        protected abstract void AddSwingForce(Vector3 forceToAdd);
        #endregion
    }
}