using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Systems.CheckpointSystem
{
    /// <summary>
    /// Place CheckPointBaseView in the sceneview. Make sure Indexes are correct.
    /// </summary>
    [Serializable]
    public abstract class CheckPointBaseView : ActionView
    {

        #region Public properties
        public int Index;
        public bool LevelStart;
        public bool LevelEnd;
		#endregion
		public CheckPoint CheckPoint;

        #region AGS Setup
        public override void InitializeView()
        {
            CheckPoint = new CheckPoint(Index, LevelStart, LevelEnd, transform.position);       
            SolveModelDependencies(CheckPoint);
        }
        #endregion

        /// <summary>
        /// Player reached the checkpoint.
        /// </summary>
        /// <param name="playerBaseView">The player base view.</param>
        protected abstract void PlayerReachedCheckpoint(PlayerBaseView playerBaseView);
    }
}