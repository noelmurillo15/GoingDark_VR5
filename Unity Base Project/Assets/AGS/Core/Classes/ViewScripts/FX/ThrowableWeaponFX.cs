using System;
using AGS.Core.Base;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio used by ProjectileWeapon
    /// </summary>
    public class ThrowableWeaponFX : ViewScriptBase
    {
        
        public AudioClip TriggerClip;
        public AudioClip BounceClip;
        public Object TriggerParticlesPrefab;
        public float TriggerParticlesSecondsToDestroy;

        private AudioSource _audioSource;
        protected ThrowableWeapon ThrowableWeapon;

        public Action ParticlesSystemsDoneAction { get; set; }
        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                var weaponViewRef = ViewReference as ThrowableWeaponBaseView;
                if (weaponViewRef == null)return;

                ThrowableWeapon = weaponViewRef.ThrowableWeapon;

            }
            if (ThrowableWeapon == null) return;
            // Subscribe to actions
            ThrowableWeapon.BounceAction += OnBounceAction;
            ThrowableWeapon.WeaponHitAction += PlayFX;

        }

        /// <summary>
        /// Called when [bounce action].
        /// Play bounce clip whenever throwable bounces off a collider.
        /// </summary>
        private void OnBounceAction()
        {
            _audioSource.PlayOneShot(BounceClip);
        }

        /// <summary>
        /// Creates the hit particle system.
        /// </summary>
        private void CreateHitParticleSystem()
        {
            if (transform == null) return;
            var hitParticleSystemGO = Instantiate(TriggerParticlesPrefab, transform.position, Quaternion.identity) as GameObject;

            if (hitParticleSystemGO == null) return;
            hitParticleSystemGO.transform.parent = GameManager.EffectsContainer.transform;
            SetParticleDestroyTimer(hitParticleSystemGO);
        }

        /// <summary>
        /// Sets the particle destroy timer.
        /// </summary>
        /// <param name="particlesGameObject">The particles game object.</param>
        private void SetParticleDestroyTimer(GameObject particlesGameObject)
        {
            if (particlesGameObject == null) return;
            var systems = particlesGameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var system in systems)
            {
                system.Clear();
                system.Play();
            }
            var particleDestroyTimer = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Particle destroy timer");
            particleDestroyTimer.TimerMethod = () =>
            {
                foreach (var system in systems)
                {
                    Destroy(system);
                }
                Destroy(particlesGameObject);
                if (ParticlesSystemsDoneAction != null)
                {
                    ParticlesSystemsDoneAction();
                }
                
            };
            particleDestroyTimer.Invoke(TriggerParticlesSecondsToDestroy);
        }

        /// <summary>
        /// Play trigger clip whenever throwables HitAction is called, start all particle systems on child objects, and hide throwable weapon renderer.
        /// </summary>
        private void PlayFX()
        {
            _audioSource.PlayOneShot(TriggerClip);
            CreateHitParticleSystem();
        }
    }
}
