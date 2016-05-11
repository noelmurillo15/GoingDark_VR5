using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// PeriodicStatusEffectBaseView
    /// </summary>
    [Serializable]
    public abstract class PeriodicStatusEffectBaseView : StatusEffectBaseView
    {
		
		#region Public properties
		public int Ticks;
        public float SecondsBetweenTicks;
        public PeriodicStatusEffectBase PeriodicStatusEffect;
		#endregion

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            PeriodicStatusEffect = model as PeriodicStatusEffectBase;
        }
        #endregion
    }
}