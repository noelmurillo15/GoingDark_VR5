using AGS.Core.Base;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles particle systems and audio based on CombatMoves
    /// </summary>
    public class WeaponFX : ViewScriptBase
    {
        private AudioSource _audioSource;
        public AudioClip[] SkillHitClips;
        public Object SkillHitParticlesPrefab;
        private System.Random _rnd;

        private WeaponBaseView _weaponBaseView;
        private WeaponBase _weapon;

        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _rnd = new System.Random();
        }
        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                _weaponBaseView = ViewReference as WeaponBaseView;
                if (_weaponBaseView == null) return;

                _weapon = _weaponBaseView.Weapon;

            }
            if (_weapon == null) return;
            _weapon.WeaponHitAction += PlayHitFX;

        }


        /// <summary>
        /// Plays the hit fx.
        /// </summary>
        private void PlayHitFX()
        {
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(SkillHitClips[_rnd.Next(SkillHitClips.Length)], 0.5f);
            }

            if (_weapon.OwnerCombatEntity.Value.CurrentWeapon.Value == null
                ||
                _weapon.OwnerCombatEntity.Value.CurrentWeapon.Value.CurrentHitVolume.Value == null)
            {
                return;
            }
            var skillHitParticleSystem = Instantiate(SkillHitParticlesPrefab,
                _weapon.OwnerCombatEntity.Value.CurrentWeapon.Value.CurrentHitVolume.Value.Transform.position,
                Mathf.Abs(_weapon.OwnerCombatEntity.Value.Transform.forward.z - 1) < 0.1
                    ? Quaternion.Euler(0f, 180f, 0f)
                    : Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            if (skillHitParticleSystem == null)
            {
                return;
            }
            skillHitParticleSystem.transform.parent = GameManager.EffectsContainer.transform;
            var particleDestroyTimer =
                MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(
                    GameManager.TemporaryTimerComponents, "Particle destroy timer");
            particleDestroyTimer.TimerMethod = () => DestroyObject(skillHitParticleSystem);
            particleDestroyTimer.Invoke(0.5f);
        }
    }
}
