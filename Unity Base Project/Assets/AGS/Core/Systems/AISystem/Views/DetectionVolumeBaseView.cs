using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.AISystem
{
    /// <summary>
    /// DetectionVolumeBaseView this the base class for player detection implementations
    /// </summary>
    [Serializable]
    public abstract class DetectionVolumeBaseView : ActionView
    {

        public DetectionVolumeBase DetectionVolumeBase;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            DetectionVolumeBase = model as DetectionVolumeBase;
        }
        #endregion
    }
}