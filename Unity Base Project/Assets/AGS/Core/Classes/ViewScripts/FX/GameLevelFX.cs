using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles audio effects based on GameLevel actions
    /// </summary>
    public class GameLevelFX : GameLevelViewScriptBase
    {
        #region propterties
        public AudioClip PickupClip;
        public AudioClip[] PlayerDeathClips;
        public AudioClip RestartClip;

        private AudioSource _audioSource;
        private System.Random _rnd;
        #endregion
        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _rnd = new System.Random();
        }

        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();

            GameLevel.PlayerDeadAction += () => _audioSource.PlayOneShot(PlayerDeathClips[_rnd.Next(PlayerDeathClips.Length)], 0.5f);
            GameLevel.LevelRestartAction += () => _audioSource.PlayOneShot(RestartClip, 0.5f);
            GameLevel.ItemPickUpAction += () => _audioSource.PlayOneShot(PickupClip);
        }
    }
}
