using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// StaticStatusEffectBaseView
    /// </summary>
    [Serializable]
    public abstract class StaticStatusEffectBaseView : StatusEffectBaseView
    {
		
		#region Public properties
		public float Duration;
        public bool IsInfinite;
        public StaticStatusEffectBase StaticStatusEffect;

        #endregion

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            StaticStatusEffect = model as StaticStatusEffectBase;
        }
        #endregion
    }
}