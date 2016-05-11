using AGS.Core.Base;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio used by ProjectileWeapon
    /// </summary>
    public class ProjectileWeaponFX : ViewScriptBase
    {
        public AudioClip FiringClip;
        public Object FiringParticlesPrefab;
        public Vector3 FiringParticlesOffset;
        public float FiringParticlesSecondsToDestroy;

        private AudioSource _audioSource;
        private CombatEntityBase _ownerCombatEntity;
        protected ProjectileWeapon ProjectileWeapon;

        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                var weaponViewRef = ViewReference as ProjectileWeaponView;
                if (weaponViewRef == null) return;

                ProjectileWeapon = weaponViewRef.ProjectileWeapon;

            }
            if (ProjectileWeapon == null) return;
            // Subscribe to actions
            ProjectileWeapon.ProjectileFiredAction += OnProjectileFired;
            _ownerCombatEntity = ProjectileWeapon.OwnerCombatEntity.Value;

        }

        void OnDestroy()
        {
            // Unsubscribe to actions
            if (ProjectileWeapon != null)
            {
                ProjectileWeapon.ProjectileFiredAction -= OnProjectileFired;   
            }
            
        }

        /// <summary>
        /// Called when [projectile fired].
        /// </summary>
        /// <param name="projectile">The projectile.</param>
        private void OnProjectileFired(Projectile projectile)
        {
            if (FiringClip != null)
            {
                _audioSource.PlayOneShot(FiringClip, 1f);
            }
            if (FiringParticlesPrefab != null)
            {

                var firingParticlesGO = Instantiate(FiringParticlesPrefab, ProjectileWeapon.ProjectileSpawnPosition.position + FiringParticlesOffset * _ownerCombatEntity.Transform.forward.z, Quaternion.LookRotation(_ownerCombatEntity.CurrentWeapon.Value.Transform.forward, Vector3.up)) as GameObject;
                if (firingParticlesGO == null) return;
                firingParticlesGO.transform.parent = _ownerCombatEntity.CurrentWeapon.Value.Transform;
                SetParticleDestroyTimer(firingParticlesGO);
            }
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
            particleDestroyTimer.Invoke(FiringParticlesSecondsToDestroy);
        }
    }
}
