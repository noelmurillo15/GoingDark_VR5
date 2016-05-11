using System;
using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// ContinuousResourceEffectView
    /// </summary>
    [Serializable]
    public class ContinuousResourceEffectView : StatusEffectBaseView
    {

        #region Public properties
        public ResourceEffectType EffectType;
        public DamageableResourceType ResourceType;
        public bool IsActive;
        public float SecondsInterval;
        public ContinuousResourceEffect ContinuousResourceEffect;
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            ContinuousResourceEffect = new ContinuousResourceEffect(Strength, StrengthType, IsActive, SecondsInterval, EffectType, ResourceType);
            SolveModelDependencies(ContinuousResourceEffect);
        }
        #endregion
    }
}