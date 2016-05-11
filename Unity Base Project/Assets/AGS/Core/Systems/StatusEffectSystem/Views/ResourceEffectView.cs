using System;
using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// ResourceEffectView
    /// </summary>
    [Serializable]
    public class ResourceEffectView : PeriodicStatusEffectBaseView
    {
		
		#region Public properties
		public ResourceEffectType EffectType;
        public DamageableResourceType ResourceType;
        public bool UseProximityModifier;
        public bool UseHitFromBehindModifier;
        public float HitFromBehindModifier;
		#endregion

        public ResourceEffect ResourceEffect;

        #region AGS Setup
        public override void InitializeView()
        {
            ResourceEffect = new ResourceEffect(Strength, StrengthType, Ticks, SecondsBetweenTicks, EffectType, ResourceType, UseProximityModifier, transform, UseHitFromBehindModifier, HitFromBehindModifier);
            SolveModelDependencies(ResourceEffect);
        }
        #endregion
    }
}