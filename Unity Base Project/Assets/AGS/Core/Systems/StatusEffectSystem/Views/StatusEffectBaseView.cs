using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// Base view for any status effect
    /// </summary>
    [Serializable]
    public abstract class StatusEffectBaseView : ActionView
    {
        #region Public properties
        public StatusEffectStrengthType StrengthType;
        public float Strength;
        #endregion

        public StatusEffectBase StatusEffect;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            StatusEffect = model as StatusEffectBase;
        }
        #endregion
    }

}
