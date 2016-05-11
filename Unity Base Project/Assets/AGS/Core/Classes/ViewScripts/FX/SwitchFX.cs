using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles audio used by a Switch
    /// </summary>
    public class SwitchFX : ViewScriptBase
    {
        public AudioClip SwitchedOnClip;
        public AudioClip SwitchedOffClip;
        public AudioClip SwitchClip;
        private AudioSource _audioSource;
        private SwitchView _switchView;
        private Switch _switch;
        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Play();
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
            _switch.On.OnValueChanged += (sender, on) =>
            {
                if (SwitchClip != null)
                {
                    _audioSource.PlayOneShot(SwitchClip);
                }
                _audioSource.clip = on.Value ? SwitchedOnClip : SwitchedOffClip;
                _audioSource.Play();
            };
        }


    }
}
