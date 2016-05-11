using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.GameLevelSystem
{
    /// <summary>
    /// Place on appropriate GameObject to prevent character from moving past
    /// </summary>
    [Serializable]
    public class GameLevelBoundView : ActionView
    {
		#region Public properties
        public GameLevelLimitSide LimitSide;
        public GameLevelBoundType Type;
		#endregion

		public GameLevelBound GameLevelBound;

        #region AGS Setup

        public override void InitializeView()
        {
            GameLevelBound = new GameLevelBound(LimitSide, Type, transform.position);
            SolveModelDependencies(GameLevelBound);
        }

        #endregion
    }
}