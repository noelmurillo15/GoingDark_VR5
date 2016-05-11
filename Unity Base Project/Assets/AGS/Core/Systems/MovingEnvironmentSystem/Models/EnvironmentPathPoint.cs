using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.MovingEnvironmentSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class EnvironmentPathPoint : ActionModel
    {
        // Constructor properties     
        public Vector3 Position { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentPathPoint"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public EnvironmentPathPoint(Vector3 position)
        {
            Position = position;
        }
    }
    }
