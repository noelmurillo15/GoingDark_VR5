using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// StatusEffectComboView can own any number of status effects
    /// Add status effect views to separate child GameObjects to this Transform
    /// </summary>
    [Serializable]
    public class StatusEffectComboView : ActionView
    {
        public StatusEffectCombo StatusEffectCombo; 

        public override void InitializeView()
        {
            StatusEffectCombo = new StatusEffectCombo();
            SolveModelDependencies(StatusEffectCombo);
        }

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            foreach (var resourceEffectView in transform.GetComponentsInChildren<ResourceEffectView>())
            {
                StatusEffectCombo.ResourceEffects.Add(resourceEffectView.ResourceEffect);
            }
            foreach (var superNaturalEffectView in transform.GetComponentsInChildren<SuperNaturalEffectView>())
            {
                StatusEffectCombo.SuperNaturalEffectsEffects.Add(superNaturalEffectView.SuperNaturalEffect);
            }
            foreach (var pushEffectView in transform.GetComponentsInChildren<PushEffectView>())
            {
                StatusEffectCombo.PushEffects.Add(pushEffectView.PushEffect);
            }
            foreach (var movementEffectView in transform.GetComponentsInChildren<MovementEffectView>())
            {
                StatusEffectCombo.MovementEffects.Add(movementEffectView.MovementEffect);
            }
        }
        #endregion
    }
}