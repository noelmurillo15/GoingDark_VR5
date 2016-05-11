using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.MovingEnvironmentSystem
{
    /// <summary>
    /// EnvironmentPaths is built with a number of path points and is used by AI's and MovingEnvironments
    /// </summary>
    public class EnvironmentPath : ActionModel
    {
        // Constructor propterties
        public EnvironmentPathType PathType { get; private set; }

        // Subscribable properties
        public ActionList<EnvironmentPathPoint> Points { get; private set; } // List of points that marks this path

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentPath"/> class.
        /// </summary>
        /// <param name="pathType">Type of the path.</param>
        public EnvironmentPath(EnvironmentPathType pathType)
        {
            PathType = pathType;
            Points = new ActionList<EnvironmentPathPoint>();
        }

        /// <summary>
        /// Gets the next point on the path based on path type and direction.
        /// </summary>
        /// <param name="environmentPathDirection">The environment path direction.</param>
        /// <param name="currentTarget">The current target.</param>
        /// <returns></returns>
        public EnvironmentPathPoint GetNextPoint(ActionProperty<EnvironmentPathDirection> environmentPathDirection, EnvironmentPathPoint currentTarget)
        {
            switch (PathType)
            {
                case EnvironmentPathType.PingPong:
                    return GetNextPingPongPoint(environmentPathDirection, currentTarget);
                case EnvironmentPathType.Circular:
                    return GetNextCirclePoint(environmentPathDirection.Value, currentTarget);
                default:
                    return GetNextPingPongPoint(environmentPathDirection, currentTarget);
            }
        }

        /// <summary>
        /// Gets the next ping pong point based on direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="currentTarget">The current target.</param>
        /// <returns></returns>
        private EnvironmentPathPoint GetNextPingPongPoint(ActionProperty<EnvironmentPathDirection> direction, EnvironmentPathPoint currentTarget)
        {
            var currentIndex = Points.IndexOf(currentTarget);
            if (currentIndex == Points.Count - 1)
            {
                direction.Value = EnvironmentPathDirection.Backward;
            }
            else if (currentIndex == 0)
            {
                direction.Value = EnvironmentPathDirection.Forward;
            }
            if (direction.Value == EnvironmentPathDirection.Forward)
            {
                currentIndex++;
            }
            else if (direction.Value == EnvironmentPathDirection.Backward)
            {
                currentIndex--;
            }
            return Points[currentIndex];
        }

        /// <summary>
        /// Gets the next circle point based on direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="currentTarget">The current target.</param>
        /// <returns></returns>
        private EnvironmentPathPoint GetNextCirclePoint(EnvironmentPathDirection direction, EnvironmentPathPoint currentTarget)
        {
            var currentIndex = Points.IndexOf(currentTarget);

            if (direction == EnvironmentPathDirection.Forward)
            {
                currentIndex++;
            }
            else if (direction == EnvironmentPathDirection.Backward)
            {
                currentIndex--;
            }
            if (direction == EnvironmentPathDirection.Forward && currentIndex == Points.Count)
            {
                currentIndex = 0;
            }
            else if (direction == EnvironmentPathDirection.Backward && currentIndex < 0)
            {
                currentIndex = Points.Count - 1;
            }
            return Points[currentIndex];
        }
    }

}
