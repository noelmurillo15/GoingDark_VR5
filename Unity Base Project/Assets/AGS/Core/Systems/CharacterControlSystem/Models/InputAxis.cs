using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CharacterControlSystem
{
    /// <summary>
    /// InputAxis is used for x and y axis input. The simplest solution is a Key axis input, but it can also be used as a joystick.
    /// </summary>
    public class InputAxis : ActionModel
    {
        #region Properties
        // Constructor properties
        public float DeadZoneRadius { get; private set; }
        public float MaxDistanceRadius { get; private set; }

        // Subscribable properties
        public ActionProperty<float> HorizontalDistance { get; private set; } // X value of AxisPosition
        public ActionProperty<float> VerticalDistance { get; private set; } // Y value of AxisPosition
        public ActionProperty<Vector2> AxisPosition { get; private set; } // The current position of the InputAxis
        public ActionProperty<float> AxisInputDistance { get; private set; } // The current AxisPosition distance from the center
        public ActionProperty<float> AxisInputAngle { get; private set; } // The current AxisPosition angle
        public ActionProperty<Direction> AxisInputDirection { get; private set; } // The current "D-pad" value of AxisPosition
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InputAxis"/> class.
        /// </summary>
        /// <param name="deadZoneRadius">The dead zone radius determines a small radius where value changes to AxisPosition should be neglected</param>
        /// <param name="maxDistanceRadius">The maximum distance radius that AxisPosition can have.</param>
		public InputAxis (float deadZoneRadius, float maxDistanceRadius) {
            DeadZoneRadius = deadZoneRadius;
            MaxDistanceRadius = maxDistanceRadius;
            HorizontalDistance = new ActionProperty<float>();
            VerticalDistance = new ActionProperty<float>();
            AxisPosition = new ActionProperty<Vector2>();
            AxisInputDistance = new ActionProperty<float>();
            AxisInputAngle = new ActionProperty<float>();
            AxisInputDirection = new ActionProperty<Direction>();
		    AxisPosition.OnValueChanged += (sender, axisPosition) =>                 
		    {
                SetAxisInputDistance(axisPosition.Value);
                SetAxisInputAngle(axisPosition.Value);
		        SetAxisInputDirection(AxisInputAngle.Value);
		    };
		}

        #region functions
        private void SetAxisInputDirection(float angle)
	    {
	        var inputDirection = Direction.None;
	        if (AxisInputDistance.Value < DeadZoneRadius
	            ||
                AxisPosition.Value == Vector2.zero)
	        {
	            inputDirection = Direction.None;
	        }

            else if (angle <= 22.5f || angle >= 337.5f)
	        {
	            inputDirection = Direction.Up;
	        }
            else if (angle >= 22.5f && angle <= 67.5f)
	        {
	            inputDirection = Direction.UpRight;
	        }
	        else if (angle >= 67.5f && angle <= 112.5f)
	        {
	            inputDirection = Direction.Right;
	        }
	        else if (angle >= 112.5f && angle <= 157.5f)
	        {
	            inputDirection = Direction.DownRight;
	        }
	        else if (angle >= 157.5f && angle <= 202.5f)
	        {
	            inputDirection = Direction.Down;
	        }
	        else if (angle >= 202.5f && angle <= 247.5f)
	        {
	            inputDirection = Direction.DownLeft;
	        }
	        else if (angle >= 247.5f && angle <= 292.5f)
	        {
	            inputDirection = Direction.Left;
	        }
	        else if (angle >= 292.5f && angle <= 337.5f)
	        {
	            inputDirection = Direction.UpLeft;
	        }
	        AxisInputDirection.Value = inputDirection;
	    }

	    private void SetAxisInputAngle(Vector2 axisPosition)
	    {
            if (axisPosition == Vector2.zero)
	        {
	            AxisInputAngle.Value = 0f;
	        }
            var angle = Mathf.Atan2(axisPosition.y, axisPosition.x) * Mathf.Rad2Deg;
	        angle -= 90f;
	        angle *= -1f;

	        if (angle < 0f)
	        {
	            angle += 360f;
	        }
	        AxisInputAngle.Value = angle;
	    }

	    private void SetAxisInputDistance(Vector2 axisPosition)
	    {
            if (axisPosition == Vector2.zero)
	        {
	            AxisInputDistance.Value = 0f;
	        }
            var distance = Mathf.Sqrt(axisPosition.y * axisPosition.y) +
                           (axisPosition.x * axisPosition.x);
	        if (distance > MaxDistanceRadius)
	        {
	            distance = MaxDistanceRadius;
	        }
            AxisInputDistance.Value = distance;
        }
        #endregion functions
    }
}
