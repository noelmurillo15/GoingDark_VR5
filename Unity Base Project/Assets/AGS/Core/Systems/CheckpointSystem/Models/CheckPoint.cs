using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CheckpointSystem
{
    /// <summary>
    /// Checkpoint models is to be used by the GameLevel.
    /// </summary>
    public class CheckPoint : ActionModel
    {
        #region Properties
        // Constructor properties
        public int Index { get; private set; }
        public bool LevelStart { get; private set; }
        public bool LevelEnd { get; private set; }
        public Vector3 Position { get; private set; }

        public Action CheckPointReachedAction { get; set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckPoint"/> class.
        /// </summary>
        /// <param name="index">The checkpoint index.</param>
        /// <param name="levelStart">if set to <c>true</c> [level start].</param>
        /// <param name="levelEnd">if set to <c>true</c> [level end].</param>
        /// <param name="position">The checkpoint position.</param>
        public CheckPoint(int index, bool levelStart, bool levelEnd, Vector3 position)
        {
            Index = index;
            LevelStart = levelStart;
            LevelEnd = levelEnd;
            Position = position;
        }

        #region public functions

        /// <summary>
        /// Call this when checkpoint is reached. Notifies subscribers any calling CheckPointReachedAction
        /// </summary>
        public void CheckPointReached()
        {
            if (CheckPointReachedAction != null)
            {
                CheckPointReachedAction();    
            }
            
        }
        #endregion
    }
}
