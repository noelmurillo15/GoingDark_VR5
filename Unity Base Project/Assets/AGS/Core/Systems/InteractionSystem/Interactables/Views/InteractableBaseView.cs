using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// Base view for any interactables
    /// </summary>
    [Serializable]
    public class InteractableBaseView : ActionView
    {
        public InteractableBase InteractableBase;

        #region AGS Setup
        public override void InitializeView()
        {
            InteractableBase = new InteractableBase(transform);
            SolveModelDependencies(InteractableBase);
        }
        #endregion
    }
}