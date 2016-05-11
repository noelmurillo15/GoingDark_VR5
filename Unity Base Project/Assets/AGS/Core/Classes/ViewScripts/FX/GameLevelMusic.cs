using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that playes different music depending on GameLevel actions
    /// </summary>
    public class GameLevelMusic : GameLevelViewScriptBase
    {
        #region propterties
        public AudioClip CalmBackGroundClip;
        public AudioClip PanicBackGroundClip;
        public AudioClip LevelFinishClip;
        public AudioClip GameOverClip;

        private AudioSource _audioSource;
        #endregion
        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();

            GameLevel.PlayerIsDetectedAction += SetBackGroundMusic;
            GameLevel.LevelFailedAction += PlayGameOverMusic;
            GameLevel.LevelFinishedAction += PlayLevelFinishedMusic;
            if (GameLevel.PlayerIsDetectedAction != null)
            {
                GameLevel.PlayerIsDetectedAction(GameLevel.CheckPlayerDetected());
            }       
        }

        /// <summary>
        /// Sets the back ground music.
        /// </summary>
        /// <param name="detected">if set to <c>true</c> [detected].</param>
        private void SetBackGroundMusic(bool detected)
        {
            if (detected && _audioSource.clip != PanicBackGroundClip)
            {
                _audioSource.clip = PanicBackGroundClip;
                _audioSource.Play();
            }
            if (!detected && _audioSource.clip != CalmBackGroundClip)
            {
                _audioSource.clip = CalmBackGroundClip;
                _audioSource.Play();
            }
        }

        /// <summary>
        /// Plays the game over music.
        /// </summary>
        private void PlayGameOverMusic()
        {
            _audioSource.clip = GameOverClip;
            _audioSource.Play();
        }

        /// <summary>
        /// Plays the level finished music.
        /// </summary>
        private void PlayLevelFinishedMusic()
        {
            _audioSource.clip = LevelFinishClip;
            _audioSource.Play();
        }
    }
}
