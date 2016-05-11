using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// ExplosionEffectBaseView
    /// </summary>
    [Serializable]
    public abstract class ExplosionEffectBaseView : ActionView
    {

        #region Public properties
        public float Strength;
        public float Radius;
        public float UpwardsModifier;
        public ExplosionEffect ExplosionEffect;
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            ExplosionEffect = new ExplosionEffect(Strength, Radius, UpwardsModifier);
            SolveModelDependencies(ExplosionEffect);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            ExplosionEffect.ExplosionEffectTriggerAction += TriggerExplosionEffect;
        }
        #endregion

        /// <summary>
        /// Triggers the explosion effect.
        /// </summary>
        protected abstract void TriggerExplosionEffect();
    }
}