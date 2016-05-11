using AGS.Core.Base;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Systems.CombatSkillSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio based on CombatMoves
    /// </summary>
    public class CombatMoveSetFX : ViewScriptBase
    {
        private CombatMoveSetView _combatMoveSetView;
        private CombatMoveSet _combatMoveSet;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _combatMoveSetView = ViewReference as CombatMoveSetView;
                if (_combatMoveSetView == null) return;

                _combatMoveSet = _combatMoveSetView.CombatMoveSet;

            }
            if (_combatMoveSet == null) return;

        }
  
    }
}
