using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.MovingEnvironmentSystem
{
    /// <summary>
    /// An EnvironmentPathPointView need to be part of an EnvironmentPath.
    /// </summary>
    [Serializable]
    public class EnvironmentPathPointView : ActionView
    {
        public EnvironmentPathPoint EnvironmentPathPoint;

        public override void InitializeView()
        {
            EnvironmentPathPoint = new EnvironmentPathPoint(transform.position);
            SolveModelDependencies(EnvironmentPathPoint);
        }
    }
}
