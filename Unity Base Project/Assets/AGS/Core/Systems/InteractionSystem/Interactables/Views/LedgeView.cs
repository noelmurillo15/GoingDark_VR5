using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// LedgeView
    /// </summary>
    [Serializable]
    public class LedgeView : ActionView
    {

        public Ledge Ledge;

        #region AGS Setup
        public override void InitializeView()
        {
            Ledge = new Ledge(transform);
            SolveModelDependencies(Ledge);
        }
        #endregion
    }
}