using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// AnimEventListeners should be used to recieve events raised by mecanim animations.
    /// Used as a bridge to connect animations with GameView
    /// </summary>
    public abstract class CombatSkillAnimEventListenerBase : ViewScriptBase
    {
        protected CombatEntityBaseView CombatEntityBaseView;
        protected CombatEntityBase CombatEntityBase;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                CombatEntityBaseView = ViewReference as CombatEntityBaseView;
                if (CombatEntityBaseView == null) return;

                CombatEntityBase = CombatEntityBaseView.CombatEntity;

            }
            if (CombatEntityBase == null) return;
        }
    }
}
