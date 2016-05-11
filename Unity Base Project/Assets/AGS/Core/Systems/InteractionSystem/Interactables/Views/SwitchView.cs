using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.InteractionSystem.Interactables
{
    /// <summary>
    /// SwitchView
    /// </summary>
    [Serializable]
    public class SwitchView : ActionView
    {

        #region Public properties
        // Fields to be set in the editor
        public bool ReLockable;
        public float SecondsSwitching;
        #endregion

        public Switch Switch;

        #region AGS Setup
        public override void InitializeView()
        {
            Switch = new Switch(transform, ReLockable, SecondsSwitching);
            SolveModelDependencies(Switch);
        }
        #endregion
    }
}