using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.MovingEnvironmentSystem
{
    /// <summary>
    /// A MovingEnvironment uses a path to move between path points
    /// </summary>
    public class MovingEnvironment : ActionModel
    {
        // Constructor properties
        public int Speed { get; private set; }
        public float TargetPointRadius { get; private set; }
        public EnvironmentPathPoint CurrentTarget { get; set; }
        public MovingEnvironmentMovementType MovementType { get; private set; }

        // Subscribable properties
        public ActionProperty<EnvironmentPathDirection> Direction { get; set; }
        public ActionProperty<EnvironmentPath> EnvironmentPath { get; private set; }
        public ActionProperty<MovingEnvironmentState> MovingEnvironmentCurrentState { get; private set; }

        public Action<MovingEnvironment> NextTarget;
        public Func<MovingEnvironment, EnvironmentPathPoint> GetNextTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovingEnvironment"/> class.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="targetPointRadius">Determines how close to a path point this moving environment need to come before seeking next point on path.</param>
        /// <param name="movementType">Type of the movement.</param>
        /// <param name="direction">The direction.</param>
        public MovingEnvironment(int speed, float targetPointRadius, MovingEnvironmentMovementType movementType,
            EnvironmentPathDirection direction)
        {
            Speed = speed;
            TargetPointRadius = targetPointRadius;
            MovementType = movementType;
            Direction = new ActionProperty<EnvironmentPathDirection>() { Value = direction };
            EnvironmentPath = new ActionProperty<EnvironmentPath>();
            EnvironmentPath.OnValueChanged += (sender, environmentPath) =>
            {
                if (environmentPath.Value != null)
                {
                    OnEnvironmentPathChanged(environmentPath.Value);
                }
            };

            MovingEnvironmentCurrentState = new ActionProperty<MovingEnvironmentState>();

        }

        #region private functions
        /// <summary>
        /// Called when [environment path changed].
        /// </summary>
        /// <param name="environmentPath">The environment path.</param>
        private void OnEnvironmentPathChanged(EnvironmentPath environmentPath)
        {
            if (CurrentTarget != null) return;

            // check for first environment path point
            if (environmentPath.Points.Any())
            {
                SetStartingPoint(environmentPath.Points.First());
            }
            else
            {
                environmentPath.Points.ListItemAdded += SetStartingPoint;
            }

        }

        /// <summary>
        /// Sets the starting point.
        /// </summary>
        /// <param name="environmentPathPoint">The environment path point.</param>
        private void SetStartingPoint(EnvironmentPathPoint environmentPathPoint)
        {
            CurrentTarget = environmentPathPoint;
        }
        #endregion

        #region state transitions
        /// <summary>
        /// Transitions to state move.
        /// </summary>
        public void TransitionToStateMove()
        {
            MovingEnvironmentCurrentState.Value = MovingEnvironmentState.Moving;
        }

        /// <summary>
        /// Transitions to state stop.
        /// </summary>
        public void TransitionToStateStop()
        {
            MovingEnvironmentCurrentState.Value = MovingEnvironmentState.Idle;
        }
        #endregion

        #region public functions
        /// <summary>
        /// Sets CurrentTarget to next point on the path.
        /// </summary>
        public void SetNextTarget()
        {
            CurrentTarget = EnvironmentPath.Value.GetNextPoint(Direction, CurrentTarget);
        }
        #endregion
    }
}


