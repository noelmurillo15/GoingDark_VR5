using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.GameLevelSystem
{
    /// <summary>
    /// Handles restriction from going past a point in a certain direction
    /// </summary>
    public class GameLevelBound : ActionModel
    {
        #region Properties
        // Constructor properties
        public GameLevelLimitSide LimitSide { get; private set; }
        public GameLevelBoundType Type { get; private set; }
        public Vector3 Position { get; private set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLevelBound"/> class.
        /// </summary>
        /// <param name="limitSide">The limit side.</param>
        /// <param name="type">The GameLevelBoundType.</param>
        /// <param name="position">The position.</param>
        public GameLevelBound(GameLevelLimitSide limitSide, GameLevelBoundType type, Vector3 position)
        {
            LimitSide = limitSide;
            Type = type;
            Position = position;
        }
    }
}
