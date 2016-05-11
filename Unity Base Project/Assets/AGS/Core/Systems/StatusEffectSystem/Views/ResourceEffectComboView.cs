using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// ResourceEffectComboView can contain any number of resource effects
    /// </summary>
    [Serializable]
    public class ResourceEffectComboView : ActionView
    {
		
		#region Public properties
		public Transform ResourceEffectsContainer; // Add resource effect views on separate child GameObjects to this Transform
        public Transform ContinuousResourceEffectsContainer; // Add continuous resource effect views on separate child GameObjects to this Transform
		#endregion

		public ResourceEffectCombo ResourceEffectCombo;
        
        #region AGS Setup
        public override void InitializeView()
        {
			ResourceEffectCombo = new ResourceEffectCombo();
            SolveModelDependencies(ResourceEffectCombo);
		}

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (ResourceEffectsContainer != null)
            {
                foreach (var resourceEffectBaseView in ResourceEffectsContainer.GetComponentsInChildren<ResourceEffectView>())
                {
                    ResourceEffectCombo.ResourceEffects.Add((ResourceEffect)resourceEffectBaseView.StatusEffect);
                }
            }

            if (ContinuousResourceEffectsContainer != null)
            {
                foreach (var continuousResourceEffectView in ContinuousResourceEffectsContainer.GetComponentsInChildren<ContinuousResourceEffectView>())
                {
                    ResourceEffectCombo.ContinuousResourceEffects.Add((ContinuousResourceEffect)continuousResourceEffectView.StatusEffect);
                }
            }
        }
        #endregion
    }
}