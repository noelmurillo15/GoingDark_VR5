using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.StatusEffectSystem;

namespace AGS.Core.Systems.PickUpSystem
{
    /// <summary>
    /// PickUpStatusEffectView should own a status effect combo view
    /// </summary>
    [Serializable]
    public class PickUpStatusEffectView : PickUpItemBaseView
    {
		
		#region Public properties
        public StatusEffectComboView StatusEffectComboView;
		#endregion

        public PickUpStatusEffect PickUpStatusEffect;

        #region AGS Setup
        public override void InitializeView()
        {
            PickUpStatusEffect = new PickUpStatusEffect(); 
            SolveModelDependencies(PickUpStatusEffect);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (StatusEffectComboView != null)
            {
                PickUpStatusEffect.EffectsCombo.Value = StatusEffectComboView.StatusEffectCombo;
            }
        }
        #endregion
        #region protected functions
        /// <summary>
        /// Called when [trigger enter notification] with PlayerBaseView.
        /// </summary>
        /// <param name="playerView">The player view.</param>
        protected override void OnTriggerEnterNotification(PlayerBaseView playerView)
        {
            if (PickUpStatusEffect.EffectsCombo.Value != null)
            {
                PickUpStatusEffect.ApplyEffects(playerView.Player);
            }
            base.OnTriggerEnterNotification(playerView);
        }
        #endregion
	}
}