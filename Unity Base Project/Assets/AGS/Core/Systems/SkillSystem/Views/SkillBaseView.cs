using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.StatusEffectSystem;

namespace AGS.Core.Systems.SkillSystem
{
    /// <summary>
    /// Base view for any skill. Override StateEnterSkillEnabled and/or StateEnterSkillDisabled notifications for specific implementations
    /// </summary>
    [Serializable]
    public abstract class SkillBaseView : ActionView
    {
        #region Public properties
        // Reference to be set in the editor
        public ResourceEffectComboView ResourceEffectCostCombo;
        #endregion

        public SkillBase Skill;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            Skill = model as SkillBase;
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            
            if (Skill == null) return;

            Skill.IsEnabled.OnValueChanged += (sender, isEnabled) => OnCurrentSkillStateChanged(isEnabled.Value);
        }
        #endregion

        #region state machine functions
        /// <summary>
        /// Called when [current skill state changed].
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnCurrentSkillStateChanged(bool isEnabled)
        {
            switch (isEnabled)
            {
                case true:
                    StateEnterSkillEnabled();
                    break;
                case false:
                    StateEnterSkillDisabled();
                    break;
            }
        }

        /// <summary>
        /// Called when skill is enabled.
        /// </summary>
        protected virtual void StateEnterSkillEnabled() { }

        /// <summary>
        /// Called when skill is disabled.
        /// </summary>
        protected virtual void StateEnterSkillDisabled() { }
        #endregion
    }
}