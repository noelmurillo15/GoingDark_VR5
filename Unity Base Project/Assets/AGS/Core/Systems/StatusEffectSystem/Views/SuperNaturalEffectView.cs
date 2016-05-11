using System;
using AGS.Core.Enums;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// SuperNaturalEffectView
    /// </summary>
    [Serializable]
    public class SuperNaturalEffectView : StaticStatusEffectBaseView
    {
		
		#region Public properties
		public SuperNaturalEffectType EffectType;
        public SuperNaturalEffect SuperNaturalEffect;
		#endregion

        public override void InitializeView()
        {
            SuperNaturalEffect = new SuperNaturalEffect(Strength, Duration, IsInfinite, EffectType);
            SolveModelDependencies(SuperNaturalEffect);
        }

	}
}