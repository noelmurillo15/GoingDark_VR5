using System;
using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// MovementEffectView
    /// </summary>
    [Serializable]
    public class MovementEffectView : StaticStatusEffectBaseView
    {
		
		#region Public properties
        public MovementEffectType EffectType;
        public MovementEffect MovementEffect;
		#endregion

        #region AGS Setup
        public override void InitializeView()
        {
            MovementEffect = new MovementEffect(Strength, StrengthType, Duration, IsInfinite, EffectType);
            SolveModelDependencies(MovementEffect);
        }
        #endregion
    }

}