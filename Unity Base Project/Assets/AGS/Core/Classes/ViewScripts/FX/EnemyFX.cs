using System;
using AGS.Core.Base;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.AISystem;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio based on Enemy activity
    /// </summary>
    public class EnemyFX : ViewScriptBase
    {
        private AudioSource _audioSource;
        public AudioClip FootStepClip;
        private EnemyBaseView _enemyBaseView;
        private Enemy _enemy;
        private TimerComponent _movementSoundInterval;
        
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
                _enemyBaseView = ViewReference as EnemyBaseView;
                if (_enemyBaseView == null) return;

                _enemy = _enemyBaseView.Enemy;

            }
            if (_enemy == null) return;
            var aiController = _enemy.CharacterController.Value as AI;
            // If enemy has AI - subscribe to AI state, otherwise subcribe to movementskillstate
            if (aiController != null)
            {
                aiController.AICurrentState.OnValueChanged += (sender, aiState) =>
                {
                    {
                        if (_movementSoundInterval != null)
                        {
                            _movementSoundInterval.FinishTimer();
                        }
                        switch (aiState.Value)
                        {
                            case AIStateMachineState.Patrolling:
                                _movementSoundInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Movement Sound Interval");
                                _movementSoundInterval.TimerMethod = () => {
                                    if (_audioSource != null)
                                    {
                                        _audioSource.PlayOneShot(FootStepClip, 0.25f);    
                                    }
                                    
                                };
                                _movementSoundInterval.SetupIntervalInfinite(TimeSpan.FromMilliseconds(750));
                                break;
                            case AIStateMachineState.Chasing:
                                _movementSoundInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Movement Sound Interval");
                                _movementSoundInterval.TimerMethod = () =>
                                {
                                    if (_audioSource != null)
                                    {
                                        _audioSource.PlayOneShot(FootStepClip, 0.35f);    
                                    }
                                    
                                };
                                _movementSoundInterval.SetupIntervalInfinite(TimeSpan.FromMilliseconds(250));
                                break;
                            default:
                                if (_movementSoundInterval != null)
                                {
                                    _movementSoundInterval.FinishTimer();
                                }
                                break;
                        }
                    }
                };
            }
            else
            {
                _enemy.MovementSkills.Value.HorizontalMovement.Value.HorizontalMovementCurrentState.OnValueChanged += (sender, movementState) =>
                    {
                        if (movementState.Value == HorizontalMovementState.Moving)
                        {
                            _movementSoundInterval = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(ViewReference.gameObject ?? GameManager.TemporaryTimerComponents, "Movement Sound Interval");
                            _movementSoundInterval.TimerMethod = () => _audioSource.PlayOneShot(FootStepClip, 0.35f);
                            _movementSoundInterval.SetupIntervalInfinite(TimeSpan.FromMilliseconds(250));
                        }
                        else if (_movementSoundInterval != null)
                        {
                            _movementSoundInterval.FinishTimer();
                        }
                    };
            }

        }


    }
}
