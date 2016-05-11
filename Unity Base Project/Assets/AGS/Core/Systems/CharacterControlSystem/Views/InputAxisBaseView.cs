using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// InputAxisBaseView. For Joystick implementations, use the DeadZoneRadius to neglect small changes on axis, and MaxDistanceRadius to determine where full speed on axis is
    /// </summary>
    [Serializable]
	public abstract class InputAxisBaseView : ActionView
    {

        #region Public properties
        // Fields to be set in the editor
        public float DeadZoneRadius;
	    public float MaxDistanceRadius;
		public InputAxis InputAxis;
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            InputAxis = new InputAxis(DeadZoneRadius, MaxDistanceRadius);
            SolveModelDependencies(InputAxis);
        }

        #endregion

        #region MonoBehaviours

        public override void Update()
        {
            base.Update();
            InputAxis.AxisPosition.Value = GetAxisPosition();
        }

        #endregion
        /// <summary>
        /// Gets the axis position.
        /// </summary>
        /// <returns></returns>
        public abstract Vector2 GetAxisPosition();


	}
}