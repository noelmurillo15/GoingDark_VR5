using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles audio used by an UnlockableObject
    /// </summary>
    public class UnlockableObjectFX : ViewScriptBase
    {
        public AudioClip UnlockedClip;
        public AudioClip LockedClip;
        public AudioClip SwitchClip;
        protected AudioSource AudioSource;
        private UnlockableObjectView _unlockableObjectView;
        private UnlockableObject _unlockableObject;
        public override void Awake()
        {
            base.Awake();
            AudioSource = GetComponent<AudioSource>();
            AudioSource.Play();
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
            _unlockableObject.Active.OnValueChanged += (sender, active) =>
            {
                if (SwitchClip != null)
                {
                    AudioSource.PlayOneShot(SwitchClip);
                }
                AudioSource.clip = active.Value ? UnlockedClip : LockedClip;
                AudioSource.Play();
            };
        }


    }
}
