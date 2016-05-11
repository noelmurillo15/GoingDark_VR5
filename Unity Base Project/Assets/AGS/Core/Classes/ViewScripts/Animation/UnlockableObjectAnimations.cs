using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to an UnlockableObject and sets animator parameters
    /// </summary>
    public class UnlockableObjectAnimations : ViewScriptBase
    {
        private Animator _animator;
        private UnlockableObjectView _unlockableObjectView;
        private UnlockableObject _unlockableObject;
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }
        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _unlockableObjectView = ViewReference as UnlockableObjectView;
                if (_unlockableObjectView == null) return;

                _unlockableObject = _unlockableObjectView.UnlockableObject;

            }
            if (_unlockableObject == null) return;
            _unlockableObject.Active.OnValueChanged += (sender, active) => _animator.SetBool("Locked", active.Value);
        }
    }
}
