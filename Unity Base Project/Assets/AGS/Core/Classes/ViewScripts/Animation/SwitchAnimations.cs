using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to a Switch and sets animator parameters
    /// </summary>
    public class SwitchAnimations : ViewScriptBase
    {
        private Animator _animator;
        private SwitchView _switchView;
        private Switch _switch;
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _switchView = ViewReference as SwitchView;
                if (_switchView == null) return;

                _switch = _switchView.Switch;

            }
            if (_switch == null) return;
            _switch.On.OnValueChanged += (sender, on) => _animator.SetBool("Switch", on.Value);
        }
    }
}
