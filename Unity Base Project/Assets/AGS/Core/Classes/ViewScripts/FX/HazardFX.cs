using System;
using AGS.Core.Base;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.HazardSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio based on HazardStates and collisions
    /// </summary>
    public class HazardFX : ViewScriptBase
    {
        public Object HitParticlesPrefab;
        public Transform ActiveStateParticleSystemsContainer;
        public Transform RechargingStateParticleSystemsContainer;
        public Transform InactiveStateParticleSystemsContainer;
        public AudioClip HitEffectClip;
        public AudioClip ActiveStateClip;
        public AudioClip RechargingStateClip;
        public AudioClip InactiveStateClip;
        private HazardBaseView _hazardBaseView;
        private AreaHazardView _areaHazardView;
        private HazardBase _hazardBase;
        private ParticleSystem[] _activeStateParticleSystems;
        private ParticleSystem[] _rechargingStateParticleSystems;
        private ParticleSystem[] _inactiveStateParticleSystems;
        private AudioSource _audioSource;

        public Action ParticlesSystemsDoneAction { get; set; }

        public override void Start()
        {
            base.Start();
            if (ActiveStateParticleSystemsContainer != null)
            {
                _activeStateParticleSystems = ActiveStateParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>();
            }
            if (RechargingStateParticleSystemsContainer != null)
            {
                _rechargingStateParticleSystems = RechargingStateParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>();
            }
            if (InactiveStateParticleSystemsContainer != null)
            {
                _inactiveStateParticleSystems = InactiveStateParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>();
            }
            _audioSource = GetComponent<AudioSource>();
        }
        
        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _hazardBaseView = ViewReference as HazardBaseView;
                if (_hazardBaseView != null)
                {
                    _hazardBase = _hazardBaseView.HazardBase;
                    
                }
                else
                {
                    _areaHazardView = ViewReference as AreaHazardView;
                    if (_areaHazardView != null)
                    {
                        _hazardBase = _areaHazardView.AreaHazard;
                    }
                }
                
            }
            if (_hazardBase == null) return;
            _hazardBase.HitTriggerAction += HitTriggerFX;
            _hazardBase.HazardCurrentState.OnValueChanged += (sender, state) => OnCurrentStateChanged(state.Value);
        }

        /// <summary>
        /// Plays FX when hit occurs.
        /// </summary>
        public void HitTriggerFX()
        {
            if (_hazardBase.DestroyOnTrigger)
            {
                var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in renderers)
                {
                    Destroy(meshRenderer);
                }    
            }
            if (HitParticlesPrefab != null)
            {
                // create trigger prefab
                var hazardTriggerParticles = Instantiate(HitParticlesPrefab, transform.position, Quaternion.identity) as GameObject;
                if (hazardTriggerParticles == null) return;
                var maxLifetime = 10f;
                hazardTriggerParticles.transform.parent = GameManager.EffectsContainer.transform;
                var systems = hazardTriggerParticles.GetComponentsInChildren<ParticleSystem>();
                foreach (var system in systems)
                {
                    maxLifetime = Mathf.Max(system.startLifetime, maxLifetime);
                }
                var particleDestroyTimer = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Particle destroy timer");
                particleDestroyTimer.TimerMethod = () =>
                {
                    foreach (var system in systems)
                    {
                        Destroy(system);
                    }
                    DestroyObject(hazardTriggerParticles);
                    if (ParticlesSystemsDoneAction != null)
                    {
                        ParticlesSystemsDoneAction();
                    }
                };
                particleDestroyTimer.Invoke(maxLifetime);
            }

            if (_audioSource != null && HitEffectClip != null)
            {
                _audioSource.PlayOneShot(HitEffectClip);
            }
        }

        /// <summary>
        /// Called when [current state changed].
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        private void OnCurrentStateChanged(HazardState currentState)
        {
            if (currentState == HazardState.Active)
            {
                if (_activeStateParticleSystems != null)
                {
                    foreach (var system in _activeStateParticleSystems)
                    {
                        system.enableEmission = true;
                    }
                }
                if (_rechargingStateParticleSystems != null)
                {
                    foreach (var system in _rechargingStateParticleSystems)
                    {
                        system.enableEmission = false;
                    }
                }
                if (_inactiveStateParticleSystems != null)
                {
                    foreach (var system in _inactiveStateParticleSystems)
                    {
                        system.enableEmission = false;
                    }
                }

                if (_audioSource == null) return;
                if (ActiveStateClip != null)
                {
                    _audioSource.clip = ActiveStateClip;
                    _audioSource.Play();
                }
                else
                {
                    _audioSource.Stop();
                }
            }
            else if (currentState == HazardState.Recharging)
            {
                if (_activeStateParticleSystems != null)
                {
                    foreach (var system in _activeStateParticleSystems)
                    {
                        system.enableEmission = false;
                    }
                }
                if (_rechargingStateParticleSystems != null)
                {
                    foreach (var system in _rechargingStateParticleSystems)
                    {
                        system.enableEmission = true;
                    }
                }
                if (_inactiveStateParticleSystems != null)
                {
                    foreach (var system in _inactiveStateParticleSystems)
                    {
                        system.enableEmission = false;
                    }
                }
                if (_audioSource == null) return;
                if (RechargingStateClip != null)
                {
                    _audioSource.clip = RechargingStateClip;
                    _audioSource.Play();
                }
                else
                {
                    _audioSource.Stop();
                }
            }
            else if (currentState == HazardState.Inactive)
            {
                if (_activeStateParticleSystems != null)
                {
                    foreach (var system in _activeStateParticleSystems)
                    {
                        system.enableEmission = false;
                    }
                }
                if (_rechargingStateParticleSystems != null)
                {
                    foreach (var system in _rechargingStateParticleSystems)
                    {
                        system.enableEmission = false;
                    }
                }
                if (_inactiveStateParticleSystems != null)
                {
                    foreach (var system in _inactiveStateParticleSystems)
                    {
                        system.enableEmission = true;
                    }
                }
                if (_audioSource == null) return;
                if (InactiveStateClip != null)
                {
                    _audioSource.clip = InactiveStateClip;
                    _audioSource.Play();
                }
            }
        }
    }
}
