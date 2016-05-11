using System;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// Create a skill sequence by adding skill names in the editor. Names must match CombatMove's names
    /// </summary>
    [Serializable]
    public class CombatMoveComboView : ActionView
    {
        #region Public properties
        public string[] SkillSequence;
        #endregion

        public CombatMoveCombo CombatMoveCombo;

        #region AGS Setup
        public override void InitializeView()
        {
            CombatMoveCombo = new CombatMoveCombo(SkillSequence);
            SolveModelDependencies(CombatMoveCombo);
        }
        #endregion
 
    }
}