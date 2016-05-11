using System;
using AGS.Core.Base;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio used by Projectile
    /// </summary>
    public class ProjectileFX : ViewScriptBase
    {
        public AudioClip HitClip;

        public float HitParticlesSecondsToDestroy;

        private AudioSource _audioSource;
        private ProjectileWeapon _ownerCombatEntity;
        protected Projectile Projectile;

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
                var projectileViewRef = ViewReference as ProjectileBaseView;
                if (projectileViewRef == null) return;

                Projectile = projectileViewRef.Projectile;

            }
            if (Projectile == null) return;
            // Subscribe to actions
            Projectile.ProjectileDestroyAction += (projectileHitData) =>
            {
                var systems = GetComponentsInChildren<ParticleSystem>();
                foreach (var system in systems)
                {
                    Destroy(system);
                }
                PlayFX(projectileHitData);
            };
            _ownerCombatEntity = Projectile.OwnerProjectileWeapon.Value;

        }

        /// <summary>
        /// Called when [projectile destroyed].
        /// </summary>
        /// <param name="hitInfo">The hit information.</param>
        private void PlayFX(ProjectileHitData hitInfo)
        {
            if (hitInfo == null) return;
            if (HitClip != null)
            {
                _audioSource.PlayOneShot(HitClip);
            }
            CreateHitParticleSystem(hitInfo);
        }

        /// <summary>
        /// Creates the hit particle system.
        /// </summary>
        /// <param name="hitInfo">The hit information.</param>
        private void CreateHitParticleSystem(ProjectileHitData hitInfo)
        {
            if (transform == null) return;
            var projectileHitFX = (GameObject)Resources.Load(string.Format("FXEffects/ProjectileHitEffects/{0}{1}", _ownerCombatEntity.LoadedProjectileSubType, _ownerCombatEntity.LoadedProjectileType));
           
            if (projectileHitFX == null) return;
            var hitParticleSystemGO = Instantiate(projectileHitFX, hitInfo.HitTransform.position, Quaternion.LookRotation(-hitInfo.ProjectileDirection)) as GameObject;
            if (hitParticleSystemGO == null) return;
            var systems = hitParticleSystemGO.GetComponentsInChildren<ParticleSystem>();
            foreach (var system in systems)
            {
                system.Clear();
                system.Play();
            }
            var particleDestroyTimer = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Particle destroy timer");
            particleDestroyTimer.TimerMethod = () =>
            {
                foreach (var system in systems)
                {
                    Destroy(system);
                }
                Destroy(hitParticleSystemGO);
                if (ParticlesSystemsDoneAction != null)
                {
                    ParticlesSystemsDoneAction();
                }

            };
            particleDestroyTimer.Invoke(HitParticlesSecondsToDestroy);
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
            };
            particleDestroyTimer.Invoke(HitParticlesSecondsToDestroy);
        }
    }
}
