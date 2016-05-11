using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.CombatSkillSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to CombatSkills and sets animator parameters
    /// </summary>
    public class ThrowingSkillAnimations : ViewScriptBase
    {
        private Animator _animator;

        private CharacterBaseView _characterBaseView;
        private CharacterBase _character;

        public override void Awake()
        {
            base.Awake();
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        protected override void SetupModelBindings()
        {
            if (_animator == null) return;
            if (ViewReference != null)
            {
                _characterBaseView = ViewReference as CharacterBaseView;
                if (_characterBaseView == null) return;

                _character = _characterBaseView.Character;

            }
            if (_character == null) return;
            _character.ThrowingSkill.OnValueChanged += (sender, throwingSkill) => OnThrowingSkillChanged(throwingSkill.Value);
        }

        /// <summary>
        /// Called when [throwing skill changed].
        /// </summary>
        /// <param name="throwingSkill">The throwing skill.</param>
        private void OnThrowingSkillChanged(ThrowingSkill throwingSkill)
        {
            if (throwingSkill == null) return;
            throwingSkill.BeginThrowAction += () => _animator.SetTrigger("ThrowTrowableWeapon");
        }
    }
}
