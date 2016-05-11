using AGS.Core.Systems.CombatSkillSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// AnimEventListeners should be used to recieve events raised by mecanim animations.
    /// Used as a bridge to connect animations with GameView
    /// </summary>
    public class ThrowingSkillAnimEventListener : ViewScriptBase
    {
        private ThrowingSkillView _throwingSkillView;
        private ThrowingSkill _throwingSkill;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _throwingSkillView = ViewReference as ThrowingSkillView;
                if (_throwingSkillView == null) return;

                _throwingSkill = _throwingSkillView.ThrowingSkill;

            }
            if (_throwingSkill == null) return;
        }

        /// <summary>
        /// Call this functions from animation event.
        /// </summary>
        public void ThrowArc()
        {
            _throwingSkill.BeginThrowArc();
        }

        /// <summary>
        /// Call this functions from animation event.
        /// </summary>
        public void ThrowForward()
        {
            _throwingSkill.BeginThrowForward();
        }
    }
}
