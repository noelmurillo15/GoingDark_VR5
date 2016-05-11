using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// LadderView
    /// </summary>
    [Serializable]
    public class LadderView : ActionView
    {

        public Ladder Ladder;

        #region AGS Setup
        public override void InitializeView()
        {
            Ladder = new Ladder(transform);
            SolveModelDependencies(Ladder);
        }
        #endregion
    }
}