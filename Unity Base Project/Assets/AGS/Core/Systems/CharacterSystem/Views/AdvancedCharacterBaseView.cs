using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.InteractionSystem.Base;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Like normal character view plus InteractionSkillView reference
    /// </summary>
    [Serializable]
    public abstract class AdvancedCharacterBaseView : CharacterBaseView
    {
		#region Public properties
        // Reference to be set in the editor
        public InteractionSkillsView InteractionSkillsView;
        
        public AdvancedCharacterBase AdvancedCharacter;
		#endregion

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            AdvancedCharacter = model as AdvancedCharacterBase;
            if (AdvancedCharacter == null) return;
            AdvancedCharacter.InteractionSkills.Value = InteractionSkillsView == null ? null : InteractionSkillsView.InteractionSkills;            
        }   
    }
}