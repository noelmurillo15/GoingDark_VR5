using System;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.MovementSystem.Base;
using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio based on Player activity
    /// </summary>
    public class PlayerFX : ViewScriptBase
    {
        #region propterties
        public AudioClip[] FootStepClips;
        public AudioClip[] TakingDamageClips;
        public AudioClip[] TauntingClips;
        public AudioClip HealedClip;
        public AudioClip JumpingClip;
        public AudioClip LandingClip;
        public UnityEngine.Object JumpingParticlesPrefab;
        public UnityEngine.Object HurtParticlesPrefab; // Particle system for when players is hurt

        private GameObject _hurtParticles;
        private AudioSource _audioSource;
        private System.Random _rnd;
        private bool _showHurtParticles;
        private PlayerBaseView _playerBaseView;
        private Player _player;
        private TimerComponent _movementSoundInterval;
        private TimerTemporaryGameObject _tauntingTimer;
        private KillableBase _target;
        #endregion

        #region MonoBehaviour
        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _rnd = new System.Random();
        }
        
        void OnDestroy()
        {
            if (_target != null)
            {
                // If this is destroyed, unsubribe any subscription to target state
                _target.DamageableCurrentState.OnValueChanged -= OnDamageableCurrentStateOnValueChanged;
            }
        }
        #endregion

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _playerBaseView = ViewReference as PlayerBaseView;
                if (_playerBaseView == null) return;

                _player = _playerBaseView.Player;

            }
            if (_player == null) return;
            _showHurtParticles = false;

            if (_player.MovementSkills.Value != null)
            {
                OnMovementSkillChanged(_player.MovementSkills.Value);
            }

            _player.ResourceEffectAppliedAction += ResourceEffectApplied;

            _player.Target.OnValueChanged += (sender, target) => OnTargetChanged(target.Value);

            if (_player.Resources.Any(x => x.ResourceType.Value == DamageableResourceType.Health))
            {
                OnPlayerHealthChanged(_player.Resources.FirstOrDefault(x => x.ResourceType.Value == DamageableResourceType.Health));
            }
        }

        /// <summary>
        /// Called when [movement skill changed].
        /// </summary>
        /// <param name="movementSkills">The movement skills.</param>
        private void OnMovementSkillChanged(MovementSkills movementSkills)
        {
            // Subscribe to MovementSkills separately
            if (movementSkills.HorizontalMovement.Value != null)
            {
                movementSkills.HorizontalMovement.Value.HorizontalMovementCurrentState.OnValueChanged += (sender, state) => OnHorizontalMovementStateChanged(state.Value);
            }
            if (movementSkills.VerticalMovement.Value != null)
            {
                movementSkills.VerticalMovement.Value.VerticalMovementCurrentState.OnValueChanged += (sender, state) => OnVerticalMovementStateChanged(state.Value);
            }
            if (movementSkills.Swimming != null)
            {
                movementSkills.Swimming.Value.SwimmingCurrentState.OnValueChanged += (sender, state) => OnSwimmingStateChanged(state.Value);
            }
        }

        /// <summary>
        /// Called when [horizontal movement state changed].
        /// Sets up different time intervals to play footstep sound dependeng state, i.e speed
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnHorizontalMovementStateChanged(HorizontalMovementState state)
        {
            if (_movementSoundInterval != null)
            {
                _movementSoundInterval.FinishTimer();
            }
            switch (state)
            {
                case HorizontalMovementState.Moving:
                    _movementSoundInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Movement Sound Interval");
                    _movementSoundInterval.TimerMethod = () => PlayFootStepSound(0.5f);
                    _movementSoundInterval.SetupIntervalInfinite(TimeSpan.FromMilliseconds(500));
                    break;
                case HorizontalMovementState.Sprinting:
                    _movementSoundInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Movement Sound Interval");
                    _movementSoundInterval.TimerMethod = () =>
                    {
                        if (Mathf.Abs(_player.CharacterController.Value.MoveVector.Value.x) > 0.1f)
                        {
                            PlayFootStepSound(0.8f);
                        }
                    };
                    _movementSoundInterval.SetupIntervalInfinite(TimeSpan.FromMilliseconds(250));

                    break;
                case HorizontalMovementState.Crouching:
                    _movementSoundInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Movement Sound Interval");
                    _movementSoundInterval.TimerMethod = () =>
                    {
                        if (Mathf.Abs(_player.CharacterController.Value.MoveVector.Value.x) > 0.1f)
                        {
                            PlayFootStepSound(0.2f);
                        }
                    };
                    _movementSoundInterval.SetupIntervalInfinite(TimeSpan.FromMilliseconds(750));
                    break;
            }
        }

        /// <summary>
        /// Plays random foot step sound.
        /// </summary>
        /// <param name="volumeScale">The volume scale.</param>
        private void PlayFootStepSound(float volumeScale)
        {
            if (_player.IsGrounded.Value && _audioSource != null)
            {
                _audioSource.PlayOneShot(FootStepClips[_rnd.Next(FootStepClips.Length)], volumeScale);
            }
        }

        /// <summary>
        /// Called when [vertical movement state changed].
        /// Used to turn off hurtparticles while in water.
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnVerticalMovementStateChanged(VerticalMovementState state)
        {
            if (state == VerticalMovementState.Landing)
            {
                _audioSource.PlayOneShot(LandingClip, 0.5f);
                CreateJumpingParticleSystem(Quaternion.Euler(-90f, 0f, 0f));
            }
            if (state == VerticalMovementState.Jumping)
            {
                _audioSource.PlayOneShot(JumpingClip, 0.5f);
                CreateJumpingParticleSystem(Quaternion.Euler(-90f, 0f, 0f));
            }
            if (state == VerticalMovementState.WallJumping)
            {
                _audioSource.PlayOneShot(JumpingClip, 0.5f);
                CreateJumpingParticleSystem(_player.FacingGameLevelForward.Value ? Quaternion.Euler(180f, 0f, 0f) : Quaternion.Euler(0f, 0f, 0f));
            }

        }

        /// <summary>
        /// Creates the jumping particle system and sets up destroy timer.
        /// </summary>
        /// <param name="euler">The euler.</param>
        private void CreateJumpingParticleSystem(Quaternion euler)
        {
            var skillHitParticleSystem = Instantiate(JumpingParticlesPrefab, _player.Transform.position, euler) as GameObject;
            if (skillHitParticleSystem != null)
            {
                skillHitParticleSystem.transform.parent = GameManager.EffectsContainer.transform;
            }

            var particleDestroyTimer = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Particle destroy timer");
            particleDestroyTimer.TimerMethod = () => DestroyObject(skillHitParticleSystem);
            particleDestroyTimer.Invoke(0.5f);
        }

        /// <summary>
        /// Called when [swimming state changed].
        /// Used to turn off hurtparticles while in water.
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnSwimmingStateChanged(SwimmingState state)
        {
            if (state == SwimmingState.InWater
                ||
                state == SwimmingState.DoingStroke)
            {
                Destroy(_hurtParticles);
            }
            else if (_showHurtParticles && _hurtParticles == null)
            {
                _hurtParticles = Instantiate(HurtParticlesPrefab,
                    new Vector3(_player.Transform.position.x,
                        _player.Transform.position.y + _player.Transform.GetComponent<CapsuleCollider>().height * 2f / 3f,
                        _player.Transform.position.z),
                    Quaternion.Euler(_player.Transform.eulerAngles.x, _player.Transform.eulerAngles.y + 180f,
                        _player.Transform.eulerAngles.z)) as GameObject;
                if (_hurtParticles == null) return;
                _hurtParticles.transform.parent = _player.Transform;
            }
        }

        /// <summary>
        /// Subscription function to resource effects. Plays different clips depending on heal or damage.
        /// </summary>
        /// <param name="resourceEffect">The resource effect.</param>
        private void ResourceEffectApplied(ResourceEffect resourceEffect)
        {
            if (_audioSource == null) return;
            if (resourceEffect.DamageableType != DamageableResourceType.Health) return;
            switch (resourceEffect.EffectType)
            {
                case ResourceEffectType.Heal:
                    _audioSource.PlayOneShot(HealedClip, 0.5f);
                    break;
                case ResourceEffectType.Damage:
                    _audioSource.PlayOneShot(TakingDamageClips[_rnd.Next(TakingDamageClips.Length)], 1f);
                    break;
            }
        }

        /// <summary>
        /// Called when [target changed].
        /// Plays random taunting clip when target is killed.
        /// </summary>
        /// <param name="target">The target.</param>
        private void OnTargetChanged(KillableBase target)
        {
            if (target == null) return;
            if (_tauntingTimer != null)
            {
                _tauntingTimer.FinishTimer();
            }
            if (_target != null)
            {
                // Unsubscribe to previous target so we dont get multiple delegate assignments
                _target.DamageableCurrentState.OnValueChanged -= OnDamageableCurrentStateOnValueChanged;    
            }
            
            _target = target;
            _target.DamageableCurrentState.OnValueChanged += OnDamageableCurrentStateOnValueChanged;
        }


        /// <summary>
        /// Called when [current target state changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{T}"/> instance containing the event data.</param>
        private void OnDamageableCurrentStateOnValueChanged(object sender, ActionPropertyEventArgs<DamageableState> state)
        {
            TauntDestroyedTargets(state);
        }

        /// <summary>
        /// Taunts if target is destroyed.
        /// </summary>
        /// <param name="state">The <see cref="ActionPropertyEventArgs{DamageableState}"/> instance containing the event data.</param>
        private void TauntDestroyedTargets(ActionPropertyEventArgs<DamageableState> state)
        {
            if (state.Value != DamageableState.Destroyed)
            {
                return;
            }
            _tauntingTimer = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Taunting timer");
            _tauntingTimer.TimerMethod = () =>
            {
                if (gameObject == null || gameObject.transform == null)
                {
                    return; // player may have died.
                }
                if (_audioSource == null || _audioSource.isPlaying)
                {
                    return;
                }
                if (UnityEngine.Random.value > 0.5f)
                {
                    return; // reduce taunting frequency
                }
                _audioSource.PlayOneShot(TauntingClips[_rnd.Next(TauntingClips.Length)], 1f);
            };
            _tauntingTimer.Invoke(0.75f);
        }

        /// <summary>
        /// Called when [player health changed].
        /// Creates hurt particles when low on life.
        /// </summary>
        /// <param name="health">The health.</param>
        private void OnPlayerHealthChanged(DamageableResource health)
        {
            if (HurtParticlesPrefab == null) return;
            health.Current.OnValueChanged += (sender, current) =>
                {
                    if (current.Value >= health.Max.Value / 2)
                    {
                        if (!_showHurtParticles) return;
                        Destroy(_hurtParticles);
                        _showHurtParticles = false;
                    }
                    else
                    {
                        if (_showHurtParticles) return;
                        _hurtParticles = Instantiate(HurtParticlesPrefab,
                             new Vector3(_player.Transform.position.x, _player.Transform.position.y + _player.Transform.GetComponent<CapsuleCollider>().height * 2f / 3f, _player.Transform.position.z),
                             Quaternion.Euler(_player.Transform.eulerAngles.x, _player.Transform.eulerAngles.y + 180f, _player.Transform.eulerAngles.z)) as GameObject;
                        if (_hurtParticles == null) return;
                        _hurtParticles.transform.parent = _player.Transform;
                        _showHurtParticles = true;
                    }
                };


        }
    }
}
