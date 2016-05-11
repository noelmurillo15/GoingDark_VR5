using System;
using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// PushEffectView
    /// </summary>
    [Serializable]
    public class PushEffectView : PeriodicStatusEffectBaseView
    {
		
		#region Public properties
		public ForceType ForceType;
        public VectorDirection Direction;
        public PushEffect PushEffect;
		#endregion

        #region AGS Setup
        public override void InitializeView()
        {
            PushEffect = new PushEffect(Strength, Ticks, SecondsBetweenTicks, ForceType, Direction);
            SolveModelDependencies(PushEffect);
        }
        #endregion
    }
}